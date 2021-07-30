using Microsoft.Extensions.Options;
using NorthwindDemo.Common;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Common.Caching;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.Cache;
using NorthwindDemo.Repository.Models.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDemo.Repository.Decorators.Redis
{
    public class RedisOrderRepository : RedisCacheRepositoryBase, IOrderESRepository
    {
        private readonly IOrderESRepository _orderESRepository;

        public RedisOrderRepository(IOptions<CacheDecoratorSettingsOptions> options,
                                  ICacheProviderResolver cacheProviderResolver,
                                  IRedisCacheHelper redisCacheRemoveHelper,
                                  IOrderESRepository orderESRepository)
            : base(options, cacheProviderResolver, redisCacheRemoveHelper)
        {
            this.SetCacheProvider(CacheTypeEnum.Redis, nameof(IOrderESRepository), nameof(RedisOrderRepository));
            _orderESRepository = orderESRepository;
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="orderIds">The order ids.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CoreProfilingAsync("CacheOrderRepository.BulkDelete")]
        public async Task<bool> BulkDelete(IEnumerable<int> orderIds)
        {
            var result = await this._orderESRepository.BulkDelete(orderIds);

            return result;
        }

        /// <summary>
        /// 批次新增
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CoreProfilingAsync("CacheOrderRepository.BulkInsert")]
        public async Task<bool> BulkInsert(IEnumerable<OrdersESModel> orders)
        {
            var result = await this._orderESRepository.BulkInsert(orders);

            return result;
        }

        /// <summary>
        /// 批次更新
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CoreProfilingAsync("CacheOrderRepository.BulkUpdate")]
        public async Task<bool> BulkUpdate(IEnumerable<OrdersESModel> orders)
        {
            var result = await this._orderESRepository.BulkUpdate(orders);

            return result;
        }

        /// <summary>
        /// 依編號取得訂單
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CoreProfilingAsync("CacheOrderRepository.GetAsync")]
        public async Task<OrdersESModel> GetAsync(int orderId)
        {
            var cacheItem = await this.GetOrAddCacheItemAsync
                (
                     string.Format(CacheKey.NorthwindGet, orderId),
                    CacheUtility.GetCacheItemExpirationOneHour(),
                    async () =>
                    {
                        var result = await this._orderESRepository.GetAsync(orderId);
                        return result;
                    }
                );

            return cacheItem;
        }

        /// <summary>
        /// 依條件查詢訂單
        /// </summary>
        /// <param name="searchESModel">The search es model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CoreProfilingAsync("CacheOrderRepository.GetAsync")]
        public async Task<IEnumerable<OrdersESModel>> GetAsync(SearchESModel searchESModel)
        {
            var result = await this._orderESRepository.GetAsync(searchESModel);

            return result;
        }
    }
}