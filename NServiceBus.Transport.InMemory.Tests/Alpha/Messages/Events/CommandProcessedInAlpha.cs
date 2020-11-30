namespace NServiceBus.Transport.InMemory.Tests.Alpha.Messages.Events
{
    using System;

    public interface ICommandProcessedInAlpha
    {
        DateTime CreatedOn { get; set; }
        DateTime ProcessedOn { get; set; }
        Guid Id { get; set; }
    }

    public class CommandProcessedInAlpha : IEvent, ICommandProcessedInAlpha
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
        public Guid Id { get; set; }
    }
}
