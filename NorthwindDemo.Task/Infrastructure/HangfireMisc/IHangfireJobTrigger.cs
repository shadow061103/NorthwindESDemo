using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Task.Infrastructure.HangfireMisc
{
    public interface IHangfireJobTrigger
    {
        void OnStart();
    }
}
