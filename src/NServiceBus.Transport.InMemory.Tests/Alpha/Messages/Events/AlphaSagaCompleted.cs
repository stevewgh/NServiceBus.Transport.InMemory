namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Events
{
    using System;

    public class AlphaSagaCompleted : IEvent
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? TimedOutOn { get; set; }
        public Guid SagaId { get; set; }
    }
}
