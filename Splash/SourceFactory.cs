using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash
{
    public class SourceFactory
    {
        private IEventEngine _engine;

        public SourceFactory()
        {
            _engine = new EventEngine();
        }

        public ISourceNode Create()
        {
            return new SourceNode(_engine);
        }
    }
}
