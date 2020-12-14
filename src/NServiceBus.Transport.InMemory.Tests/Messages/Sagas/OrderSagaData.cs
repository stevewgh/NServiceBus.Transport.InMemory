namespace NServiceBus.Transport.InMemory.Tests.Messages.Sagas
{
    using System;

    public class OrderSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? TimedOutOn { get; set; }

        public Guid SagaId { get; set; }
    }
}
