namespace NServiceBus.Transport.InMemory.Tests.Orders.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Messages.Events;
    using Messages.Commands;
    using Tests.Messages.Sagas;

    public class OrderSaga : Saga<OrderSagaData>, IAmStartedByMessages<StartOrderSaga>, IHandleTimeouts<OrderSagaTimeout>, IHandleMessages<StopOrderSaga>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrderSaga>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
            mapper.ConfigureMapping<StopOrderSaga>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
            mapper.ConfigureMapping<OrderSagaTimeout>(model => model.SagaId)
                .ToSaga(model => model.SagaId);
        }

        public async Task Handle(StartOrderSaga message, IMessageHandlerContext context)
        {
            Data.CreatedOn = DateTime.UtcNow;
            Data.SagaId = message.SagaId;

            await RequestTimeout(context, DateTime.UtcNow.AddSeconds(message.TimeoutInSeconds), new OrderSagaTimeout
            {
                SagaId = Data.SagaId
            });
        }

        public async Task Timeout(OrderSagaTimeout state, IMessageHandlerContext context)
        {
            Data.TimedOutOn = DateTime.UtcNow;

            await context.SendLocal(new StopOrderSaga {SagaId = state.SagaId });
        }

        public async Task Handle(StopOrderSaga message, IMessageHandlerContext context)
        {
            await context.Publish(new OrderSagaCompleted
            {
                CreatedOn = Data.CreatedOn,
                TimedOutOn = Data.TimedOutOn,
                SagaId = Data.SagaId
            });

            MarkAsComplete();
        }
    }
}
