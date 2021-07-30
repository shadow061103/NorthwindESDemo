using NorthwindDemo.Common.Attribute;

namespace NorthwindDemo.Common.Enum
{
    /// <summary>
    /// Enum CacheTypeEnum
    /// </summary>
    public enum CacheTypeEnum
    {
        /// <summary>
        /// None - NullCacheProvider
        /// </summary>
        [CacheDescription("NullCacheProvider")]
        None = 0,

        /// <summary>
        /// Memory - MemoryCacheProvider
        /// </summary>
        [CacheDescription("MemoryCacheProvider")]
        Memory = 1,

        /// <summary>
        /// Redis - RedisCacheProvider
        /// </summary>
        [CacheDescription("RedisCacheProvider")]
        Redis = 2
    }
}