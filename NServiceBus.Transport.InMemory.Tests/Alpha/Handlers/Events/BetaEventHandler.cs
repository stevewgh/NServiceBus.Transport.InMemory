namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Events
{
    using Beta.Messages.Events;
    using Logging;

    public class BetaEventHandler : IHandleMessages<CommandProcessedInBeta>
    {
        public readonly ILog Log = LogManager.GetLogger<BetaEventHandler>();
        public void Handle(CommandProcessedInBeta message)
        {
            Log.Info("Alpha.IHandleMessages<CommandProcessedInBeta>");
        }
    }
}
