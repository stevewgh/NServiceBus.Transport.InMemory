namespace NServiceBus.Transport.InMemory.Tests.Payments
{
    using AcceptanceTesting;
    using IntegrationTesting;
    using Tests.Messages.Commands;

    public class PaymentsEndpoint : EndpointConfigurationBuilder
    {
        public PaymentsEndpoint()
        {
            EndpointSetup<EndpointTemplate<PaymentsEndpointConfiguration>>();
        }
    }

    public class PaymentsEndpointConfiguration : EndpointConfiguration
    {
        public PaymentsEndpointConfiguration() : base("Payments")
        {
            this.UsePersistence<InMemoryPersistence>();
            var transport = this.UseTransport<InMemoryTransport>();
            transport.Routing().RouteToEndpoint(typeof(CancelOrderCommand), "Orders");
        }
    }
}