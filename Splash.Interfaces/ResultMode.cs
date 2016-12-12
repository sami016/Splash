using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public enum ResultMode
    {
        /// <summary>
        /// Use the result output for the origin node only.
        /// Downstream events will still get fired, but the returned result will only be from processors on the origin node.
        /// </summary>
        OriginOnlyResult,

        /// <summary>
        /// Includes downstream nodes, taking the result from the final stream processed.
        /// </summary>
        IncludeDownstreamLast
    }
}
