namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using IntegrationTesting;
    using Messages.Commands;
    using NUnit.Framework;
    using Orders;
    using Orders.Handlers;

    [TestFixture]
    public class DeferredSendTests
    {
        public class DeferredSendEndpointContext : IntegrationScenarioContext
        {
            public Guid CommandId { get; set; }

            public bool CreateOrderCommandHandled
            {
                get
                {
                    var result = this.MessageWasProcessedByHandler<CreateOrderCommand, CreateOrderHandler>();
                    if (!result) return false;

                    var message = this.InvokedHandlers.FirstOrDefault(invocation => invocation.HandlerType == typeof(CreateOrderHandler))?.Message;

                    if (!(message is CreateOrderCommand commandHandledEvent)) return false;

                    if (NotBefore != null && DateTime.UtcNow < NotBefore)
                    {
                        throw new Exception("Command arrived too early");
                    }

                    Assert.AreEqual(CommandId, commandHandledEvent.Id, "The event published does not match the command sent.");
                    return true;
                }
            }

            public DateTime? NotBefore { get; set; }

            public override string ToString()
            {
                return $"{nameof(CreateOrderCommandHandled)} {CreateOrderCommandHandled}";
            }
        }

        [Test]
        public async Task DeferringACommandAndWaitingForAnEventToBePublished()
        {
            var context = await
                Scenario.Define<DeferredSendEndpointContext>()
                    .WithEndpoint<OrdersEndpoint>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            // first command should never be used, but the second one should
                            var sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DelayDeliveryWith(TimeSpan.FromDays(1));

                            await session.Send(new CreateOrderCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);

                            sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(15));

                            ctx.NotBefore = DateTime.UtcNow + TimeSpan.FromSeconds(10);

                            await session.Send(new CreateOrderCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);
                        });
                    })
                    .Done(ctx => ctx.CreateOrderCommandHandled)
                    .Run(TimeSpan.FromSeconds(300));

            Assert.True(context.CreateOrderCommandHandled, context.ToString());
        }

        [Test]
        public async Task DeferringACommandWithNotBeforeAndWaitingForAnEventToBePublished()
        {
            var context = await
                Scenario.Define<DeferredSendEndpointContext>()
                    .WithEndpoint<OrdersEndpoint>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            var sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DoNotDeliverBefore(DateTime.UtcNow + TimeSpan.FromSeconds(15));

                            ctx.NotBefore = DateTime.UtcNow + TimeSpan.FromSeconds(10);

                            await session.Send(new CreateOrderCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);
                        });
                    })
                    .Done(ctx => ctx.CreateOrderCommandHandled)
                    .Run(TimeSpan.FromSeconds(300));

            Assert.True(context.CreateOrderCommandHandled, context.ToString());
        }
    }
}
