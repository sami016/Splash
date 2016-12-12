using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Tests
{
    class CounterEventData : ICloneable
    {
        public int Count { get; set; } = 0;

        public object Clone()
        {
            return new CounterEventData
            {
                Count = Count
            };
        }
    }
}
