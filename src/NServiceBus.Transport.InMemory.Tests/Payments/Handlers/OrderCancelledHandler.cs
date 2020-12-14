namespace NServiceBus.Transport.InMemory.Tests.Payments.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class OrderCancelledHandler : IHandleMessages<IOrderCancelled>
    {
        public readonly ILog Log = LogManager.GetLogger<OrderCancelledHandler>();

        public async Task Handle(IOrderCancelled message, IMessageHandlerContext context)
        {
            Log.Info("Payments.IHandleMessages<IOrderCancelled>");

            await context.Publish<IPaymentCancelled>(e =>
            {
                e.CancelledOn = DateTime.UtcNow;
                e.ProcessedOn = DateTime.UtcNow;
                e.Id = message.Id;
            });
        }
    }
}
