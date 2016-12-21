using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Tests
{
    public class AlertEventData : ICloneable
    {
        public string AlertMessage { get; set; }

        public object Clone()
        {
            return new AlertEventData
            {
                AlertMessage = AlertMessage
            };
        }
    }
}
