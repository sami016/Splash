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
        private IEventEngine _engine;

        private IList<ISourceNode> _downstreamRepeat = new List<ISourceNode>();
        private IList<ISourceNode> _downstreamEmit= new List<ISourceNode>();

        public SourceNode(IEventEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<ISourceNode> DownstreamNodes(FlowType flowType)
        {
            if (flowType == FlowType.Repeat)
            {
                return _downstreamRepeat;
            }else
            {
                return _downstreamEmit;
            }
        }

        public void Fire<TEventData>(TEventData eventData, EventMode eventMode = EventMode.Immutable)
            where TEventData : class, ICloneable
        {
            _engine.Process<TEventData>(this, eventData, eventMode);
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

        public void FlowInto(ISourceNode source, FlowType flowType)
        {
            if (flowType == FlowType.Repeat)
            {
                _downstreamRepeat.Add(source);
            }else
            {
                _downstreamEmit.Add(source);
            }
        }
    }
}
