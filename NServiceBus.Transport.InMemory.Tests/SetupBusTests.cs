namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using Helpers;
    using IntegrationTesting;
    using NUnit.Framework;

    [TestFixture]
    public class SetupBusTests
    {
        [Test]
        public async Task SetupBus()
        {
            var context = await
                Scenario.Define<IntegrationScenarioContext>()
                    .WithEndpoint<AlphaServer>()
                    .WithEndpoint<BetaServer>()
                    .Done(ctx => ctx.EndpointsStarted)
                    .Run(TimeSpan.FromSeconds(30));

            Assert.True(context.EndpointsStarted);
        }
    }
}
