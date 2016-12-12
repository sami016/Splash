using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface IEventEngine
    {
        TEventData Process<TEventData>(ISourceNode node, TEventData input, ResultMode resultMode = ResultMode.OriginOnlyResult)
            where TEventData : class, ICloneable;
    }
}
