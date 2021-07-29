using Hangfire.Console;
using Hangfire.Server;
using NorthwindDemo.Service.Interfaces;
using NorthwindDemo.Task.Interfaces;
using System;
using System.ComponentModel;

namespace NorthwindDemo.Task.Jobs
{
    public class OrdersESJob : IOrdersESJob
    {
        private readonly IOrderESService _orderESService;

        private readonly IOrderServices _orderServices;

        public OrdersESJob(IOrderESService orderESService, IOrderServices orderServices)
        {
            _orderESService = orderESService;
            _orderServices = orderServices;
        }

        /// <summary>
        /// 新增訂單ES工作
        /// </summary>
        /// <param name="context">The context.</param>
        [DisplayName("結轉訂單ES資料")]
        public async System.Threading.Tasks.Task InsertOrderESAsync(PerformContext context)
        {
            context.WriteLine($"{Environment.MachineName}-{DateTime.Now} Start run InsertOrderES job");

            var ordersDto = await this._orderServices.Get();

            var result = await this._orderESService.Add(ordersDto);

            context.WriteLine($"{Environment.MachineName}-{DateTime.Now} End run InsertOrderES job");
        }
    }
}