using CoreProfiler.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NorthwindDemo.Api.Infrastructure.Extensions;
using NorthwindDemo.Repository.Implements;
using NorthwindDemo.Repository.Interfaces;
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

            services.AddElasticsearch(Configuration);

            //DI
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderESRepository, OrderESRepository>();
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