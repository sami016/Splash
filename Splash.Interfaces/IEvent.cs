using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface IEvent
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
        ISource Origin { get; }
        
        /// <summary>
        /// Get the source the event is currently flowing through.
        /// </summary>
        ISource Source { get; }
    }
}
