namespace NServiceBus.Transport.InMemory.Tests.Alpha.Handlers.Commands
{
    using Beta.Messages.Commands;
    using Logging;
    using Messages.Commands;

    public class SendCommandToBetaHandler : IHandleMessages<SendCommandToBeta>
    {
        public IBus Bus { get; set; }
        public readonly ILog Log = LogManager.GetLogger<SendCommandToBetaHandler>();
        public void Handle(SendCommandToBeta command)
        {
            Log.Info("Alpha.IHandleMessages<SendCommandToBeta>");

            Bus.Send(new BetaCommand
            {
                Id = command.Id
            });
        }
    }
}
