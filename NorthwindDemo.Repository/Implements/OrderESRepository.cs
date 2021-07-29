using Nest;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDemo.Repository.Implements
{
    public class OrderESRepository : IOrderESRepository
    {
        private readonly IElasticClient _elasticClient;

        private readonly string _index = "northwind_orders";

        public OrderESRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// 批次新增
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        public async Task<bool> BulkInsert(IEnumerable<OrdersESModel> orders)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var model in orders)
            {
                bulkDescriptor.Index<OrdersESModel>
                (
                    d => d.Index(_index)
                        .Id(model.OrderId)
                        .Document(model)
                );
            }

            var response = await this._elasticClient.BulkAsync(bulkDescriptor);

            if (response.IsValid.Equals(false))
            {
                throw response.OriginalException;
            }

            return response.IsValid;
        }

        /// <summary>
        /// 批次更新
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        public async Task<bool> BulkUpdate(IEnumerable<OrdersESModel> orders)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var model in orders)
            {
                bulkDescriptor.Update<OrdersESModel>
                (
                    d => d.Index(_index)
                        .Id(model.OrderId)
                        .Doc(model)
                );
            }

            var response = await this._elasticClient.BulkAsync(bulkDescriptor);

            if (response.IsValid.Equals(false))
            {
                throw response.OriginalException;
            }

            return response.IsValid;
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="orderIds">The order ids.</param>
        /// <returns></returns>
        public async Task<bool> BulkDelete(IEnumerable<int> orderIds)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var id in orderIds)
            {
                bulkDescriptor.Delete<OrdersESModel>
                (
                    d => d.Index(_index)
                        .Id(id)
                );
            }

            var response = await this._elasticClient.BulkAsync(bulkDescriptor);

            if (response.IsValid.Equals(false))
            {
                throw response.OriginalException;
            }

            return response.IsValid;
        }
    }
}