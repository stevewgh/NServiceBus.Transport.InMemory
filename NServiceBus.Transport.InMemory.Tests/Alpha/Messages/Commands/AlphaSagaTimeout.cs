namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Commands
{
    using System;

    public class AlphaSagaTimeout : ICommand
    {
        public Guid SagaId { get; set; }
    }
}
