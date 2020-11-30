namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Sagas
{
    using System;

    public class TestSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? TimedOutOn { get; set; }

        public Guid SagaId { get; set; }
    }
}
