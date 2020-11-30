namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using Alpha.Handlers.Commands;
    using Alpha.Messages.Commands;
    using Alpha.Messages.Events;
    using Beta.Messages.Commands;
    using Helpers;
    using IntegrationTesting;
    using NUnit.Framework;

    [TestFixture]
    public class SendTests
    {
        public class SendTestsContext : IntegrationScenarioContext
        {
            public Guid CommandId { get; set; }

            public bool CommandHandledEventPublished
            {
                get
                {
                    var result = this.MessageWasProcessedByHandler<AlphaCommand, AlphaCommandHandler>();
                    if (!result) return false;

                    var message = this.OutgoingMessageOperations.First(operation => operation.MessageType == typeof(CommandProcessedInAlpha))?.MessageInstance;

                    if (!(message is CommandProcessedInAlpha commandHandledEvent)) return false;

                    Assert.AreEqual(CommandId, commandHandledEvent.Id, "The event published does not match the command sent.");
                    return true;
                }
            }

            public bool SendCommandHandled
            {
                get
                {
                    var result = this.MessageWasProcessedByHandler<AlphaCommand, AlphaCommandHandler>();
                    if (!result) return false;

                    var message = this.InvokedHandlers.FirstOrDefault(invocation => invocation.HandlerType == typeof(AlphaCommandHandler))?.Message;

                    if (!(message is AlphaCommand command)) return false;
                    
                    Assert.AreEqual(CommandId, command.Id, "The command sent does not match the command handled.");
                    return true;
                }
            }

            public virtual bool TestComplete => SendCommandHandled && CommandHandledEventPublished;// && BetaCommandHandled;

            public override string ToString()
            {
                return $"{nameof(SendCommandHandled)} {SendCommandHandled}\n{nameof(CommandHandledEventPublished)} {CommandHandledEventPublished}";
            }
        }

        public class SendTestsMultiEndpointContext : IntegrationScenarioContext
        {
            public Guid CommandId { get; set; }

            public bool BetaCommandHandled
            {
                get
                {
                    var result = this.MessageWasProcessedByHandler<AlphaCommand, AlphaCommandHandler>();
                    if (!result) return false;

                    var message = this.InvokedHandlers.FirstOrDefault(invocation => invocation.HandlerType == typeof(AlphaCommandHandler))?.Message;

                    if (!(message is AlphaCommand commandHandledEvent)) return false;

                    if (NotBefore != null && DateTime.UtcNow < NotBefore)
                    {
                        throw new Exception("Command arrived too early");
                    }

                    Assert.AreEqual(CommandId, commandHandledEvent.Id, "The event published does not match the command sent.");
                    return true;
                }
            }

            public bool TestComplete => BetaCommandHandled;

            public DateTime? NotBefore { get; set; }

            public override string ToString()
            {
                return $"{nameof(BetaCommandHandled)} {BetaCommandHandled}";
            }
        }

        [Test]
        public async Task SendingACommandAndWaitingForAnEventToBePublished()
        {
            var context = await
                Scenario.Define<SendTestsContext>()
                    .WithEndpoint<AlphaServer>(behavior =>
                    {
                        behavior.When(async (session, ctx) =>
                        {
                            await session.SendLocal(new AlphaCommand
                            {
                                Id = ctx.CommandId = Guid.NewGuid()
                            });
                        });
                    })
                    .Done(ctx => ctx.TestComplete)
                    .Run(TimeSpan.FromSeconds(15));

            Assert.True(context.TestComplete, context.ToString());
        }

        [Test]
        public async Task DeferringACommandAndWaitingForAnEventToBePublished()
        {
            var context = await
                Scenario.Define<SendTestsMultiEndpointContext>()
                    .WithEndpoint<AlphaServer>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            // first command should never be used, but the second one should
                            var sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DelayDeliveryWith(TimeSpan.FromDays(1));

                            await session.Send(new AlphaCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);

                            sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(15));

                            ctx.NotBefore = DateTime.UtcNow + TimeSpan.FromSeconds(10);

                            await session.Send(new AlphaCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);
                        });
                    })
                    .Done(ctx => ctx.TestComplete)
                    .Run(TimeSpan.FromSeconds(300));

            Assert.True(context.TestComplete, context.ToString());
        }

        [Test]
        public async Task DeferringACommandWithNotBeforeAndWaitingForAnEventToBePublished()
        {
            var context = await
                Scenario.Define<SendTestsMultiEndpointContext>()
                    .WithEndpoint<AlphaServer>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            var sendOptions = new SendOptions();
                            sendOptions.RouteToThisEndpoint();
                            sendOptions.DoNotDeliverBefore(DateTime.UtcNow + TimeSpan.FromSeconds(15));

                            ctx.NotBefore = DateTime.UtcNow + TimeSpan.FromSeconds(10);

                            await session.Send(new AlphaCommand
                                {
                                    Id = ctx.CommandId = Guid.NewGuid()
                                },
                                sendOptions);
                        });
                    })
                    .Done(ctx => ctx.TestComplete)
                    .Run(TimeSpan.FromSeconds(300));

            Assert.True(context.TestComplete, context.ToString());
        }

        [Test]
        public async Task SendCommandFromOneEndpointToAnother()
        {
            var context = await
                Scenario.Define<SendTestsMultiEndpointContext>()
                    .WithEndpoint<AlphaServer>()
                    .WithEndpoint<BetaServer>(builder =>
                    {
                        builder.When(async (session, ctx) =>
                        {
                            await session.SendLocal(new SendCommandToAlpha()
                            {
                                Id = ctx.CommandId = Guid.NewGuid()
                            });
                        });
                    })
                    .Done(ctx => ctx.TestComplete)
                    .Run(TimeSpan.FromSeconds(15));

            Assert.True(context.TestComplete, context.ToString());
        }
    }
}
