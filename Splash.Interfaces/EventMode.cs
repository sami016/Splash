using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    /// <summary>
    /// Determines how events are handled - whether they are mutable, and if so how downstream nodes are handled.
    /// </summary>
    public enum EventMode
    {
        /// <summary>
        /// Event data is immutable.
        /// Each processor receives a exact clone of the original event data.
        /// 
        ///     S
        ///     
        ///     
        ///   A    B
        /// 
        /// All run independently.
        /// </summary>
        Immutable,

        /// <summary>
        /// Event data is mutable - subsequent processors will see modifications made by previous processors.
        /// Parallel streaming - downstream nodes all receive modified event data, but are independent from one another.
        /// 
        ///     S
        ///    / \
        ///   /   \
        ///  A     B
        ///  
        /// S -> A
        /// S -> B
        /// </summary>
        Mutable_Parallel,

        /// <summary>
        /// Event data is mutable - subsequent processors will see modifications made by previous processors.
        /// Sequential processing - downstream nodes feed into one another, meaning all downstream processors work with the same result.
        /// 
        /// e.g. event data received by root node, is then passed down downstream node A, then passed to downstraem node B.
        /// 
        ///     S
        ///    /
        ///   /
        ///  A------>B
        ///  
        /// S -> A -> B
        /// </summary>
        Mutable_Sequential
    }
}
