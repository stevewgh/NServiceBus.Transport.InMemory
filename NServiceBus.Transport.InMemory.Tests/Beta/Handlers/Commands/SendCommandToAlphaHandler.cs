namespace NServiceBus.Transport.InMemory.Tests.Beta.Handlers.Commands
{
    using System.Threading.Tasks;
    using Alpha.Messages.Commands;
    using Logging;
    using Messages.Commands;

    public class SendCommandToAlphaHandler : IHandleMessages<SendCommandToAlpha>
    {
        public readonly ILog Log = LogManager.GetLogger<SendCommandToAlphaHandler>();

        public async Task Handle(SendCommandToAlpha message, IMessageHandlerContext context)
        {
            Log.Info("Beta.IHandleMessages<SendCommandToAlpha>");

            await context.Send(new AlphaCommand
            {
                Id = message.Id
            });
        }
    }
}
