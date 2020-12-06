namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Events
{
    using System.Threading.Tasks;
    using Logging;
    using Messages.Events;

    public class AlphaSagaCompletedHandler : IHandleMessages<AlphaSagaCompleted>
    {
        public readonly ILog Log = LogManager.GetLogger<AlphaEventHandler>();

        public Task Handle(AlphaSagaCompleted message, IMessageHandlerContext context)
        {
            Log.Info("Alpha.IHandleMessages<AlphaEventHandler>");
            return Task.CompletedTask;
        }
    }
}
