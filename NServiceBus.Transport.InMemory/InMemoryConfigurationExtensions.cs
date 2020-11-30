namespace NServiceBus.Transport.InMemory
{
    using Configuration.AdvancedExtensibility;

    /// <summary>
    /// Adds extensions methods to <see cref="TransportExtensions{T}" /> for configuration purposes.
    /// </summary>
    public static class InMemoryConfigurationExtensions
    {
        //public static void OverrideQueueName(this TransportExtensions<InMemoryTransport> config, string queue)
        //{
        //    Guard.AgainstNull(nameof(config), config);
        //    Guard.AgainstNull(nameof(queue), queue);
        //    config.GetSettings().Set("queue", queue);
        //}
    }
}
