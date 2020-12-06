namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Events
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class AlphaEventHandler : IHandleMessages<CommandProcessedInAlpha>
    {
        public readonly ILog Log = LogManager.GetLogger<AlphaEventHandler>();

        public Task Handle(CommandProcessedInAlpha message, IMessageHandlerContext context)
        {
            Log.Info("Alpha.IHandleMessages<CommandProcessedInAlpha>");
            return Task.CompletedTask;
        }
    }
}
