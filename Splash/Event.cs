using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash
{
    public class Event<TEventData> : IEvent
    {

        private ISourceNode _origin;
        private ISourceNode _source;

        public Event(ISourceNode origin, ISourceNode source)
        {
            _origin = origin;
            _source = source;
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
    }
}
