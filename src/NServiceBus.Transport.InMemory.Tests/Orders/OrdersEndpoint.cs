namespace NServiceBus.Transport.InMemory.Tests.Orders
{
    using System;
    using AcceptanceTesting;
    using IntegrationTesting;

    public class OrdersEndpoint : EndpointConfigurationBuilder
    {
        public OrdersEndpoint()
        {
            EndpointSetup<EndpointTemplate<OrdersEndpointConfiguration>>();
        }
    }

    public class OrdersEndpointConfiguration : EndpointConfiguration
    {
        public OrdersEndpointConfiguration() : base("Orders")
        {
            this.UsePersistence<InMemoryPersistence>();
            var transport = this.UseTransport<InMemoryTransport>();
            transport.PollingTime(TimeSpan.FromSeconds(1));
        }
    }
}
