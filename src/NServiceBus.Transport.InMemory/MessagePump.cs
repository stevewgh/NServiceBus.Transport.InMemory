namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class MessagePump : IPushMessages
    {
        private Func<MessageContext, Task>? OnMessage { get; set; }
        private Func<ErrorContext, Task<ErrorHandleResult>>? OnError { get; set; }
        private CriticalError? Error { get; set; }
        private PushSettings? Settings { get; set; }

        private readonly string endpointName;
        private CancellationTokenSource? cancellationTokenSource;
        private Task? receiveLoopTask;

        public MessagePump(string endpointName)
        {
            Guard.AgainstNullAndEmpty(nameof(endpointName), endpointName);
            this.endpointName = endpointName;
        }

        public Task Init(Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CriticalError criticalError, PushSettings settings)
        {
            this.OnMessage = onMessage;
            this.OnError = onError;
            this.Error = criticalError;
            this.Settings = settings;

            this.cancellationTokenSource = new CancellationTokenSource();

            return Task.CompletedTask;
        }

        public void Start(PushRuntimeSettings limitations)
        {
            receiveLoopTask = Task.Run(ReceiveLoop);
        }

        private async Task ReceiveLoop()
        {
            if (this.cancellationTokenSource == null)
            {
                throw new Exception($"{nameof(cancellationTokenSource)} should be set in the Init method");
            }

            if (this.Settings == null)
            {
                throw new Exception($"{nameof(Settings)} should be set in the Init method");
            }

            if (this.OnMessage == null)
            {
                throw new Exception($"{nameof(OnMessage)} should be set in the Init method");
            }

            MessageContext? message = null;
            try
            {
                while (!this.cancellationTokenSource.IsCancellationRequested)
                {
                    message = SharedStorage.Instance.Query(this.endpointName);

                    if (message != null) await this.OnMessage(message);

                    await Task.Delay(TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException) when (this.cancellationTokenSource.IsCancellationRequested)
            {
            }
            catch (Exception exception)
            {
                try
                {
                    await this.OnError!(new ErrorContext(exception, message?.Headers, message?.MessageId, message?.Body, null, 1));
                }
                catch (Exception innerException)
                {
                    this.Error!.Raise("Failed to execute recoverability policy.", innerException);
                }
            }
        }

        public async Task Stop()
        {
            this.cancellationTokenSource?.Cancel();
            if (this.receiveLoopTask != null) await this.receiveLoopTask;
        }
    }
}
