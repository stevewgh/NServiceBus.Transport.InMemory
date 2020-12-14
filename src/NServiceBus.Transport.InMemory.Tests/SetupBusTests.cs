namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using IntegrationTesting;
    using NUnit.Framework;
    using Orders;
    using Payments;

    [TestFixture]
    public class SetupBusTests
    {
        [Test]
        public async Task SetupBus()
        {
            var context = await
                Scenario.Define<IntegrationScenarioContext>()
                    .WithEndpoint<OrdersEndpoint>()
                    .WithEndpoint<PaymentsEndpoint>()
                    .Done(ctx => ctx.EndpointsStarted)
                    .Run(TimeSpan.FromSeconds(30));

            Assert.True(context.EndpointsStarted);
        }
    }
}
