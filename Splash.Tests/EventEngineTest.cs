using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splash.Interfaces;

namespace Splash.Tests
{
    public abstract class EventEngineTest
    {

        protected IEventEngine Engine { get; }

        public EventEngineTest()
        {
            Engine = new EventEngine();
        }

    }
}
