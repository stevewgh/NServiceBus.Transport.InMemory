namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DelayedDelivery;
    using Performance.TimeToBeReceived;
    using Routing;
    using Support;

    internal class InMemoryTransportInfrastructure : TransportInfrastructure
    {
        private readonly InMemorySettings settings;

        public InMemoryTransportInfrastructure(InMemorySettings settings)
        {
            Guard.AgainstNull(nameof(settings), settings);
            this.settings = settings;
        }

        public override TransportReceiveInfrastructure ConfigureReceiveInfrastructure()
        {
            return new TransportReceiveInfrastructure(
                () => new MessagePump(settings),
                () => new QueueCreator(settings.EndpointName),
                () => Task.FromResult(StartupCheckResult.Success));
        }

        public override TransportSendInfrastructure ConfigureSendInfrastructure()
        {
            return new TransportSendInfrastructure(
                () => new MessageDispatcher(this.settings), 
                () => Task.FromResult(StartupCheckResult.Success));
        }

        public override TransportSubscriptionInfrastructure ConfigureSubscriptionInfrastructure() 
            => new TransportSubscriptionInfrastructure(() => new InMemorySubscriptionManager(this.settings));

        public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance) => instance;

        public override string ToTransportAddress(LogicalAddress logicalAddress)
        {
            if (!logicalAddress.EndpointInstance.Properties.TryGetValue("queue", out var queue))
            {
                queue = "receiving";
            }
            
            return $"{queue}@{logicalAddress.EndpointInstance.Endpoint}";
        }

        public override IEnumerable<Type> DeliveryConstraints => new List<Type>
        {
            typeof(DelayDeliveryWith),
            typeof(DoNotDeliverBefore),
            typeof(DiscardIfNotReceivedBefore)
        };

        public override TransportTransactionMode TransactionMode { get; } = TransportTransactionMode.None;
        
        public override OutboundRoutingPolicy OutboundRoutingPolicy { get; } 
            = new OutboundRoutingPolicy(OutboundRoutingType.Unicast, OutboundRoutingType.Multicast, OutboundRoutingType.Unicast);
    }
}
