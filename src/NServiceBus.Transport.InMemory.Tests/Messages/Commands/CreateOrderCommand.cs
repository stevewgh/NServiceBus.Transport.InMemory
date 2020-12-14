namespace NServiceBus.Transport.InMemory.Tests.Messages.Commands
{
    using System;

    public class CreateOrderCommand : ICommand
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
