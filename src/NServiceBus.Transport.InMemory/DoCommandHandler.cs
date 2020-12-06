namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Threading.Tasks;

    public class DoCommandHandler : IHandleMessages<DoWorkCommand>
    {
        public async Task Handle(DoWorkCommand message, IMessageHandlerContext context)
        {
            await context.Publish<WorkCompletedEvent>();
        }
    }

    public class WorkCompletedEventHandler : IHandleMessages<WorkCompletedEvent>
    {
        public Task Handle(WorkCompletedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine("Handled event");
            return Task.CompletedTask;
        }
    }
}
