namespace NServiceBus.Transport.InMemory.Tests.Payments.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Logging;
    using Messages.Commands;

    public class FraudDetectedHandler : IHandleMessages<FraudDetectedCommand>
    {
        public readonly ILog Log = LogManager.GetLogger<FraudDetectedHandler>();

        public async Task Handle(FraudDetectedCommand message, IMessageHandlerContext context)
        {
            Log.Info("Payments.IHandleMessages<FraudDetectedCommand>");

            await context.Send(new CancelOrderCommand
            {
                Id = message.Id,
                CreatedOn = DateTime.UtcNow
            });
        }
    }
}
