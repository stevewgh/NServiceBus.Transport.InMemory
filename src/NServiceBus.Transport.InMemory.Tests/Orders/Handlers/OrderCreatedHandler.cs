namespace NServiceBus.Transport.InMemory.Tests.Orders.Handlers
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class OrderCreatedHandler : IHandleMessages<OrderCreated>
    {
        public readonly ILog Log = LogManager.GetLogger<OrderCreatedHandler>();

        public Task Handle(OrderCreated message, IMessageHandlerContext context)
        {
            Log.Info("Orders.IHandleMessages<OrderCreated>");
            return Task.CompletedTask;
        }
    }
}
