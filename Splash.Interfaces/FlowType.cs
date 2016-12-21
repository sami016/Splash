using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public enum FlowType : byte
    {
        /// <summary>
        /// Events automatically flow from the upstream node to the downstream node.
        /// </summary>
        Repeat,

        /// <summary>
        /// Events only flow when explicitely emitted by a processor on an upstream node.
        /// </summary>
        Emit
    }
}
