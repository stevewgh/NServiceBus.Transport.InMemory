namespace NServiceBus.Transport.InMemory.Tests.Beta.Messages.Commands
{
    using System;

    public class SendCommandToAlpha : ICommand
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
