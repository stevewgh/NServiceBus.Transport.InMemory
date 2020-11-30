namespace NServiceBus.Transport.InMemory
{
    using System.Threading.Tasks;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfig = new EndpointConfiguration("Program");

            var transport = endpointConfig.UseTransport<InMemoryTransport>();

            var endpoint = await Endpoint.Start(endpointConfig);

            await endpoint.SendLocal(new DoWorkCommand());

            await Task.Delay(-1);
        }
    }

    public class DoWorkCommand : ICommand
    {

    }

    public class WorkCompletedEvent : IEvent
    {

    }

}
