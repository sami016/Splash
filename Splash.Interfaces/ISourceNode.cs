using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface ISourceNode : ISource
    {
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
        /// <returns>set of downstream nodes</returns>
        IEnumerable<ISourceNode> DownstreamNodes();
    }
}
