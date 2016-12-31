using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splash.Interfaces;
using Xunit;
using FluentAssertions;

namespace Splash.Tests
{
    
    public class EmissionFlowTest : EventEngineTest
    {
        [Fact]
        public void EmitFlow_SingleDownstream_EmissionShouldWork()
        {
            bool downstreamCalled = false;

            var source = new SourceNode(Engine);
            var source2 = new SourceNode(Engine);
            source.FlowInto(source2, FlowType.Emit);
            // Throw 
            source.Register<CounterEventData>((data, evt) =>
            {
                evt.Emit(new AlertEventData
                {
                    AlertMessage = "Alert!"
                });
            });
            // Check: downstream node should receive the alert event.
            source2.Register<AlertEventData>((data, evt) =>
            {
                downstreamCalled = true;
            });
            // Check: downstream node shouldn't recieve the counter event.
            source2.Register<CounterEventData>((data, evt) =>
            {
                throw new Exception("This shouldn't be called.");
            });

            source.Fire(new CounterEventData());
            downstreamCalled.Should().BeTrue();
        }
    }
}
