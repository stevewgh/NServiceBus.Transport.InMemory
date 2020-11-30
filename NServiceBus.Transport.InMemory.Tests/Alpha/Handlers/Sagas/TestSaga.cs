namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Sagas
{
    using System;
    using System.Threading.Tasks;
    using Messages.Commands;
    using Messages.Events;
    using Messages.Sagas;

    public class TestSaga : Saga<TestSagaData>, IAmStartedByMessages<StartAlphaSaga>, IHandleTimeouts<AlphaSagaTimeout>, IHandleMessages<StopAlphaSaga>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
        {
            mapper.ConfigureMapping<StartAlphaSaga>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
            mapper.ConfigureMapping<StopAlphaSaga>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
            mapper.ConfigureMapping<AlphaSagaTimeout>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
        }

        public async Task Handle(StartAlphaSaga message, IMessageHandlerContext context)
        {
            Data.CreatedOn = DateTime.UtcNow;
            Data.SagaId = message.SagaId;

            await RequestTimeout(context, DateTime.UtcNow.AddSeconds(message.TimeoutInSeconds), new AlphaSagaTimeout
            {
                SagaId = Data.SagaId
            });
        }

        public async Task Timeout(AlphaSagaTimeout state, IMessageHandlerContext context)
        {
            Data.TimedOutOn = DateTime.UtcNow;

            await context.SendLocal(new StopAlphaSaga {SagaId = state.SagaId });
        }

        public async Task Handle(StopAlphaSaga message, IMessageHandlerContext context)
        {
            await context.Publish(new AlphaSagaCompleted
            {
                CreatedOn = Data.CreatedOn,
                TimedOutOn = Data.TimedOutOn,
                SagaId = Data.SagaId
            });
            MarkAsComplete();
        }
    }
}
