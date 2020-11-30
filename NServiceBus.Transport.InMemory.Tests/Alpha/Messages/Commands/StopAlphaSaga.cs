namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Commands
{
    using System;

    public class StopAlphaSaga : ICommand
    {
        public Guid SagaId { get; set; }
    }
}
