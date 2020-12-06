namespace NServiceBus.Transport.InMemory.Tests.Helpers
{
    using AcceptanceTesting;
    using Beta.Messages.Commands;
    using IntegrationTesting;

    public class AlphaServer : EndpointConfigurationBuilder
    {
        public AlphaServer()
        {
            EndpointSetup<EndpointTemplate<AlphaServerEndpointConfiguration>>();
        }
    }


    public class AlphaServerEndpointConfiguration : EndpointConfiguration
    {
        public AlphaServerEndpointConfiguration() : base("Alpha")
        {
            this.UsePersistence<InMemoryPersistence>();
            var transport = this.UseTransport<InMemoryTransport>();
            transport.Routing().RouteToEndpoint(typeof(BetaCommand), "Beta");

        }
    }
}
