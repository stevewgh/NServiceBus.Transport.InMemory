namespace NServiceBus.Transport.InMemory.Tests.Orders.Handlers
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class OrderSagaCompletedHandler : IHandleMessages<OrderSagaCompleted>
    {
        public readonly ILog Log = LogManager.GetLogger<OrderCreatedHandler>();

        public Task Handle(OrderSagaCompleted message, IMessageHandlerContext context)
        {
            Log.Info("Orders.IHandleMessages<OrderSagaCompleted>");
            return Task.CompletedTask;
        }
    }
}
