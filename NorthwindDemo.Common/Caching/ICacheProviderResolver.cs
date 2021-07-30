using NorthwindDemo.Common.Enum;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// Interface ICacheProviderResolver
    /// </summary>
    public interface ICacheProviderResolver
    {
        /// <summary>
        /// Get the CacheProvider.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <returns>ICacheProvider.</returns>
        ICacheProvider GetCacheProvider(CacheTypeEnum cacheType);
    }
}