namespace NServiceBus.Transport.InMemory
{
    using System.Threading.Tasks;
    using Extensibility;

    internal class MessageDispatcher : IDispatchMessages
    {
        private readonly InMemorySettings settings;

        public MessageDispatcher(InMemorySettings settings)
        {
            this.settings = settings;
        }

        public Task Dispatch(TransportOperations outgoingMessages, TransportTransaction transaction, ContextBag context)
        {
            foreach (var transportOperation in outgoingMessages.UnicastTransportOperations)
            {
                var destination = new InMemoryDestination(transportOperation.Destination);
                SharedStorage.Instance.Send(destination, transportOperation.Message, transportOperation.DeliveryConstraints);
            }

            foreach (var transportOperation in outgoingMessages.MulticastTransportOperations)
            {
                SharedStorage.Instance.Publish(transportOperation.Message, transportOperation.MessageType);
            }

            return Task.CompletedTask;
        }
    }
}
