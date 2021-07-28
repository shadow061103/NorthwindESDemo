using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Task.Infrastructure.HangfireMisc
{
    public class HangfireSettings
    {
        /// <summary>
        /// EnableServer.
        /// </summary>
        public bool EnableServer { get; set; }

        /// <summary>
        /// EnableDashboard.
        /// </summary>
        public bool EnableDashboard { get; set; }

        /// <summary>
        /// ServerName.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Hangfire's WorkerCount.
        /// </summary>
        public int WorkerCount { get; set; }

        /// <summary>
        /// Hangfire's queues.
        /// </summary>
        public string[] Queues { get; set; }

        /// <summary>
        /// Hangfire Dashboard Users.
        /// </summary>
        public string[] DashboardUsers { get; set; }

        /// <summary>
        /// Hangfire Table Schema Name (default: Hangfire).
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// PrepareSchemaIfNecessary (default: false).
        /// </summary>
        public bool PrepareSchemaIfNecessary { get; set; }

        /// <summary>
        /// 啟用排程任務
        /// </summary>
        public bool EnableRecurringJob { get; set; }

        public HangfireSettings()
        {
            this.EnableServer = false;
            this.EnableDashboard = false;
            this.ServerName = string.Empty;
            this.WorkerCount = 10;
            this.Queues = new List<string>().ToArray();
            this.DashboardUsers = new List<string>().ToArray();
            this.SchemaName = "NorthwindDemo_Task";
            this.PrepareSchemaIfNecessary = false;
            this.EnableRecurringJob = false;
        }
    }
}
