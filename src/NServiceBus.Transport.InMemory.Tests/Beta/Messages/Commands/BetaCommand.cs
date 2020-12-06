namespace NServiceBus.Transport.InMemory.Tests.Beta.Messages.Commands
{
    using System;

    public class BetaCommand : ICommand
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
