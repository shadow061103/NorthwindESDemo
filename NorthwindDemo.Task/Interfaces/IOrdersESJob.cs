using Hangfire.Server;
using System.ComponentModel;

namespace NorthwindDemo.Task.Interfaces
{
    public interface IOrdersESJob
    {
        /// <summary>
        /// 新增訂單ES工作
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        [DisplayName("結轉訂單ES資料")]
        System.Threading.Tasks.Task InsertOrderESAsync(PerformContext context);
    }
}