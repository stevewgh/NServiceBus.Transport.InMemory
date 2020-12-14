namespace NServiceBus.Transport.InMemory.Tests.Messages.Events
{
    using System;

    public interface IOrderCreated : IEvent
    {
        DateTime CreatedOn { get; set; }
        DateTime ProcessedOn { get; set; }
        Guid Id { get; set; }
    }

    public class OrderCreated : IOrderCreated
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; }
    }
}
