namespace NServiceBus.Transport.InMemory.Tests.Messages.Commands
{
    using System;

    public class StopOrderSaga : ICommand
    {
        public Guid SagaId { get; set; }
    }
}
