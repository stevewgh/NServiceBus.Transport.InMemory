namespace NServiceBus.Transport.InMemory.Tests.Messages.Events
{
    using System;

    public class OrderSagaCompleted : IEvent
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? TimedOutOn { get; set; }
        public Guid SagaId { get; set; }
    }
}
