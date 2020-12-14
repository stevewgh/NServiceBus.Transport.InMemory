namespace NServiceBus.Transport.InMemory
{
    using System;
    using Settings;

    /// <summary>
    /// An Transport which runs entirely in memory with no host machine requirements. Designed for testing use only, do not use this transport in Production.
    /// </summary>
    public class InMemoryTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            new TransportExtensions<InMemoryTransport>(settings).PollingTimeIfNotSet(TimeSpan.FromMilliseconds(250));

            return new InMemoryTransportInfrastructure(new InMemorySettings(settings));
        }

        public override string ExampleConnectionStringForErrorMessage { get; } = "There are no connection strings required.";

        public override bool RequiresConnectionString { get; } = false;
    }
}
