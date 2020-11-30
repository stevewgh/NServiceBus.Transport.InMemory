namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Threading.Tasks;
    using Extensibility;

    internal class InMemorySubscriptionManager : IManageSubscriptions
    {
        private readonly InMemorySettings settings;

        public InMemorySubscriptionManager(InMemorySettings settings)
        {
            Guard.AgainstNull(nameof(settings), settings);
            this.settings = settings;
        }

        public Task Subscribe(Type eventType, ContextBag context)
        {
            SharedStorage.Instance.Subscribe(this.settings.EndpointName, eventType);
            return Task.CompletedTask;
        }

        public Task Unsubscribe(Type eventType, ContextBag context)
        {
            SharedStorage.Instance.Unsubscribe(this.settings.EndpointName, eventType);
            return Task.CompletedTask;
        }
    }
}
