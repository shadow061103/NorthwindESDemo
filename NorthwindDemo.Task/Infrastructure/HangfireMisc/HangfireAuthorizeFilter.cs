using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Task.Infrastructure.HangfireMisc
{
    public class HangfireAuthorizeFilter : IDashboardAuthorizationFilter
    {
        private IHttpContextAccessor HttpContextAccessor { get; set; }

        private string[] DashboardUsers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireAuthorizeFilter" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HttpContextAccessor.</param>
        /// <param name="users">The users.</param>
        public HangfireAuthorizeFilter(IHttpContextAccessor httpContextAccessor, string[] users)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.DashboardUsers = users;
        }

        //-----------------------------------------------------------------------------------------

        public bool Authorize([NotNull] DashboardContext context)
        {
            var userName = this.HttpContextAccessor.HttpContext.User.Identity.Name;
            var isAuthenticated = this.IsAuthenticated(userName);
            return isAuthenticated;
        }

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAuthenticated(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName).Equals(false)
                &&
                userName.StartsWith("EVERTRUST"))
            {
                userName = userName.Replace(@"EVERTRUST\", "");
            }

            if (this.DashboardUsers is null || this.DashboardUsers.Any().Equals(false))
            {
                return false;
            }

            if (this.DashboardUsers.Contains("*"))
            {
                return true;
            }

            var result = this.DashboardUsers.Contains(userName);
            return result;
        }
    }
}
