using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splash.Interfaces;

namespace Splash.Tests
{
    [TestClass]
    public class RepeatFlowTest : EventEngineTest
    {

        [TestMethod]
        public void RepeatFlow_SingleSource_ShouldRunProcessors()
        {
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            var output = source.Fire(new CounterEventData());
            Assert.AreEqual(2, output.Count);
        }


        [TestMethod]
        public void RepeatFlow_ExtensionFire_ShouldRunProcessors()
        {
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            object eventData = new CounterEventData();
            var output = source.FireDynamic(eventData);
            Assert.AreEqual(2, (output as CounterEventData).Count);
        }

        [TestMethod]
        public void RepeatFlow_Stop_ShouldPreventSubsequentProcessors()
        {
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
                evt.Stop();
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            var output = source.Fire(new CounterEventData());
            Assert.AreEqual(1, output.Count);
        }

        [TestMethod]
        public void RepeatFlow_SingleSource_ShouldRunProcessorsInCorrectOrder()
        {
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
            });
            var output = source.Fire(new CounterEventData());
            Assert.AreEqual(10, output.Count);
        }


        [TestMethod]
        public void RepeatFlow_SourceFlow_ShouldCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new SourceNode(Engine);
            var downstream = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.FlowInto(downstream, FlowType.Repeat);
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
                downstreamHasRun = true;
            });
            var output = source.Fire(new CounterEventData());
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(downstreamHasRun);
        }


        [TestMethod]
        public void RepeatFlow_SourceFlowBlockDownstream_ShouldNotCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new SourceNode(Engine);
            var downstream = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
                evt.BlockDownstream();
            });
            source.FlowInto(downstream, FlowType.Repeat);
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
                downstreamHasRun = true;
            });
            var output = source.Fire(new CounterEventData(), ResultMode.IncludeDownstreamLast);
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(1, output.Count);
            Assert.IsFalse(downstreamHasRun);
        }


        [TestMethod]
        public void RepeatFlow_SourceFlowDownstreamLast_ShouldCallDownstream()
        {
            var downstreamHasRun = false;

            var source = new SourceNode(Engine);
            var downstream1 = new SourceNode(Engine);
            var downstream2 = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            source.FlowInto(downstream1, FlowType.Repeat);
            source.FlowInto(downstream2, FlowType.Repeat);
            downstream1.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 100;
                downstreamHasRun = true;
            });
            downstream2.Register<CounterEventData>((data, evt) =>
            {
                data.Count *= 10;
            });
            var output = source.Fire(new CounterEventData(), ResultMode.IncludeDownstreamLast);
            // Now, the output should only be 1, since returned result is only for the current node.
            Assert.AreEqual(10, output.Count);
            Assert.IsTrue(downstreamHasRun);
        }
    }
}
