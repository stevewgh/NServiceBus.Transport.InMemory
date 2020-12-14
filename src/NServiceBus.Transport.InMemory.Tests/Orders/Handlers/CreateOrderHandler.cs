namespace NServiceBus.Transport.InMemory.Tests.Orders.Handlers
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;
    using Tests.Messages.Commands;

    public class CreateOrderHandler : IHandleMessages<CreateOrderCommand>
    {
        public readonly ILog Log = LogManager.GetLogger<CreateOrderHandler>();

        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            Log.Info("Orders.IHandleMessages<CreateOrderCommand>");

            await context.Publish<OrderCreated>(alpha =>
            {
                alpha.CreatedOn = message.CreatedOn;
                alpha.Id = message.Id;
            });
        }
    }
}
