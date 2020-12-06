namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Commands
{
    using System;

    public class SendCommandToBeta : ICommand
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
