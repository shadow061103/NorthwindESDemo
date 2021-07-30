using Microsoft.Extensions.Options;
using NorthwindDemo.Common;
using NorthwindDemo.Common.Caching;
using NorthwindDemo.Common.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Repository.Decorators
{
    /// <summary>
    /// class CachedRepositoryBase
    /// </summary>
    public abstract class CachedRepositoryBase
    {
        protected ICacheProvider CacheProvider { get; private set; }

        private CacheDecoratorSettingsOptions CacheDecoratorSettingsOptions { get; set; }

        private ICacheProviderResolver CacheProviderResolver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepositoryBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="cacheProviderResolver">The cacheProviderResolver.</param>
        protected CachedRepositoryBase(IOptions<CacheDecoratorSettingsOptions> options,
                                       ICacheProviderResolver cacheProviderResolver)
        {
            this.CacheDecoratorSettingsOptions = options.Value;
            this.CacheProviderResolver = cacheProviderResolver;
        }

        /// <summary>
        /// Sets the CacheProvider.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="declaration">The declaration.</param>
        /// <param name="implement">The implement.</param>
        protected void SetCacheProvider(CacheTypeEnum cacheType, string declaration, string implement)
        {
            var cacheProviders = this.CacheDecoratorSettingsOptions.CacheProviders;
            var cacheDecorators = this.CacheDecoratorSettingsOptions.CacheDecorators;

            var isDefinedCacheProvider = cacheProviders.Any(x => x.Equals(cacheType.EnumDescription(), StringComparison.OrdinalIgnoreCase));
            if (isDefinedCacheProvider.Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            var decorator = cacheDecorators.FirstOrDefault(x => x.Declaration.Equals(declaration, StringComparison.OrdinalIgnoreCase));
            if (decorator is null)
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            if (decorator.Implements is null || decorator.Implements.Any().Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            if (decorator.Implements.Any(x => x.Equals(implement, StringComparison.OrdinalIgnoreCase)).Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(cacheType);
        }

        /// <summary>
        /// 取得(建立)快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected abstract T GetOrAddCacheItem<T>(string cachekey, TimeSpan cacheItemExpiration, Func<T> source);

        /// <summary>
        /// 取得(建立)快取資料 asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected abstract Task<T> GetOrAddCacheItemAsync<T>(string cachekey, TimeSpan cacheItemExpiration, Func<Task<T>> source);

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        protected abstract bool RemoveCacheItem(string cachekey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract void RemoveCacheItem(string cachekey, object primaryKey);

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        protected abstract void RemoveCacheItemByKeyPrefix(string keyPrefix);
    }
}