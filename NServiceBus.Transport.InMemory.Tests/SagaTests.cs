namespace NServiceBus.Transport.InMemory.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using Alpha.Handlers.Sagas;
    using Alpha.Messages.Commands;
    using Alpha.Messages.Events;
    using Helpers;
    using IntegrationTesting;
    using NUnit.Framework;

    [TestFixture]
    public class SagaTests
    {
        public class SagaTestsContext : IntegrationScenarioContext
        {
            public Guid SagaId { get; set; }

            public bool StartSagaCommandHandled
            {
                get
                {
                    var result = this.MessageWasProcessedBySaga<StartAlphaSaga, TestSaga>();
                    if (!result) return false;

                    var message = this.InvokedSagas.First(operation => operation.MessageType == typeof(StartAlphaSaga))?.Message;

                    if (!(message is StartAlphaSaga commandHandledEvent)) return false;

                    Assert.AreEqual(SagaId, commandHandledEvent.SagaId, "The start saga command sent does not match the test saga.");
                    return true;
                }
            }

            public bool SagaTimeoutHandled
            {
                get
                {
                    var result = this.MessageWasProcessedBySaga<AlphaSagaTimeout, TestSaga>();
                    if (!result) return false;

                    var message = this.InvokedSagas.First(operation => operation.MessageType == typeof(AlphaSagaTimeout))?.Message;

                    if (!(message is AlphaSagaTimeout commandHandledEvent)) return false;

                    Assert.AreEqual(SagaId, commandHandledEvent.SagaId, "The saga timeout command sent does not match the test saga.");
                    return true;
                }
            }

            public bool StopSagaCommandHandled
            {
                get
                {
                    var result = this.MessageWasProcessedBySaga<StopAlphaSaga, TestSaga>();
                    if (!result) return false;

                    var message = this.InvokedSagas.First(operation => operation.MessageType == typeof(StopAlphaSaga))?.Message;

                    if (!(message is StopAlphaSaga commandHandledEvent)) return false;

                    Assert.AreEqual(SagaId, commandHandledEvent.SagaId, "The stop saga command sent does not match the test saga.");
                    return true;
                }

            }

            public bool SagaStoppedEventHandled
            {
                get
                {
                    var result = this.MessageWasProcessedBySaga<StopAlphaSaga, TestSaga>();
                    if (!result) return false;

                    var message = this.OutgoingMessageOperations.First(operation => operation.MessageType == typeof(AlphaSagaCompleted))?.MessageInstance;

                    if (!(message is AlphaSagaCompleted commandHandledEvent)) return false;

                    Assert.AreEqual(SagaId, commandHandledEvent.SagaId, "The saga completed event sent does not match the test saga.");
                    return true;
                }
            }

            public bool TestComplete => StartSagaCommandHandled && SagaTimeoutHandled && StopSagaCommandHandled && SagaStoppedEventHandled;
        }

        [Test]
        public async Task TestSaga()
        {

            var context = await
                Scenario.Define<SagaTestsContext>()
                    .WithEndpoint<AlphaServer>(behavior =>
                    {
                        behavior.When(async (session, ctx) =>
                        {
                            await session.SendLocal(new StartAlphaSaga
                            {
                                SagaId = ctx.SagaId = Guid.NewGuid(),
                                TimeoutInSeconds = 5
                            });
                        });
                    })
                    .Done(ctx => ctx.TestComplete)
                    .Run(TimeSpan.FromSeconds(15));

            Assert.True(context.TestComplete, context.ToString());



            //var context = await
            //    Scenario.Define<IntegrationScenarioContext>()
            //        .WithEndpoint<AlphaServer>(behavior => behavior
            //            .When(ctx => ctx.EndpointsStarted && !ctx.StartSagaCommandSent, (bus, ctx) =>
            //            {
            //                ctx.StartSagaCommandSent = true;
            //                ctx.Events.MessageHandled += args =>
            //                {
            //                    if (args.Exception != null)
            //                    {
            //                        return;
            //                    }

            //                    var startSagaCommand = args.Message as StartAlphaSaga;
            //                    if (startSagaCommand != null)
            //                    {
            //                        Assert.AreEqual(ctx.SagaId, startSagaCommand.SagaId, "The start saga command sent does not match the test saga.");
            //                        ctx.StartSagaCommandHandled = true;
            //                    }

            //                    var alphaSagaTimeout = args.Message as AlphaSagaTimeout;
            //                    if (alphaSagaTimeout != null)
            //                    {
            //                        Assert.AreEqual(ctx.SagaId, alphaSagaTimeout.SagaId, "The saga timeout does not match test saga.");
            //                        ctx.SagaTimeoutHandled = true;
            //                        bus.Send(new StopAlphaSaga
            //                        {
            //                            SagaId = ctx.SagaId
            //                        });
            //                    }

            //                    var stopSagaCommand = args.Message as StopAlphaSaga;
            //                    if (stopSagaCommand != null && ctx.SagaTimeoutHandled)
            //                    {
            //                        Assert.AreEqual(ctx.SagaId, stopSagaCommand.SagaId, "The stop saga command sent does not match the test saga.");
            //                        ctx.StopSagaCommandHandled = true;
            //                    }

            //                    var stopSagaEvent = args.Message as AlphaSagaCompleted;
            //                    if (stopSagaEvent != null && ctx.StopSagaCommandHandled)
            //                    {
            //                        Assert.AreEqual(ctx.SagaId, stopSagaEvent.SagaId, "The saga stopped event does not match the test saga.");
            //                        Assert.IsTrue(stopSagaEvent.TimedOutOn - stopSagaEvent.CreatedOn > TimeSpan.FromSeconds(14), "The saga did not actually do a delay for the timeout.");
            //                        ctx.SagaStoppedEventHandled = true;
            //                    }
            //                };
            //                bus.Send(new StartAlphaSaga
            //                {
            //                    SagaId = ctx.SagaId = Guid.NewGuid(),
            //                    TimeoutInSeconds = 15
            //                });
            //            }))
            //    .Done(ctx => ctx.TestComplete)
            //    .Run(TimeSpan.FromMinutes(5)).TestComplete, 
            //    "The test did not complete");
        }
    }
}
