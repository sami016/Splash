using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splash.Interfaces;

namespace Splash.Tests
{
    [TestClass]
    public class EventEngineTest
    {

        private IEventEngine _engine;

        public EventEngineTest()
        {
            _engine = new EventEngine();
        }

        [TestMethod]
        public void Process_SingleSource_ShouldRunProcessors()
        {
            var source = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            var output = source.Emit(new CounterEventData());
            Assert.AreEqual(2, output.Count);
        }
        [TestMethod]
        public void Process_Stop_ShouldPreventSubsequentProcessors()
        {
            var source = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
                evt.Stop();
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            var output = source.Emit(new CounterEventData());
            Assert.AreEqual(1, output.Count);
        }

        [TestMethod]
        public void Process_SingleSource_ShouldRunProcessorsInCorrectOrder()
        {
            var source = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
            });
            var output = source.Emit(new CounterEventData());
            Assert.AreEqual(10, output.Count);
        }


        [TestMethod]
        public void Process_SourceFlow_ShouldCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new Source(_engine);
            var downstream = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.FlowInto(downstream);
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
                downstreamHasRun = true;
            });
            var output = source.Emit(new CounterEventData());
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(downstreamHasRun);
        }


        [TestMethod]
        public void Process_SourceFlowBlockDownstream_ShouldNotCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new Source(_engine);
            var downstream = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
                evt.BlockDownstream();
            });
            source.FlowInto(downstream);
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
                downstreamHasRun = true;
            });
            var output = source.Emit(new CounterEventData(), ResultMode.IncludeDownstreamLast);
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(1, output.Count);
            Assert.IsFalse(downstreamHasRun);
        }


        [TestMethod]
        public void Process_SourceFlowDownstreamLast_ShouldCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new Source(_engine);
            var downstream1 = new Source(_engine);
            var downstream2 = new Source(_engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.FlowInto(downstream1);
            source.FlowInto(downstream2);
            downstream1.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 100;
                downstreamHasRun = true;
            });
            downstream2.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
            });
            var output = source.Emit(new CounterEventData(), ResultMode.IncludeDownstreamLast);
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(10, output.Count);
            Assert.IsTrue(downstreamHasRun);
        }
    }
}
