using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NorthwindDemo.Common.Caching
{
    /// <summary>
    /// Class RedisCacheProvider. Implements the <see cref="CacheProvider"/>
    /// </summary>
    /// <seealso cref="CacheProvider"/>
    public class RedisCacheProvider : CacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        private readonly MessagePackSerializerOptions options;

        /// <summary>
        /// Constructor for RedisCacheProvider.
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="duration"></param>
        public RedisCacheProvider(IDistributedCache distributedCache, TimeSpan duration)
            : base(duration)
        {
            this._distributedCache = distributedCache;
            options = ContractlessStandardResolver.Options;
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Insert an item in the cache with the expiration that it will expire if not used past its window
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="slidingExpiration">The expiration window</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">key or value</exception>
        public override bool Save(string key, object value, TimeSpan slidingExpiration)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.Equals(null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            bool result;

            try
            {
                this._distributedCache.Set
                (
                    key: key,
                    value: MessagePackSerializer.Serialize(value, options),
                    options: new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = slidingExpiration
                    }
                );

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Insert an item in the cache with the expiration that will expire at a specific point in time
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="absoluteExpiration">The DateTime in which this object will expire</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">key or value</exception>
        public override bool Save(string key, object value, DateTime absoluteExpiration)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.Equals(null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            bool result;

            try
            {
                this._distributedCache.Set
                (
                    key: key,
                    value: MessagePackSerializer.Serialize(value, options),
                    options: new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = new TimeSpan(absoluteExpiration.Ticks)
                    }
                );

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time (Minutes).</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Save(string key, object value, int cacheTime)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.Equals(null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            bool result;

            try
            {
                this._distributedCache.Set
                (
                    key: key,
                    value: MessagePackSerializer.Serialize(value, options),
                    options: this.GetCacheEntryOptions(TimeSpan.FromMinutes(cacheTime))
                );

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Insert an item in the cache with the cacheTime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Save<T>(string key, T value, TimeSpan cacheTime)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.Equals(null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            bool result;

            try
            {
                this._distributedCache.Set
                (
                    key: key,
                    value: MessagePackSerializer.Serialize(value, options),
                    options: this.GetCacheEntryOptions(cacheTime)
                );

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Insert an Collection in the cache with the CacheItemPolicy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SaveCollection<T>(string keyPrefix, List<T> collection, TimeSpan cacheTime)
        {
            if (string.IsNullOrWhiteSpace(keyPrefix))
            {
                throw new ArgumentNullException(nameof(keyPrefix));
            }

            if (collection.Equals(null) || collection.Any().Equals(false))
            {
                throw new ArgumentNullException(nameof(collection));
            }

            bool result;

            try
            {
                this._distributedCache.Set
                (
                    key: keyPrefix,
                    value: MessagePackSerializer.Serialize(collection, options),
                    options: this.GetCacheEntryOptions(cacheTime)
                );

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public override IEnumerable<T> GetCollection<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = this._distributedCache.Get(key);

            var cachedValue = value != null
                ? MessagePackSerializer.Deserialize<IEnumerable<T>>(value, options)
                : Enumerable.Empty<T>();

            return cachedValue;
        }

        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public override bool TryGetValue(string key, out object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var exists = this.Exists(key);
            if (exists.Equals(false))
            {
                value = null;
                return false;
            }

            value = this.Get(key);

            if (value != null)
            {
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Retrieves a cached object from the cache
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>The object from the database, or an exception if the object doesn't exist</returns>
        public override object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = this._distributedCache.Get(key);

            var cachedValue = value != null
                ? MessagePackSerializer.Deserialize<object>(value, options)
                : null;

            return cachedValue;
        }

        /// <summary>
        /// Retrieves a cached object from the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key used to identify this object.</param>
        /// <returns>T.</returns>
        public override T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = this._distributedCache.Get(key);

            var cachedValue = value != null
                ? MessagePackSerializer.Deserialize<T>(value, options)
                : default;

            return cachedValue;
        }

        /// <summary>
        /// Gets a set of cached objects
        /// </summary>
        /// <param name="keys">All of the keys of the objects we wish to retrieve</param>
        /// <returns>
        /// A key / value dictionary containing all of the keys and objects we wanted to retrieve
        /// </returns>
        public override IDictionary<string, object> Get(string[] keys)
        {
            if (keys.Equals(null) || keys.Any().Equals(false))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cachedValue = keys.ToDictionary(key => key, this.Get);
            return cachedValue;
        }

        /// <summary>
        /// Removes an object from the cache with the specified key
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>
        /// True if the object was removed, false if it didn't exist or was unable to be removed
        /// </returns>
        public override bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (this.Exists(key).Equals(false))
            {
                return false;
            }

            this._distributedCache.Remove(key);

            var result = this.Exists(key).Equals(false);
            return result;
        }

        /// <summary>
        /// Flush this instance.
        /// </summary>
        public override void Flush()
        {
            // 請使用 RedisCacheRemoveHelper 的 FlushDatabase 功能
        }

        /// <summary>
        /// Validates if an object with this key exists in the cache
        /// </summary>
        /// <param name="key">The key of the object to check</param>
        /// <returns>True if it exists, false if it doesn't</returns>
        public override bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var cachedValue = this._distributedCache.Get(key);
            var result = cachedValue != null;
            return result;
        }

        /// <summary>
        /// Create entry options to item of memory cache
        /// </summary>
        /// <param name="cacheTime">Cache time</param>
        protected DistributedCacheEntryOptions GetCacheEntryOptions(TimeSpan cacheTime)
        {
            var options = new DistributedCacheEntryOptions
            {
                // set cache time
                AbsoluteExpirationRelativeToNow = cacheTime
            };

            return options;
        }
    }
}