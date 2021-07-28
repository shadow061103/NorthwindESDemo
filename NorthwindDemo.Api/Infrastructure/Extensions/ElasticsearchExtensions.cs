
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using NorthwindDemo.Repository.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Api.Infrastructure.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        { 
            var setting= configuration.GetSection("elasticsearch").Get<ElasticSearchSetting>();

            var nodeUris = setting.Nodes.Select(x => new Uri(x));

            var connectionPool = new StaticConnectionPool(nodeUris);

            var connectionSetting = new ConnectionSettings(connectionPool)
                .DisableAutomaticProxyDetection()
                .DisableDirectStreaming();

            var client = new ElasticClient(connectionSetting);

            var index = "northwind_orders";
            var temp = client.Indices.Exists(index).Exists;
            if (client.Indices.Exists(index).Exists.Equals(false))
            {
                client.Indices.Create(index, i => i
                .Map<Orders>(m=>m
                .AutoMap()).Settings(
                    index=>index.Sorting<Orders>(
                        sort=>sort.Fields(f=>f.Field(y=>y.OrderId)).Order(IndexSortOrder.Descending)
                        )));

            }

            services.AddSingleton<IElasticClient>(client);

        }
    }
    public class ElasticSearchSetting
    {
        public IList<string> Nodes { get; set; }
    }
}
