namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Commands
{
    using System;

    public class StartAlphaSaga : ICommand
    {
        public Guid SagaId { get; set; } = Guid.NewGuid();
        public int TimeoutInSeconds { get; set; } = 5;
    }
}
