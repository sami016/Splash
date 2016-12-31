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
    public class RepeatFlowTest : EventEngineTest
    {
        /// <summary>
        /// Tests that state persists between different processors on a node when the event mode is mutable.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Immutable)]
        public void Flow_Immutable_StateShouldPersistBetweenProcessors(EventMode eventMode)
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
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count.Should().Be(0);
            });

            source.Fire(new CounterEventData(), eventMode);
        }


        /// <summary>
        /// Tests that state persists between different processors on a node when the event mode is mutable.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Mutable_Parallel)]
        [InlineData(EventMode.Mutable_Sequential)]
        public void Flow_Mutable_StateShouldPersistBetweenProcessors(EventMode eventMode)
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
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count.Should().Be(2);
            });

            source.Fire(new CounterEventData(), eventMode);
        }


        /// <summary>
        /// Tests that state persists between different processors on a node when the event mode is mutable.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Mutable_Parallel)]
        [InlineData(EventMode.Mutable_Sequential)]
        public void Flow_Mutable_StateShouldPersistBetweenNodes(EventMode eventMode)
        {
            var source = new SourceNode(Engine);
            var downstream = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count++;
            });
            downstream.Register<CounterEventData>((data, evt) =>
            {
                data.Count.Should().Be(2);
            });

            source.Fire(new CounterEventData(), eventMode);
        }


        [Fact]
        public void Flow_ExtensionFire_ShouldRunProcessors()
        {
            var hasRun = false;
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                hasRun = true;
            });
            object eventData = new CounterEventData();
            source.FireDynamic(eventData);
            hasRun.Should().BeTrue();
        }

        /// <summary>
        /// Checks that calling stop on the context prevents subsequent processors.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Immutable)]
        [InlineData(EventMode.Mutable_Sequential)]
        [InlineData(EventMode.Mutable_Parallel)]
        public void Flow_Stop_ShouldPreventSubsequentProcessors(EventMode eventMode)
        {
            var finalHasRun = false;
            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                evt.Stop();
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                // This should not execute.
                finalHasRun = true;
            });
            source.Fire(new CounterEventData(), eventMode);
            finalHasRun.Should().BeFalse();
        }

        /// <summary>
        /// Checks execution order by using an external counter.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Immutable)]
        [InlineData(EventMode.Mutable_Sequential)]
        [InlineData(EventMode.Mutable_Parallel)]
        public void RepeatFlow_SingleSource_ShouldRunProcessorsInCorrectOrder(EventMode eventMode)
        {
            var externalCounter = 0;

            var source = new SourceNode(Engine);
            source.Register<CounterEventData>((data, evt) =>
            {
                externalCounter++;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                externalCounter *= 10;
            });
            source.Register<CounterEventData>((data, evt) =>
            {
                externalCounter += 2;
            });
            source.Fire(new CounterEventData(), eventMode);
            externalCounter.Should().Be(12);
        }

        /// <summary>
        /// In all cases, downstream node processors should be run.
        /// </summary>
        [Theory]
        [InlineData(EventMode.Immutable)]
        [InlineData(EventMode.Mutable_Parallel)]
        [InlineData(EventMode.Mutable_Sequential)]
        public void Flow_All_DownstreamShouldRun(EventMode eventMode)
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
                downstreamHasRun = true;
            });

            source.Fire(new CounterEventData(), eventMode);
            downstreamHasRun.Should().BeTrue();
        }


        [Fact]
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
            source.Fire(new CounterEventData(), EventMode.Mutable_Parallel);
            // Now, the output should only be 1, since returned result is only for the current node.
            //Assert.AreEqual(1, output.Count);
            //Assert.IsFalse(downstreamHasRun);
        }

    }
}
