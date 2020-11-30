namespace NServiceBus.Transport.InMemory
{
    using System.Threading.Tasks;

    internal class QueueCreator : ICreateQueues
    {
        private readonly string endpointName;

        public QueueCreator(string endpointName)
        {
            Guard.AgainstNullAndEmpty(nameof(endpointName), endpointName);
            this.endpointName = endpointName;
        }

        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            return Task.CompletedTask;
        }
    }
}
