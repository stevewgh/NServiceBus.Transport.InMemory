namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Events
{
    using System.Threading.Tasks;
    using Beta.Messages.Events;
    using Logging;

    public class BetaEventHandler : IHandleMessages<CommandProcessedInBeta>
    {
        public readonly ILog Log = LogManager.GetLogger<BetaEventHandler>();

        public Task Handle(CommandProcessedInBeta message, IMessageHandlerContext context)
        {
            Log.Info("Alpha.IHandleMessages<CommandProcessedInBeta>");
            return Task.CompletedTask;
        }
    }
}
