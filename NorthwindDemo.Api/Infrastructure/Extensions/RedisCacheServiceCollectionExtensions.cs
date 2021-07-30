using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace NorthwindDemo.Api.Infrastructure.Extensions
{
    public static class RedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(services.GetGetConfigString());
            });

            //distributed cache 以下兩種擇一使用

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = services.GetGetConfigString();
                options.InstanceName = "RedisShakeHand";
            });

            return services;
        }

        private static string GetGetConfigString(this IServiceCollection services)
        {
            return "127.0.0.1:6379,syncTimeout=8000";
        }
    }
}