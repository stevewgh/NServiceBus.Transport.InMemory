namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using IntegrationTesting;
    using Messages.Commands;
    using Messages.Events;
    using NUnit.Framework;
    using Orders;
    using Payments;

    [TestFixture]
    public class SendTests
    {
        public class SendTestsContext : IntegrationScenarioContext
        {
            public Guid CommandId { get; set; }

            public bool OrderCreatedEventPublished
            {
                get
                {
                    var message = this.OutgoingMessageOperations.FirstOrDefault(operation => operation.MessageType == typeof(OrderCreated))?.MessageInstance;

                    if (!(message is OrderCreated commandHandledEvent)) return false;

                    Assert.AreEqual(CommandId, commandHandledEvent.Id, "The event published does not match the command sent.");
                    return true;
                }
            }

            public bool OrderCancelledPublished
            {
                get
                {
                    var message = this.OutgoingMessageOperations.FirstOrDefault(operation => operation.MessageType == typeof(IOrderCancelled))?.MessageInstance;

                    if (!(message is IOrderCancelled eventHandled)) return false;

                    Assert.AreEqual(CommandId, eventHandled.Id, "The event published does not match the command sent.");
                    return true;
                }
            }

            public override string ToString()
            {
                return $"{nameof(OrderCancelledPublished)} {OrderCancelledPublished}\n{nameof(OrderCreatedEventPublished)} {OrderCreatedEventPublished}";
            }
        }

        [Test]
        public async Task When_An_Order_Is_Created_Then_An_Order_Created_Event_Is_Published()
        {
            var context = await
                Scenario.Define<SendTestsContext>()
                    .WithEndpoint<OrdersEndpoint>(behavior =>
                    {
                        behavior.When(async (session, ctx) =>
                        {
                            await session.SendLocal(new CreateOrderCommand
                            {
                                Id = ctx.CommandId = Guid.NewGuid()
                            });
                        });
                    })
                    .Done(ctx => ctx.OrderCreatedEventPublished)
                    .Run(TimeSpan.FromSeconds(15));

            Assert.True(context.OrderCreatedEventPublished, context.ToString());
        }

        [Test]
        public async Task When_Fraud_Is_Detected_Then_The_Order_Is_Cancelled_And_The_Payment_Is_Cancelled()
        {
            var context = await
                Scenario.Define<SendTestsContext>()
                    .WithEndpoint<OrdersEndpoint>()
                    .WithEndpoint<PaymentsEndpoint>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            await session.SendLocal(new FraudDetectedCommand
                            {
                                Id = ctx.CommandId = Guid.NewGuid()
                            });
                        });
                    })
                    .Done(ctx => ctx.OrderCancelledPublished)
                    .Run(TimeSpan.FromSeconds(15));

            Assert.True(context.OrderCancelledPublished, context.ToString());
        }
    }
}
