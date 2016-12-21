using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash
{
    public class SourceNode : ISourceNode
    {
        private IDictionary<Type, IList<object>> processors = new Dictionary<Type, IList<object>>();
        private IList<ISourceNode> _downstream = new List<ISourceNode>();
        private IEventEngine _engine;

        public SourceNode(IEventEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<ISourceNode> DownstreamNodes()
        {
            return _downstream;
        }

        public TEventData Fire<TEventData>(TEventData eventData, ResultMode resultMode = ResultMode.OriginOnlyResult)
            where TEventData : class, ICloneable
        {
            return _engine.Process<TEventData>(this, eventData, resultMode);
        }

        public void FlowInto(ISourceNode source)
        {
            _downstream.Add(source);
        }

        public void Register<TEventData>(EventProcessor<TEventData> eventProcessor) 
            where TEventData : ICloneable
        {
            var type = typeof(TEventData);
            if (!processors.ContainsKey(type))
            {
                processors[type] = new List<object>();
            }
            processors[type].Add(eventProcessor);
        }

        public IEnumerable<EventProcessor<TEventData>> RegisteredProcessors<TEventData>()
            where TEventData : class, ICloneable
        {
            var type = typeof(TEventData);
            if (!processors.ContainsKey(type))
            {
                return new EventProcessor<TEventData>[] { };
            }
            return processors[type].Cast<EventProcessor<TEventData>>();
        }
        
    }
}
