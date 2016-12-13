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
        public TEventData Process<TEventData>(ISourceNode origin, TEventData eventData, ResultMode resultMode)
            where TEventData : class, ICloneable
        {
            return ProcessRecursive(origin, origin, eventData, resultMode);
        }

        private TEventData ProcessRecursive<TEventData>(ISourceNode origin, ISourceNode current, TEventData eventData, ResultMode resultMode)
            where TEventData : class, ICloneable
        {
            Event<TEventData> evnt = new Event<TEventData>(origin, origin);
            TEventData data = eventData.Clone() as TEventData;

            foreach (var processor in current.RegisteredProcessors<TEventData>())
            {
                processor(data, evnt);
                // If a stop if requested, execution terminates immediately.
                if (evnt.IsStopped)
                {
                    return data;
                }
            }
            // After running all processors, the downstream may be blocked.
            if (evnt.IsBlocked)
            {
                return data;
            }
            // Continue on recursively downstream.
            TEventData lastResult = data;
            foreach (var node in current.DownstreamNodes())
            {
                lastResult = Process<TEventData>(node, data, resultMode);
            }
            // Handle different result modes.
            switch(resultMode)
            {
                case ResultMode.OriginOnlyResult:
                    return data;
                case ResultMode.IncludeDownstreamLast:
                    return lastResult;
            }
            return lastResult;
        }
    }
}
