using Nest;
using NorthwindDemo.Common.Enum;
using NorthwindDemo.Repository.Infrastructure.Helpers;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.ES;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 依編號取得訂單
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<OrdersESModel> GetAsync(int orderId)
        {
            var searchRequest = new SearchRequest<OrdersESModel>(index: _index)
            {
                Query = new TermQuery
                {
                    Field = Infer.Field<OrdersESModel>(c => c.OrderId),
                    Value = orderId
                }
            };

            var searchResponse = await _elasticClient.SearchAsync<OrdersESModel>(s => searchRequest);

            return searchResponse.Documents.FirstOrDefault();
        }

        /// <summary>
        ///依條件查詢訂單
        /// </summary>
        /// <param name="searchESModel">The search es model.</param>
        /// <returns></returns>
        public async Task<IEnumerable<OrdersESModel>> GetAsync(SearchESModel searchESModel)
        {
            var orders = new List<OrdersESModel>();

            var queryContainer = new List<QueryContainer>();
            queryContainer.Append(EsCommandHelper.GetShipCountyContainer(searchESModel.ShipCity));
            queryContainer.Append(EsCommandHelper.GetFreightMinContainer(searchESModel.FreightMin));
            queryContainer.Append(EsCommandHelper.GetFreightMaxContainer(searchESModel.FreightMax));
            queryContainer.Append(EsCommandHelper.GetShipNameContainer(searchESModel.ShipName));
            queryContainer.Append(EsCommandHelper.GetOrderDateContainer(searchESModel.StartOrderDate, searchESModel.EndtOrderDate));

            var query = new BoolQuery { Filter = queryContainer };

            var searchRequest = new SearchRequest<OrdersESModel>(index: _index)
            {
                Source = new SourceFilter
                {
                    Includes = "*"
                },
                Query = query,
                Sort = EsSortHelper.GetOrder(OrderField.Default),
                From = 0,
                Size = 5000
            };

            var searchResponse = await _elasticClient.SearchAsync<OrdersESModel>(s => searchRequest);

            orders.AddRange(searchResponse.Documents);

            return orders;
        }
    }
}