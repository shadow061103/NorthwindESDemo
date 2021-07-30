using CoreProfiler.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NorthwindDemo.Api.Infrastructure.Extensions;
using NorthwindDemo.Common;
using NorthwindDemo.Common.Caching;
using NorthwindDemo.Repository.Decorators.MemoryCache;
using NorthwindDemo.Repository.Decorators.Redis;
using NorthwindDemo.Repository.Implements;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.Context;
using NorthwindDemo.Service.Implements;
using NorthwindDemo.Service.Interfaces;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthwindDemo.Api
{
    public class Startup
    {
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
            {
                //ViewModel �P Parameter ��ܬ��p�m�p�R�W
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                //�C�|��ܬ����Ѥ�r
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            #region swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "NorthwindDemo.Api",
                    Description = "This is NorthwindDemo ASP.NET Core 3.1 RESTful API."
                });

                var basePath = AppContext.BaseDirectory;
                var xmlFiles = Directory.EnumerateFiles(basePath, $"*.xml", SearchOption.TopDirectoryOnly);

                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile, true);
                }
            });

            #endregion swagger

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //EF core
            services//.AddEntityFrameworkSqlServer()
                   .AddDbContext<DbContext, NorthwindContext>(options => options
                    .UseLoggerFactory(_loggerFactory)
                    .UseSqlServer(Configuration.GetConnectionString("Northwind")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();

            services.AddElasticsearch(Configuration);

            #region �֨�

            var cacheDecoratorSetingsSection = this.Configuration.GetSection(CacheDecoratorSettingsOptions.SectionName);
            services.Configure<CacheDecoratorSettingsOptions>(cacheDecoratorSetingsSection);

            services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
                options.CompactionPercentage = 0.02d;
            });

            services.AddRedisCache();

            // CacheProviderResolver
            services.AddSingleton<ICacheProviderResolver, CacheProviderResolver>();

            // IMemoryCacheRemoveHelper
            services.AddScoped<IMemoryCacheRemoveHelper, MemoryCacheRemoveHelper>();

            // IRedisCacheRemoveHelper
            services.AddScoped<IRedisCacheHelper, RedisCacheHelper>();

            #endregion �֨�

            //DI
            services.AddScoped<IOrderESRepository, OrderESRepository>()
                .Decorate<IOrderESRepository, RedisOrderRepository>()
                .Decorate<IOrderESRepository, CacheOrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderESService, OrderESService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                    // url: �ݰt�X SwaggerDoc �� name�C "/swagger/{SwaggerDoc name}/swagger.json"
                    url: "./v1/swagger.json",
                    // description: �Ω� Swagger UI �k�W����ܤ��P������ SwaggerDocument ��ܦW�٨ϥΡC
                    name: "northwind v1.0.0"
                );
            });

            #endregion swagger

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