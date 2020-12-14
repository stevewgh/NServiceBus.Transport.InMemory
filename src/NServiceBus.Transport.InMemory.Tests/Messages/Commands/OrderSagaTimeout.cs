namespace NServiceBus.Transport.InMemory.Tests.Messages.Commands
{
    using System;

    public class OrderSagaTimeout : ICommand
    {
        public Guid SagaId { get; set; }
    }
}
