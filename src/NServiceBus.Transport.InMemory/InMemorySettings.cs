namespace NServiceBus.Transport.InMemory
{
    using System;
    using Settings;

    internal class InMemorySettings
    {
        public InMemorySettings(ReadOnlySettings readOnlySettings)
        {
            readOnlySettings = readOnlySettings ?? throw new ArgumentNullException(nameof(readOnlySettings));
            EndpointName = readOnlySettings.EndpointName();
            PollingTime = readOnlySettings.Get<TimeSpan>(InMemoryConfigurationExtensions.PollingTimeKey);
        }

        public string EndpointName { get; }

        public TimeSpan PollingTime { get; }
    }
}
