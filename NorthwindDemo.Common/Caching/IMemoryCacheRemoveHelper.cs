namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// interface IMemoryCacheRemoveHelper
    /// </summary>
    public interface IMemoryCacheRemoveHelper
    {
        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        bool RemoveCacheItem(string cachekey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        void RemoveCacheItem(string cachekey, object primaryKey);

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        void RemoveCacheItemByKeyPrefix(string keyPrefix);
    }
}