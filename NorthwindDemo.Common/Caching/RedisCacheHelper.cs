using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// class RedisCacheRemoveHelper (dependency on StackExchange.Redis)
    /// </summary>
    /// <seealso cref="IRedisCacheHelper"/>
    public class RedisCacheHelper : IRedisCacheHelper
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private RedisCacheOptions _redisConfigurationOptions;

        private IConnectionMultiplexer _connection;

        private RedisCacheOptions RedisConfigurationOptions
        {
            get
            {
                if (this._redisConfigurationOptions != null)
                {
                    return this._redisConfigurationOptions;
                }

                var scope = this._serviceScopeFactory.CreateScope();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<RedisCacheOptions>>();
                this._redisConfigurationOptions = options.Value;
                return this._redisConfigurationOptions;
            }
        }

        private IConnectionMultiplexer Connection
        {
            get
            {
                if (this._connection != null)
                {
                    return this._connection;
                }

                var scope = this._serviceScopeFactory.CreateScope();
                this._connection = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
                return this._connection;
            }
        }

        private IDatabase Database => this.Connection.GetDatabase();

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheHelper"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The serviceScopeFactory.</param>
        public RedisCacheHelper(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        public bool RemoveCacheItem(string cachekey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var result = false;

            var keyExists = this.Database.KeyExists(redisCachekey);
            if (keyExists.Equals(true))
            {
                result = this.Database.KeyDelete(redisCachekey);
            }

            this.HashDelete(redisCachekey);

            return result;
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        public async Task<bool> RemoveCacheItemAsync(string cachekey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var result = false;

            var keyExists = await this.Database.KeyExistsAsync(redisCachekey);
            if (keyExists.Equals(true))
            {
                result = await this.Database.KeyDeleteAsync(redisCachekey);
            }

            await this.HashDeleteAsync(redisCachekey);

            return result;
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        public void RemoveCacheItem(string cachekey, object primaryKey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var keyExists = this.Database.KeyExists(redisCachekey);
            if (keyExists.Equals(true))
            {
                this.Database.KeyDelete(redisCachekey);
                this.HashDelete(redisCachekey);
            }

            var patternMatch = $"*{primaryKey}*";
            var scanResult = this.HashScan(patternMatch);
            foreach (var item in scanResult)
            {
                this.Database.KeyDelete(item);
                this.HashDelete(item);
            }
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public async Task RemoveCacheItemAsync(string cachekey, object primaryKey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var keyExists = await this.Database.KeyExistsAsync(redisCachekey);
            if (keyExists.Equals(true))
            {
                await this.Database.KeyDeleteAsync(redisCachekey);
                await this.HashDeleteAsync(redisCachekey);
            }

            var patternMatch = $"*{primaryKey}*";
            var scanResult = this.HashScan(patternMatch);
            foreach (var item in scanResult)
            {
                await this.Database.KeyDeleteAsync(item);
                await this.HashDeleteAsync(item);
            }
        }

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        public void RemoveCacheItemByKeyPrefix(string keyPrefix)
        {
            var patternMatch = $"{keyPrefix}*";
            var scanResult = this.HashScan(patternMatch);
            foreach (var item in scanResult)
            {
                this.Database.KeyDelete(item);
                this.HashDelete(item);
            }
        }

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <returns></returns>
        public async Task RemoveCacheItemByKeyPrefixAsync(string keyPrefix)
        {
            var patternMatch = $"{keyPrefix}*";
            var scanResult = this.HashScan(patternMatch);
            foreach (var item in scanResult)
            {
                await this.Database.KeyDeleteAsync(item);
                await this.HashDeleteAsync(item);
            }
        }

        /// <summary>
        /// 清除所有的 Cache 資料
        /// </summary>
        public void FlushDatabase()
        {
            var hashEntries = this.Database.HashGetAll(this.RedisConfigurationOptions.InstanceName);
            foreach (var entry in hashEntries)
            {
                this.RemoveCacheItem(entry.Name);
            }
        }

        /// <summary>
        /// 清除所有的 Cache 資料
        /// </summary>
        /// <returns></returns>
        public async Task FlushDatabaseAsync()
        {
            var hashEntries = await this.Database.HashGetAllAsync(this.RedisConfigurationOptions.InstanceName);
            foreach (var entry in hashEntries)
            {
                await this.RemoveCacheItemAsync(entry.Name);
            }
        }

        /// <summary>
        /// 將 Cachekey 存放到 Hash 裡
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <param name="timeSpan">timeSpan</param>
        public void HashSet(string cachekey, TimeSpan timeSpan)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var value = DateTime.UtcNow.Add(timeSpan).Ticks;

            this.Database.HashSet(
                key: this.RedisConfigurationOptions.InstanceName,
                hashField: redisCachekey,
                value: value.ToString(),
                when: When.Always,
                flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 將 Cachekey 存放到 Hash 裡
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <param name="timeSpan">timeSpan</param>
        /// <returns></returns>
        public async Task HashSetAsync(string cachekey, TimeSpan timeSpan)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            var value = DateTime.UtcNow.Add(timeSpan).Ticks;

            await this.Database.HashSetAsync(
                key: this.RedisConfigurationOptions.InstanceName,
                hashField: redisCachekey,
                value: value.ToString(),
                when: When.Always,
                flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 將 Cachekey 從 Hash 裡移除
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        public void HashDelete(string cachekey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            this.Database.HashDelete(
                this.RedisConfigurationOptions.InstanceName,
                redisCachekey,
                CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 將 Cachekey 從 Hash 裡移除
        /// </summary>
        /// <param name="cachekey">cachekey</param>
        /// <returns></returns>
        public async Task HashDeleteAsync(string cachekey)
        {
            var redisCachekey = this.BuildCachekey(cachekey);

            await this.Database.HashDeleteAsync(
                this.RedisConfigurationOptions.InstanceName,
                redisCachekey,
                CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 清除 Hash 裡以超過存續時間的 Cachekey.
        /// </summary>
        public int HashClean()
        {
            var hashEntries = this.Database.HashGetAll(this._redisConfigurationOptions.InstanceName);
            var cleanCount = 0;
            var currentTicks = DateTime.UtcNow.Ticks;

            foreach (var item in hashEntries)
            {
                if (long.TryParse(item.Value, out var value).Equals(false))
                {
                    continue;
                }

                if (value >= currentTicks)
                {
                    continue;
                }

                var cachekey = $"{item.Name}";
                if (string.IsNullOrWhiteSpace(cachekey))
                {
                    continue;
                }

                var keyExists = this.Database.KeyExists(cachekey);
                if (keyExists.Equals(true))
                {
                    continue;
                }

                this.Database.HashDelete(this._redisConfigurationOptions.InstanceName, cachekey, CommandFlags.FireAndForget);
                cleanCount++;
            }

            return cleanCount;
        }

        /// <summary>
        /// 清除 Hash 裡已超過存續時間的 Cachekey.
        /// </summary>
        /// <returns>回傳清除的 cachekey 數目</returns>
        public async Task<int> HashCleanAsync()
        {
            var hashEntries = await this.Database.HashGetAllAsync(this._redisConfigurationOptions.InstanceName);
            var cleanCount = 0;
            var currentTicks = DateTime.UtcNow.Ticks;

            foreach (var item in hashEntries)
            {
                if (long.TryParse(item.Value, out var value).Equals(false))
                {
                    continue;
                }

                if (value >= currentTicks)
                {
                    continue;
                }

                var cachekey = $"{item.Name}";
                if (string.IsNullOrWhiteSpace(cachekey))
                {
                    continue;
                }

                var keyExists = await this.Database.KeyExistsAsync(cachekey);
                if (keyExists.Equals(true))
                {
                    continue;
                }

                await this.Database.HashDeleteAsync(this._redisConfigurationOptions.InstanceName, cachekey, CommandFlags.FireAndForget);
                cleanCount++;
            }

            return cleanCount;
        }

        /// <summary>
        /// 取得 Hash 內的 Cachekey 總數
        /// </summary>
        /// <returns></returns>
        public long HashLength()
        {
            var length = this.Database.HashLength(this.RedisConfigurationOptions.InstanceName);
            return length;
        }

        /// <summary>
        /// 取得 Hash 內的 Cachekey 總數
        /// </summary>
        /// <returns></returns>
        public async Task<long> HashLengthAsync()
        {
            var length = await this.Database.HashLengthAsync(this.RedisConfigurationOptions.InstanceName);
            return length;
        }

        /// <summary>
        /// 從 Hash 裡找出符合 patternMatch 的 Cachekey.
        /// </summary>
        /// <param name="patternMatch">patternMatch</param>
        /// <returns></returns>
        private IEnumerable<string> HashScan(string patternMatch)
        {
            var length = this.Database.HashLength(this.RedisConfigurationOptions.InstanceName);
            if (length.Equals(0))
            {
                return Enumerable.Empty<string>();
            }

            var collection = new List<HashEntry>();

            var scanResult = this.Database.HashScan(
                key: this._redisConfigurationOptions.InstanceName,
                pattern: patternMatch);

            if (scanResult != null && scanResult.Any().Equals(true))
            {
                collection.AddRange(scanResult);
            }

            var result = collection.Select(x => $"{x.Name}").Distinct();
            return result;
        }

        /// <summary>
        /// 建立 Cachekey.
        /// </summary>
        /// <param name="cachekey"></param>
        /// <returns></returns>
        private string BuildCachekey(string cachekey)
        {
            if (string.IsNullOrWhiteSpace(cachekey))
            {
                throw new ArgumentNullException(nameof(cachekey), $"The value '{nameof(cachekey)}' cannot be null or Empty.");
            }

            var cachekeyPrefix = $"{this.RedisConfigurationOptions.InstanceName}::";

            string result;

            if (cachekey.StartsWith(cachekeyPrefix).Equals(true))
            {
                result = cachekey;
                return result;
            }

            if (cachekey.StartsWith("::"))
            {
                result = $"{this.RedisConfigurationOptions.InstanceName}{cachekey}";
                return result;
            }

            result = $"{cachekeyPrefix}{cachekey}";

            return result;
        }
    }
}