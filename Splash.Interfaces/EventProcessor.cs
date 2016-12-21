using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{

    /// <summary>
    /// Event Processor
    /// </summary>
    /// <typeparam name="TEventData">event data type</typeparam>
    /// <param name="eventData">event data</param>
    /// <param name="eventContext">event</param>
    public delegate void EventProcessor<in TEventData>(TEventData eventData, IEventContext eventContext)
            where TEventData : ICloneable;
}
