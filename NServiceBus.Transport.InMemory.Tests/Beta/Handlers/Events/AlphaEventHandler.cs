namespace NServiceBus.Transport.InMemory.Tests.Beta.Handlers.Events
{
    using System.Threading.Tasks;
    using Alpha.Messages.Events;
    using Logging;

    public class AlphaEventHandler : IHandleMessages<ICommandProcessedInAlpha>
    {
        public readonly ILog Log = LogManager.GetLogger<AlphaEventHandler>();

        public Task Handle(ICommandProcessedInAlpha message, IMessageHandlerContext context)
        {
            Log.Info("Beta.IHandleMessages<CommandProcessedInAlpha>");
            return Task.CompletedTask;
        }
    }
}
