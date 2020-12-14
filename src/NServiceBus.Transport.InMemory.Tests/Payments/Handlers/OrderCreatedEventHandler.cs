namespace NServiceBus.Transport.InMemory.Tests.Payments.Handlers
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class OrderCreatedEventHandler : IHandleMessages<IOrderCreated>
    {
        public readonly ILog Log = LogManager.GetLogger<OrderCreatedEventHandler>();

        public Task Handle(IOrderCreated message, IMessageHandlerContext context)
        {
            Log.Info("Payments.IHandleMessages<OrderCreatedEventHandler>");
            return Task.CompletedTask;
        }
    }
}