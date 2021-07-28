using AspectInjector.Broker;
using NorthwindDemo.Common.Aspects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDemo.Common.Attribute
{

    /// <summary>
    ///方法性能監控標籤(非同步處理 & 公開方法)
    /// </summary>
    [Injection(typeof(CoreProfilerAspect))]
    public class CoreProfilingAsyncAttribute : System.Attribute
    {
        public string StepName { get; set; }

        /// <summary>
        /// 以執行位置為監控描述
        /// </summary>
        public CoreProfilingAsyncAttribute()
        {
        }

        /// <summary>
        /// 以傳入參數為監控描述
        /// </summary>
        /// <param name="stepName"></param>
        public CoreProfilingAsyncAttribute(string stepName)
        {
            StepName = stepName;
        }
    }
}
