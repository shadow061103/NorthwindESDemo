﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// Class CacheProviderResolver. Implements the <see cref="ICacheProviderResolver"/>
    /// </summary>
    /// <seealso cref="ICacheProviderResolver"/>
    public class CacheProviderResolver : ICacheProviderResolver
    {
        private IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProviderResolver"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CacheProviderResolver(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Get the CacheProvider.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <returns>ICacheProvider.</returns>
        public ICacheProvider GetCacheProvider(CacheTypeEnum cacheType)
        {
            if (cacheType.Equals(null))
            {
                return new NullCacheProvider();
            }

            ICacheProvider cacheProvider;

            switch (cacheType)
            {
                case CacheTypeEnum.None:
                    cacheProvider = new NullCacheProvider();
                    break;

                case CacheTypeEnum.Memory:
                    cacheProvider = new MemoryCacheProvider(this.ServiceProvider.GetService<IMemoryCache>());
                    break;

                case CacheTypeEnum.Redis:
                    cacheProvider = new RedisCacheProvider(this.ServiceProvider.GetService<IDistributedCache>(), TimeSpan.FromHours(1));
                    break;

                default:
                    cacheProvider = new NullCacheProvider();
                    break;
            }

            return cacheProvider;
        }
    }
}