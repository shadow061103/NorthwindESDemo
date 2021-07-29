using NorthwindDemo.Task.Interfaces;
using System;

namespace NorthwindDemo.Task.Infrastructure.HangfireMisc
{
    public class HangfireJobTrigger : IHangfireJobTrigger
    {
        public void OnStart()
        {
            Hangfire.RecurringJob.AddOrUpdate<IOrdersESJob>
                (
                x => x.InsertOrderESAsync(null),
                "0 * * * *",
                TimeZoneInfo.Local
                );
        }
    }
}