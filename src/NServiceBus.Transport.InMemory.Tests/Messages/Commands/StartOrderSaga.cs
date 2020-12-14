namespace NServiceBus.Transport.InMemory.Tests.Messages.Commands
{
    using System;

    public class StartOrderSaga : ICommand
    {
        public Guid SagaId { get; set; } = Guid.NewGuid();

        public int TimeoutInSeconds { get; set; } = 5;
    }
}
