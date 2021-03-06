using CoreProfiler.Web;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NorthwindDemo.Repository.Implements;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.Context;
using NorthwindDemo.Service.Implements;
using NorthwindDemo.Service.Interfaces;
using NorthwindDemo.Task.Infrastructure.Extensions;
using NorthwindDemo.Task.Infrastructure.HangfireMisc;
using NorthwindDemo.Task.Interfaces;
using NorthwindDemo.Task.Jobs;
using System;
using System.IO;
using System.Linq;

namespace NorthwindDemo.Task
{
    public class Startup
    {
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public IConfiguration Configuration { get; }
        private HangfireSettings HangfireSettings { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "NorthwindDemo.Api",
                    Description = "This is NorthwindDemo Task ASP.NET Core 3.1 RESTful API."
                });

                var basePath = AppContext.BaseDirectory;
                var xmlFiles = Directory.EnumerateFiles(basePath, $"*.xml", SearchOption.TopDirectoryOnly);

                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile, true);
                }
            });

            #endregion swagger

            #region Hangfire

            var hangfireConnection = Configuration.GetConnectionString("Hangfire");
            // HangfireSettings
            var hangfireSettings = new HangfireSettings();
            this.Configuration.GetSection("HangfireSettings").Bind(hangfireSettings);
            this.HangfireSettings = hangfireSettings;

            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage
                  (
                    nameOrConnectionString: hangfireConnection,
                    options: new SqlServerStorageOptions
                    {
                        SchemaName = hangfireSettings.SchemaName,
                        //????????Table ?????n????False
                        PrepareSchemaIfNecessary = hangfireSettings.PrepareSchemaIfNecessary,
                        JobExpirationCheckInterval = TimeSpan.FromMinutes(60)//???d?W?L?O?s????????
                    }
                  );
                x.UseConsole();
                x.UseDashboardMetric(SqlServerStorage.ActiveConnections);
                x.UseDashboardMetric(SqlServerStorage.TotalConnections);
                x.UseDashboardMetric(DashboardMetrics.RecurringJobCount);
                x.UseDashboardMetric(DashboardMetrics.ScheduledCount);
                x.UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount);
                x.UseDashboardMetric(DashboardMetrics.DeletedCount);
                x.UseDashboardMetric(DashboardMetrics.RetriesCount);
                x.UseDashboardMetric(DashboardMetrics.ProcessingCount);
                x.UseDashboardMetric(DashboardMetrics.FailedCount);
                x.UseDashboardMetric(DashboardMetrics.SucceededCount);
            });

            #endregion Hangfire

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DbContext, NorthwindContext>(options => options
                     .UseLoggerFactory(_loggerFactory)
                     .UseSqlServer(Configuration.GetConnectionString("Northwind")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();

            services.AddElasticsearch(Configuration);

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //DI
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderESRepository, OrderESRepository>();
            services.AddScoped<IOrderESService, OrderESService>();
            services.AddScoped<IOrdersESJob, OrdersESJob>();
            services.AddSingleton<IHangfireJobTrigger, HangfireJobTrigger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    // url: ???t?X SwaggerDoc ?? name?C "/swagger/{SwaggerDoc name}/swagger.json"
                    url: "./v1/swagger.json",
                    // description: ???? Swagger UI ?k?W?????????P?????? SwaggerDocument ?????W???????C
                    name: "northwind v1.0.0"
                );
            });

            #endregion swagger

            #region Hangfire

            var queues = this.HangfireSettings.Queues.Any()
                ? this.HangfireSettings.Queues
                : new[] { "default" };

            app.UseHangfireServer
            (
                options: new BackgroundJobServerOptions
                {
                    ServerName = $"{Environment.MachineName}:{HangfireSettings.ServerName}",
                    WorkerCount = this.HangfireSettings.WorkerCount,
                    Queues = queues
                }
             );
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

            app.UseHangfireDashboard
            (
                pathMatch: "/hangfire",
                options: new DashboardOptions
                {
                    Authorization = new[]
                    {
                            new HangfireAuthorizeFilter
                            (
                                httpContextAccessor,
                                this.HangfireSettings.DashboardUsers
                            )
                    },
                    IgnoreAntiforgeryToken = true
                }
            );
            if (this.HangfireSettings.EnableRecurringJob)
            {
                // ?????C?????{?u?@ ?????????}??
                serviceProvider.GetService<IHangfireJobTrigger>().OnStart();
            }

            #endregion Hangfire

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCoreProfiler(true);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}