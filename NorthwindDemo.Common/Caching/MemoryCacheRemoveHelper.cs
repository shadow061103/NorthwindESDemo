using NorthwindDemo.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// class MemoryCacheRemoveHelper
    /// </summary>
    public class MemoryCacheRemoveHelper : IMemoryCacheRemoveHelper
    {
        private readonly ICacheProvider _cacheProvider;

        public MemoryCacheRemoveHelper(ICacheProviderResolver cacheProviderResolver)
        {
            this._cacheProvider = cacheProviderResolver.GetCacheProvider(CacheTypeEnum.Memory);
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        public bool RemoveCacheItem(string cachekey)
        {
            if (this._cacheProvider.Exists(cachekey).Equals(false))
            {
                return false;
            }

            var result = this._cacheProvider.Remove(cachekey);

            return result;
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public void RemoveCacheItem(string cachekey, object primaryKey)
        {
            var keys = new List<string> { cachekey };

            var collection = MemoryCacheProvider.Cachekeys
                                                .Where(x => x.Contains(primaryKey.ToString(), StringComparison.OrdinalIgnoreCase))
                                                .ToList();

            keys.AddRange(collection);

            foreach (var key in keys)
            {
                if (this._cacheProvider.Exists(key).Equals(false))
                {
                    continue;
                }

                this._cacheProvider.Remove(key);
            }
        }

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        public void RemoveCacheItemByKeyPrefix(string keyPrefix)
        {
            var keys = new List<string>();

            var collection = MemoryCacheProvider.Cachekeys
                                                .Where(x => x.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
                                                .ToList();

            keys.AddRange(collection);

            foreach (var key in keys)
            {
                if (this._cacheProvider.Exists(key).Equals(false))
                {
                    continue;
                }

                this._cacheProvider.Remove(key);
            }
        }
    }
}