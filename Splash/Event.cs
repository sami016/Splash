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

        private ISource _origin;
        private ISource _source;

        public Event(ISource origin, ISource source)
        {
            _origin = origin;
            _source = source;
        }

        public ISource Origin => _origin;

        public ISource Source => _source;

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
