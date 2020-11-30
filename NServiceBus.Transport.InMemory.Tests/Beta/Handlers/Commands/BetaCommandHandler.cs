namespace NServiceBus.Transport.InMemory.Tests.Beta.Handlers.Commands
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Commands;
    using Messages.Events;

    public class BetaCommandHandler : IHandleMessages<BetaCommand>
    {
        public readonly ILog Log = LogManager.GetLogger<BetaCommandHandler>();

        public async Task Handle(BetaCommand message, IMessageHandlerContext context)
        {
            Log.Info("Beta.IHandleMessages<BetaCommand>");

            await context.Publish<CommandProcessedInBeta>(e =>
            {
                e.CreatedOn = message.CreatedOn;
                e.Id = message.Id;
            });
        }
    }
}
