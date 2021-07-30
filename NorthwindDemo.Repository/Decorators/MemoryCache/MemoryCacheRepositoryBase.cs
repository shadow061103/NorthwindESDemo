﻿using CoreProfiler;
using Microsoft.Extensions.Options;
using NorthwindDemo.Common;
using NorthwindDemo.Common.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Repository.Decorators.MemoryCache
{
    /// <summary>
    /// Class MemoryCacheRepository. Implements the <see cref="CachedRepositoryBase"/>
    /// </summary>
    /// <seealso cref="CachedRepositoryBase"/>
    public abstract class MemoryCacheRepositoryBase : CachedRepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheRepositoryBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="cacheProviderResolver">The cacheProviderResolver.</param>
        protected MemoryCacheRepositoryBase(IOptions<CacheDecoratorSettingsOptions> options,
                                            ICacheProviderResolver cacheProviderResolver)
            : base(options, cacheProviderResolver)
        {
        }

        /// <summary>
        /// 取得(建立)快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override T GetOrAddCacheItem<T>(string cachekey, TimeSpan cacheItemExpiration, Func<T> source)
        {
            var stepName = $"{nameof(MemoryCacheRepositoryBase)}.{nameof(this.GetOrAddCacheItem)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (this.CacheProvider.Exists(cachekey))
                {
                    var cachedValue = this.CacheProvider.Get<T>(cachekey);
                    if (cachedValue != null)
                    {
                        return cachedValue;
                    }
                }

                var returnResult = source();

                if (returnResult is null)
                {
                    return returnResult;
                }

                this.CacheProvider.Save(key: cachekey, value: returnResult, cacheTime: cacheItemExpiration);

                return returnResult;
            }
        }

        /// <summary>
        /// 取得(建立)快取資料 asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override async Task<T> GetOrAddCacheItemAsync<T>(string cachekey, TimeSpan cacheItemExpiration, Func<Task<T>> source)
        {
            var stepName = $"{nameof(MemoryCacheRepositoryBase)}.{nameof(this.GetOrAddCacheItemAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (this.CacheProvider.Exists(cachekey))
                {
                    var cachedValue = this.CacheProvider.Get<T>(cachekey);
                    if (cachedValue != null)
                    {
                        return cachedValue;
                    }
                }

                var returnResult = await source();

                if (returnResult is null)
                {
                    return returnResult;
                }

                this.CacheProvider.Save(key: cachekey, value: returnResult, cacheTime: cacheItemExpiration);

                return returnResult;
            }
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        protected override bool RemoveCacheItem(string cachekey)
        {
            var stepName = $"{nameof(MemoryCacheRepositoryBase)}.{nameof(this.RemoveCacheItem)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (this.CacheProvider.Exists(cachekey).Equals(false))
                {
                    return false;
                }

                var result = this.CacheProvider.Remove(cachekey);

                return result;
            }
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override void RemoveCacheItem(string cachekey, object primaryKey)
        {
            var stepName = $"{nameof(MemoryCacheRepositoryBase)}.{nameof(this.RemoveCacheItem)}-2";
            using (ProfilingSession.Current.Step(stepName))
            {
                var keys = new List<string> { cachekey };

                var collection = MemoryCacheProvider.Cachekeys
                                                    .Where(x => x.Contains(primaryKey.ToString(), StringComparison.OrdinalIgnoreCase))
                                                    .ToList();

                keys.AddRange(collection);

                foreach (var key in keys)
                {
                    if (this.CacheProvider.Exists(key).Equals(false))
                    {
                        continue;
                    }

                    this.CacheProvider.Remove(key);
                }
            }
        }

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        protected override void RemoveCacheItemByKeyPrefix(string keyPrefix)
        {
            var stepName = $"{nameof(MemoryCacheRepositoryBase)}.{nameof(this.RemoveCacheItemByKeyPrefix)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var keys = new List<string>();

                var collection = MemoryCacheProvider.Cachekeys
                                                    .Where(x => x.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
                                                    .ToList();

                keys.AddRange(collection);

                foreach (var key in keys.Where(key => this.CacheProvider.Exists(key).Equals(true)))
                {
                    this.CacheProvider.Remove(key);
                }
            }
        }
    }
}