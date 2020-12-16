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

## Examples

I recommend that you take a look at the [SendTests.cs](https://github.com/stevewgh/NServiceBus.Transport.InMemory/blob/ce7c87896bbbe1d7cb45770316901796c7a4c7ef/src/NServiceBus.Transport.InMemory.Tests/SendTests.cs#L54) test class which makes use of the excellent [NServiceBus.IntegrationTesting library by Mauro Servienti](https://github.com/mauroservienti/NServiceBus.IntegrationTesting/). There's plenty of examples on Mauro's page, just swap out the Learning transport for the InMemory transport and you have a very clean test.

Alternatively, you can use an AAA/GWT approach to testing the endpoints.

``` c#
[Test]
public async Task Given_An_Endpoint_When_Receiving_A_Command_Then_The_3rd_Party_Is_Called()
{
    var thirdParty = new Mock<IThirdPartyClient>();
    var config = new EndpointConfiguration("TestEndpoint");
    config.UseTransport<InMemoryTransport>();
    config.DisableFeature<Sagas>();
    config.Conventions().DefiningCommandsAs(t => t == typeof(ICommandToDoWork));
    config.RegisterComponents(c => c.RegisterSingleton(thirdParty.Object));
    var endpoint = await Endpoint.Start(config);
    
    await Task.WhenAll(
        endpoint.SendLocal<ICommandToDoWork>(command => command.Id = Guid.NewGuid()),
        Task.Delay(TimeSpan.FromSeconds(5)));
    await endpoint.Stop();

    thirdParty.Verify(client => client.Send(), Times.Once);
}
```

## Transport Limitations

### Warning: Do not use this in transport in Production, you will lose data

- The transport only supports a pub/sub model between endpoints hosted in the same process
  - E.g. two endpoints in different processes can not communicate with each other (send / publish / subscribe etc)
- There is no transaction support, retries will not work
