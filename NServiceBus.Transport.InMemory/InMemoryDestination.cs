namespace NServiceBus.Transport.InMemory
{
    using System;

    class InMemoryDestination
    {
        private readonly string destination;

        public InMemoryDestination(string destination)
        {
            Guard.AgainstNullAndEmpty(nameof(destination), destination);
            this.destination = destination;
            var parts = this.destination.Split('@');
            if (parts.Length == 0 || parts.Length > 2)
            {
                throw new ArgumentException("destination must be either queue@endpoint or endpoint");
            }

            if (parts.Length == 1)
            {
                EndPoint = destination;
                Queue = "receiving";
            }
            else
            {
                EndPoint = parts[1];
                Queue = parts[0];
            }
        }

        public string EndPoint { get; }

        public string Queue { get; }
    }
}
