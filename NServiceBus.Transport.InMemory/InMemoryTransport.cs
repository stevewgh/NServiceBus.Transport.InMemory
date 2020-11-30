namespace NServiceBus.Transport.InMemory
{
    using Routing;
    using Settings;

    public class InMemoryTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            return new InMemoryTransportInfrastructure(new InMemorySettings(settings));
        }

        public override string ExampleConnectionStringForErrorMessage { get; } = "There are no connection strings required.";

        public override bool RequiresConnectionString { get; } = false;
    }
}
