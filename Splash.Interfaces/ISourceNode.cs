using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface ISourceNode
    {
        /// <summary>
        /// Emits, getting handled by registered handlers, as well as downstream sources.
        /// 
        /// Notes: returned result will not capture changes made by downstream nodes.
        /// </summary>
        /// <typeparam name="TEventData">event data type</typeparam>
        /// <param name="eventData">event data</param>
        /// <returns>output processed event data after processors have been applied to this source.</returns>
        TEventData Fire<TEventData>(TEventData eventData, ResultMode resultMode = ResultMode.OriginOnlyResult)
            where TEventData : class, ICloneable;

        /// <summary>
        /// Registers an event processor on the stream.
        /// The processor will be run after existing event processors.
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventProcessor"></param>
        void Register<TEventData>(EventProcessor<TEventData> eventProcessor)
            where TEventData : ICloneable;

        /// <summary>
        /// Links the source to a downstream source.
        /// </summary>
        /// <param name="source">downstream source</param>
        /// <param name="flowType">flow type</param>
        void FlowInto(ISourceNode source, FlowType flowType);

        /// <summary>
        /// Gets the set of registered event processors for a given event data type.
        /// </summary>
        /// <typeparam name="TEventData">event data type</typeparam>
        /// <returns>set of processors for this event type</returns>
        IEnumerable<EventProcessor<TEventData>> RegisteredProcessors<TEventData>()
            where TEventData : class, ICloneable;

        /// <summary>
        /// Get the set of downstream nodes.
        /// </summary>
        /// <param name="flowType">flow type</param>
        /// <returns>set of downstream nodes</returns>
        IEnumerable<ISourceNode> DownstreamNodes(FlowType flowType);
    }
}
