using Splash;
using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash
{
    public class EventEngine : IEventEngine
    {
        public void Process<TEventData>(ISourceNode origin, TEventData eventData, EventMode resultMode)
            where TEventData : class, ICloneable
        {
            ProcessRecursive(origin, origin, eventData, resultMode);
        }

        private void ProcessRecursive<TEventData>(ISourceNode origin, ISourceNode current, TEventData eventData, EventMode eventMode)
            where TEventData : class, ICloneable
        {
            EventContext context = new EventContext(origin, origin, this);

            foreach (var processor in current.RegisteredProcessors<TEventData>())
            {
                var data = eventMode == EventMode.Immutable
                    ? eventData.Clone() as TEventData
                    : eventData;
                processor(data, context);
                // If a stop if requested, execution terminates immediately.
                if (context.IsStopped)
                {
                    return;
                }
            }
            // After running all processors, the downstream may be blocked.
            if (context.IsBlocked)
            {
                return;
            }
            // Continue onto downstream nodes.
            FireDownstream(origin, current, eventData, eventMode, eventMode != EventMode.Mutable_Sequential);
        }

        private void FireDownstream<TEventData>(ISourceNode origin, ISourceNode current, TEventData eventData, EventMode eventMode, bool parallel)
            where TEventData : class, ICloneable
        {
            foreach (var node in current.DownstreamNodes(FlowType.Repeat))
            {
                var eventDataToUse = parallel
                    ? eventData.Clone() as TEventData
                    : eventData;
                ProcessRecursive<TEventData>(origin, node, eventDataToUse, eventMode);
            }
        }
    }
}
