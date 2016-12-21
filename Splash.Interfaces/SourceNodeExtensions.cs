using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public static class SourceNodeExtensions
    {
        private static MethodInfo fireMethod;

        static SourceNodeExtensions() {
            fireMethod = typeof(ISourceNode).GetMethod("Fire");
        }

        /// <summary>
        /// Fires an event dynamically. Useful when type is only known at runtime.
        /// </summary>
        /// <param name="sourceNode">source node</param>
        /// <param name="eventType">event type</param>
        /// <param name="eventDataResultMode">event data instance</param>
        /// <param name="resultMode">result mode</param>
        /// <returns>event output</returns>
        public static object FireDynamic(this ISourceNode sourceNode, Type eventType, object eventDataResultMode, ResultMode resultMode = ResultMode.OriginOnlyResult)
        {
            object[] parameters = new object[]
            {
                eventDataResultMode,
                resultMode
            };
            return fireMethod.MakeGenericMethod(eventType)
                .Invoke(sourceNode, parameters);
        }


        /// <summary>
        /// Fires an event dynamically. Useful when type is only known at runtime.
        /// </summary>
        /// <param name="sourceNode">source node</param>
        /// <param name="eventDataResultMode">event data instance</param>
        /// <param name="resultMode">result mode</param>
        /// <returns>event output</returns>
        public static object FireDynamic(this ISourceNode sourceNode, object eventDataResultMode, ResultMode resultMode = ResultMode.OriginOnlyResult)
        {
            var eventType = eventDataResultMode.GetType();
            return sourceNode.FireDynamic(eventType, eventDataResultMode, resultMode);
        }
    }
}
