using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    /// <summary>
    /// Event context.
    /// </summary>
    public interface IEventContext
    {
        /// <summary>
        /// Stops the event from being processed further.
        /// </summary>
        void Stop();

        /// <summary>
        /// Prevents the event from flowing into subsequent source stages.
        /// </summary>
        void BlockDownstream();
        
        /// <summary>
        /// Get the source the event was originally emitted at.
        /// </summary>
        ISourceNode Origin { get; }
        
        /// <summary>
        /// Get the source the event is currently flowing through.
        /// </summary>
        ISourceNode Source { get; }

        /// <summary>
        /// Emits an event to be handled by downstream emit nodes.
        /// </summary>
        void Emit<TEventData>(TEventData eventData, EventMode eventMode = EventMode.Immutable)
            where TEventData : class, ICloneable;

    }
}
