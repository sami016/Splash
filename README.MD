# Splash

An event flow library

## Simple idea, design decisions

An event system where events are fired onto source nodes, which may then flow into other nodes in a cascading graph.

Each event type has a mutable event data type which is used to store both inputs and results. 
Both the inputs and outputs may be modified, the effects of which will be seen by all downstream and subsequent event processors.
Due to the behaviour of forking streams, event data must implement the ICloneable interface so that it can be copied without seperate forks having side effects.

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
source.FlowInto(downstream);

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

