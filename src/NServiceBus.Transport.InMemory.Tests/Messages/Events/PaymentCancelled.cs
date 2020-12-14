namespace NServiceBus.Transport.InMemory.Tests.Messages.Events
{
    using System;

    public interface IPaymentCancelled : IEvent
    {
        DateTime CancelledOn { get; set; }
        DateTime ProcessedOn { get; set; }
        Guid Id { get; set; }
    }

    public class PaymentCancelled : IPaymentCancelled
    {
        public DateTime CancelledOn { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; }
    }
}
