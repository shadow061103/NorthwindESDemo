using NorthwindDemo.Common.Models;
using System.Collections.Generic;

namespace NorthwindDemo.Common
{
    /// <summary>
    /// Class CacheDecoratorSettingsOptions.
    /// </summary>
    public class CacheDecoratorSettingsOptions : INullObject
    {
        public const string SectionName = "CacheDecoratorSettings";

        private static CacheDecoratorSettingsOptions _null;

        public static CacheDecoratorSettingsOptions Null
        {
            get
            {
                if (_null is null)
                {
                    _null = new NullCacheDecoratorSettings();
                }

                return _null;
            }
        }

        /// <summary>
        /// CacheProvider: NullCacheProvider, MemoryCacheProvider, RedisCacheProvider.
        /// </summary>
        public string[] CacheProviders { get; set; }

        /// <summary>
        /// 快取裝飾者的定義與實做.
        /// </summary>
        public CacheDecorator[] CacheDecorators { get; set; }

        //-----------------------------------------------------------------------------------------
        // Null

        public virtual bool IsNull()
        {
            return false;
        }

        private class NullCacheDecoratorSettings : CacheDecoratorSettingsOptions
        {
            public NullCacheDecoratorSettings()
            {
                this.CacheProviders = new[] { "NullCacheProvider" };
                this.CacheDecorators = new List<CacheDecorator>().ToArray();
            }

            public override bool IsNull()
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Class CacheDecorator.
    /// </summary>
    public class CacheDecorator
    {
        /// <summary>
        /// 定義 (interface name)
        /// </summary>
        public string Declaration { get; set; }

        /// <summary>
        /// 實做類別名稱
        /// </summary>
        public string[] Implements { get; set; }
    }
}