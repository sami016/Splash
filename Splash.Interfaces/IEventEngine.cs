using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface IEventEngine
    {
        void Process<TEventData>(ISourceNode node, TEventData input, EventMode eventMode)
            where TEventData : class, ICloneable;
    }
}
