namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Commands
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Commands;
    using Messages.Events;

    public class AlphaCommandHandler : IHandleMessages<AlphaCommand>
    {
        public readonly ILog Log = LogManager.GetLogger<AlphaCommandHandler>();

        public async Task Handle(AlphaCommand message, IMessageHandlerContext context)
        {
            Log.Info("Alpha.IHandleMessages<AlphaCommand>");

            await context.Publish<CommandProcessedInAlpha>(alpha =>
            {
                alpha.CreatedOn = message.CreatedOn;
                alpha.Id = message.Id;
            });
        }
    }
}
