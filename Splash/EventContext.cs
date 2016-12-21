using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash
{
    public class EventContext : IEventContext
    {

        private ISourceNode _origin;
        private ISourceNode _source;
        private IEventEngine _engine;
        
        public EventContext(ISourceNode origin, ISourceNode source, IEventEngine engine)
        {
            _origin = origin;
            _source = source;
            _engine = engine;
        }

        public ISourceNode Origin => _origin;

        public ISourceNode Source => _source;

        public bool IsStopped { get; private set; }
        public bool IsBlocked { get; private set; }

        public void BlockDownstream()
        {
            IsBlocked = true;
        }

        public void Stop()
        {
            IsStopped = true;
        }

        public void Emit<TEventData>(TEventData eventData)
            where TEventData : class, ICloneable
        {
            foreach (var node in _source.DownstreamNodes(FlowType.Emit))
            {
                _engine.Process<TEventData>(node, eventData, ResultMode.OriginOnlyResult);
            }
        }
    }
}
