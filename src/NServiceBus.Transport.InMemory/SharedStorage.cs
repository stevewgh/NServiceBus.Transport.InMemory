namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using DeliveryConstraints;
    using Extensibility;

    internal class SharedStorage
    {
        public static SharedStorage Instance { get; set; } = new SharedStorage();

        private readonly ConcurrentDictionary<string, EndpointStorage> endpointStorage = new ConcurrentDictionary<string, EndpointStorage>();

        private SharedStorage() { }

        public MessageContext? Query(string endpoint)
        {
            var storage = this.endpointStorage.GetOrAdd(endpoint, new EndpointStorage(endpoint));
            return storage.Query();
        }

        public void Send(InMemoryDestination destination, OutgoingMessage message, List<DeliveryConstraint> constraints)
        {
            var storage = this.endpointStorage.GetOrAdd(destination.EndPoint, new EndpointStorage(destination.EndPoint));
            storage.Enqueue(message, constraints);
        }

        public void Publish(OutgoingMessage message, Type messageType)
        {
            var messageTypes = new List<Type> { messageType };
            messageTypes.AddRange(messageType.GetInterfaces());
            if (messageType.BaseType != null) messageTypes.Add(messageType.BaseType);

            foreach (var storage in this.endpointStorage)
            {
                storage.Value.EnqueueIfSubscribed(message, messageTypes);
            }
        }

        public void Subscribe(string endpointName, Type eventType)
        {
            var storage = this.endpointStorage.GetOrAdd(endpointName, new EndpointStorage(endpointName));
            storage.Subscribe(eventType);
        }

        public void Unsubscribe(string endpointName, Type eventType)
        {
            var storage = this.endpointStorage.GetOrAdd(endpointName, new EndpointStorage(endpointName));
            storage.Unsubscribe(eventType);
        }
    }

    internal class EndpointStorage
    {
        private readonly string endpointName;
        private readonly ConcurrentQueue<IInMemoryMessageContext> receivingQueue = new ConcurrentQueue<IInMemoryMessageContext>();
        private readonly List<Type> subscriptions = new List<Type>();
        private readonly SortedList<DateTime, InMemoryDeferredMessageContext> deferredMessages = new SortedList<DateTime, InMemoryDeferredMessageContext>();

        public EndpointStorage(string endpointName)
        {
            this.endpointName = endpointName;
        }

        public MessageContext? Query()
        {
            if (deferredMessages.Any(pair => pair.Value.IsReadyToEnqueue))
            {
                var messagesToProcess = deferredMessages.Where(pair => pair.Value.IsReadyToEnqueue).ToList();
                foreach (var keyValuePair in messagesToProcess)
                {
                    var context = keyValuePair.Value.Context;
                    if (context != null)
                    {
                        receivingQueue.Enqueue(new InMemoryMessageContext(context));
                    }

                    deferredMessages.Remove(keyValuePair.Key);
                }
            }

            receivingQueue.TryDequeue(out var message);
            return message?.Context;
        }

        public void Enqueue(OutgoingMessage message, List<DeliveryConstraint> deliveryConstraints)
        {
            var context = new MessageContext(message.MessageId, message.Headers, message.Body,
                new TransportTransaction(), new CancellationTokenSource(), new ContextBag());

            var delayUntil = deliveryConstraints.DelayUntil();
            if (delayUntil == null)
            {
                receivingQueue.Enqueue(new InMemoryMessageContext(context));
                return;
            }

            deferredMessages.Add(delayUntil.Value, new InMemoryDeferredMessageContext(context, delayUntil.Value));
        }

        public void Subscribe(Type eventType)
        {
            if (!this.subscriptions.Contains(eventType))
            {
                this.subscriptions.Add(eventType);
            }
        }

        public void Unsubscribe(Type eventType)
        {
            if (this.subscriptions.Contains(eventType))
            {
                this.subscriptions.Remove(eventType);
            }
        }

        public void EnqueueIfSubscribed(OutgoingMessage message, IEnumerable<Type> messageTypes)
        {
            foreach (var messageType in messageTypes)
            {
                if (this.subscriptions.Contains(messageType))
                {
                    this.Enqueue(message, new List<DeliveryConstraint>());
                }
            }
        }

        private interface IInMemoryMessageContext
        {
            MessageContext? Context { get; }
        }

        private class InMemoryMessageContext : IInMemoryMessageContext
        {
            public InMemoryMessageContext(MessageContext messageContext)
            {
                this.Context = messageContext ?? throw new ArgumentNullException(nameof(messageContext));
            }

            public virtual MessageContext Context { get; }
        }

        private class InMemoryDeferredMessageContext : IComparable<InMemoryDeferredMessageContext>, IInMemoryMessageContext
        {
            private readonly DateTime deferredUntil;

            public InMemoryDeferredMessageContext(MessageContext messageContext, DateTime deferredUntil)
            {
                this.Context = messageContext;
                this.deferredUntil = deferredUntil;
            }

            public int CompareTo(InMemoryDeferredMessageContext? other)
            {
                return this.deferredUntil.CompareTo(deferredUntil);
            }

            public bool IsReadyToEnqueue => this.deferredUntil <= DateTime.UtcNow;

            public MessageContext? Context { get; }
        }

    }
}