using System;
using System.Threading.Tasks;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// interface IRedisCacheHelper
    /// </summary>
    public interface IRedisCacheHelper
    {
        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        bool RemoveCacheItem(string cachekey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        Task<bool> RemoveCacheItemAsync(string cachekey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        void RemoveCacheItem(string cachekey, object primaryKey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        Task RemoveCacheItemAsync(string cachekey, object primaryKey);

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        void RemoveCacheItemByKeyPrefix(string keyPrefix);

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <returns></returns>
        Task RemoveCacheItemByKeyPrefixAsync(string keyPrefix);

        /// <summary>
        /// 清除所有的 Cache 資料
        /// </summary>
        void FlushDatabase();

        /// <summary>
        /// 清除所有的 Cache 資料
        /// </summary>
        /// <returns></returns>
        Task FlushDatabaseAsync();

        /// <summary>
        /// 將 Cachekey 存放到 Hash 裡
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <param name="timeSpan">timeSpan</param>
        void HashSet(string cachekey, TimeSpan timeSpan);

        /// <summary>
        /// 將 Cachekey 存放到 Hash 裡
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <param name="timeSpan">timeSpan</param>
        /// <returns></returns>
        Task HashSetAsync(string cachekey, TimeSpan timeSpan);

        /// <summary>
        /// 將 Cachekey 從 Hash 裡移除
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        void HashDelete(string cachekey);

        /// <summary>
        /// 將 Cachekey 從 Hash 裡移除
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <returns></returns>
        Task HashDeleteAsync(string cachekey);

        /// <summary>
        /// 清除 Hash 裡已超過存續時間的 Cachekey.
        /// </summary>
        /// <returns>回傳清除的 cachekey 數目</returns>
        int HashClean();

        /// <summary>
        /// 清除 Hash 裡已超過存續時間的 Cachekey.
        /// </summary>
        /// <returns>回傳清除的 cachekey 數目</returns>
        Task<int> HashCleanAsync();

        /// <summary>
        /// 取得 Hash 內的 Cachekey 總數
        /// </summary>
        /// <returns></returns>
        long HashLength();

        /// <summary>
        /// 取得 Hash 內的 Cachekey 總數
        /// </summary>
        /// <returns></returns>
        Task<long> HashLengthAsync();
    }
}