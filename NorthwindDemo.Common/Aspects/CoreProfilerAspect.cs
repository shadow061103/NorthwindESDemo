using AspectInjector.Broker;
using CoreProfiler;
using NorthwindDemo.Common.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NorthwindDemo.Common.Aspects
{
    /// <summary>
    /// CoreProfilerAspect
    /// </summary>
    [Aspect(Scope.Global)]
    public class CoreProfilerAspect
    {
        /// <summary>
        /// Invokes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="method">The method.</param>
        /// <param name="type">The type.</param>
        /// <param name="triggers">The triggers.</param>
        /// <returns></returns>
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Invoke(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.Type)] Type type,
            [Argument(Source.Triggers)] System.Attribute[] triggers
        )
        {
            var attribute = triggers.OfType<CoreProfilingAsyncAttribute>().FirstOrDefault();
            string stepName = attribute is null
                ? $"{type.Name}.{name}"
                : string.IsNullOrWhiteSpace(attribute.StepName)
                    ? $"{type.Name}.{name}"
                    : attribute.StepName;
            using (ProfilingSession.Current.Step(stepName))
            {
                return method(arguments);
            }
        }
    }
}
