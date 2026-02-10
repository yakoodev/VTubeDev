using System.IO.Pipes;

namespace app.Transport
{
    public class PipeCommandServer : BackgroundService
    {
        private readonly ILogger<PipeCommandServer> _logger;
        private readonly PipeCommandTransport _transport;

        public PipeCommandServer(ILogger<PipeCommandServer> logger, PipeCommandTransport transport)
        {
            _logger = logger;
            _transport = transport;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var server = new NamedPipeServerStream(
                    TransportConstants.CommandPipeName,
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Message,
                    PipeOptions.Asynchronous);

                try
                {
                    _logger.LogInformation("Waiting for Unity pipe connection: {Pipe}", TransportConstants.CommandPipeName);
                    await server.WaitForConnectionAsync(stoppingToken);
                    _logger.LogInformation("Unity pipe connected");

                    _transport.Attach(server);

                    while (server.IsConnected && !stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(250), stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Pipe server error");
                }
                finally
                {
                    _transport.Detach();
                    server.Dispose();
                }

                if (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
    }
}
