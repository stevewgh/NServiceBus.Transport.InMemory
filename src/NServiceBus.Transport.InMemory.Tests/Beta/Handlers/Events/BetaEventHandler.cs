namespace NServiceBus.Transport.InMemory.Tests.Beta.Handlers.Events
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class BetaEventHandler : IHandleMessages<CommandProcessedInBeta>
    {
        public readonly ILog Log = LogManager.GetLogger<BetaEventHandler>();
        
        public Task Handle(CommandProcessedInBeta message, IMessageHandlerContext context)
        {
            Log.Info("Beta.IHandleMessages<CommandProcessedInBeta>");
            return Task.CompletedTask;
        }
    }
}
