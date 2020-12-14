namespace NServiceBus.Transport.InMemory.Tests.Orders.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;
    using Messages.Commands;

    public class CancelOrderHandler : IHandleMessages<CancelOrderCommand>
    {
        public readonly ILog Log = LogManager.GetLogger<CancelOrderHandler>();

        public async Task Handle(CancelOrderCommand message, IMessageHandlerContext context)
        {
            Log.Info("Orders.IHandleMessages<CancelOrderHandler>");

            await context.Publish<IOrderCancelled>(e =>
            {
                e.CancelledOn = DateTime.UtcNow;
                e.Id = message.Id;
                e.ProcessedOn = DateTime.UtcNow;
            });
        }
    }
}
