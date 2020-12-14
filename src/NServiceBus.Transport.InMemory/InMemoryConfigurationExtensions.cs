namespace NServiceBus.Transport.InMemory
{
    using System;
    using Configuration.AdvancedExtensibility;

    /// <summary>
    /// Adds extensions methods to <see cref="TransportExtensions{T}" /> for configuration purposes.
    /// </summary>
    public static class InMemoryConfigurationExtensions
    {
        internal const string PollingTimeKey = "NServiceBus.Transport.InMemory.PollingTime";

        internal static void PollingTimeIfNotSet(this TransportExtensions<InMemoryTransport> config, TimeSpan pollingTimeSpan)
        {
            Guard.AgainstNull(nameof(config), config);

            if (!config.GetSettings().HasSetting(PollingTimeKey))
            {
                PollingTime(config, pollingTimeSpan);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="pollingTimeSpan"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void PollingTime(this TransportExtensions<InMemoryTransport> config, TimeSpan pollingTimeSpan)
        {
            Guard.AgainstNull(nameof(config), config);

            if (pollingTimeSpan.TotalMilliseconds <= 0 || pollingTimeSpan.TotalSeconds > 60)
            {
                throw new ArgumentOutOfRangeException(nameof(pollingTimeSpan), $"{pollingTimeSpan} must be greater than 0 but less than 60 seconds.");
            }

            config.GetSettings().Set(PollingTimeKey, pollingTimeSpan);
        }
    }
}
