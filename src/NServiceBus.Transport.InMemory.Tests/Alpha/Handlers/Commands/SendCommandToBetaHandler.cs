namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Commands
{
    using System.Threading.Tasks;
    using Beta.Messages.Commands;
    using Logging;
    using Messages.Commands;

    public class SendCommandToBetaHandler : IHandleMessages<SendCommandToBeta>
    {
        public readonly ILog Log = LogManager.GetLogger<SendCommandToBetaHandler>();

        public async Task Handle(SendCommandToBeta message, IMessageHandlerContext context)
        {
            Log.Info("Alpha.IHandleMessages<SendCommandToBeta>");

            await context.Send(new BetaCommand
            {
                Id = message.Id
            });
        }
    }
}
