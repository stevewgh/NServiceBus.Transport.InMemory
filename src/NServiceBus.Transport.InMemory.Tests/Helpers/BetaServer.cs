namespace NServiceBus.Transport.InMemory.Tests.Helpers
{
    using AcceptanceTesting;
    using Alpha.Messages.Commands;
    using Beta.Messages.Commands;
    using IntegrationTesting;

    public class BetaServer : EndpointConfigurationBuilder
    {
        public BetaServer()
        {
            EndpointSetup<EndpointTemplate<BetaServerEndpointConfiguration>>();
        }
    }

    public class BetaServerEndpointConfiguration : EndpointConfiguration
    {
        public BetaServerEndpointConfiguration() : base("Beta")
        {
            this.UsePersistence<InMemoryPersistence>();
            var transport = this.UseTransport<InMemoryTransport>();
            transport.Routing().RouteToEndpoint(typeof(AlphaCommand), "Alpha");
        }
    }
}