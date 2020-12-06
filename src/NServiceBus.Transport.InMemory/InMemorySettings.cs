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
        }

        public string EndpointName { get; }
    }
}
