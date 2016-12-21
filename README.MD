# Splash

An event flow library

## Simple idea, design decisions

An event system where events are fired onto source nodes, which may then flow into other nodes in a cascading graph.

Each event type has a mutable event data type which is used to store both inputs and results. 
Both the inputs and outputs may be modified, the effects of which will be seen by all downstream and subsequent event processors.
Due to the behaviour of forking streams, event data must implement the ICloneable interface so that it can be copied without seperate forks having side effects.

## Flow types

At present there are two possible types of flows between nodes: Repeat flows and emission flows.

 - Repeat flows work by passing all events downstream automatically. Firing an event on a source will also mean the event is passed to all downstream nodes that are connected via a repeater flow relationship.
 - Emission flows work by restricting the flow of events. A nodes processors may call `Emit` on the event context which fires events on all downstream nodes connected via an emit flow relationship. Note: This allows you to change event type, or fire many downstream events for one upstream event.

 ```
 // FlowType.Repeat
 // FlowType.Emit

 sourceNode1.FlowInto(sourceNode2, FlowType.Emit);
 ```

## Examples

### Creating a single source

```
var factory = new SourceFactory();

var source = factory.Create();
source.Register<CounterEventData>((data, evt) =>
{
	data.Count++;
});
var result = source.Fire(new CounterEventData());
```

### Creating a tree of sources

```
var factory = new SourceFactory();

var source = factory.Create();
var downstream = factory.Create();
source.FlowInto(downstream, FlowType.Repeat);

source.Register<CounterEventData>((data, evt) =>
{
    data.Count++;
});
downstream.Register<CounterEventData>((data, evt) =>
{
	data.Count++;
    data.Count *= 10;
});
// Result will have count 20 - we've flagged the engine to take the result from the last downstream source.
var output = source.Fire(new CounterEventData(), ResultMode.IncludeDownstreamLast);
// We can also take the result from the source node only by using ResultMode.OriginOnlyResult
```

### Basic event listeners

