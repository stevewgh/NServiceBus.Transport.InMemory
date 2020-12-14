# NServiceBus.Transport.InMemory

An InMemory Transport for NServiceBus 7.x designed to let you test an endpoint from a blank slate each time.

This has a number of benefits for testing endpoints

- requires no special permissions
- requires nothing to be installed on the host
- potentially faster execution
- nothing to clean up after the test, making the test repeatable without side effects

## Getting started

``` c#
Install-Package NServiceBus.Transport.InMemory
```

To register the transport create your EndpointConfiguration object and tell it to use the InMemory transport

``` c#
var config = new EndpointConfiguration("Orders");
var transport = config.UseTransport<InMemoryTransport>();
```

You can still configure routing for commands

``` c#
transport.Routing().RouteToEndpoint(typeof(OrderPlacedCommand), "Payments");
```

Event subscriptions will be automatically managed because the transport supports them natively.

## Additional Configuration

You can configure the default polling interval, which is how long the tranport waits before checking for new messages. 
The default value is 250ms but you can change it to any value so long as it's greater than zero and less than 1 minute.

``` c#
var config = new EndpointConfiguration("Orders");
var transport = config.UseTransport<InMemoryTransport>();
transport.PollingTime(TimeSpan.FromSeconds(1));
```

## Transport Limitations

### Warning: Do not use this in transport in Production, you will lose data

- The transport only supports a pub/sub model between endpoints hosted in the same process
  - E.g. two endpoints in different processes can not communicate with each other (send / publish / subscribe etc)
- There is no transaction support, retries will not work
