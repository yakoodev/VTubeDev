using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace app.Transport
{
    public class PipeCommandTransport
    {
        private readonly ILogger<PipeCommandTransport> _logger;
        private readonly object _sync = new();
        private NamedPipeServerStream? _stream;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public PipeCommandTransport(ILogger<PipeCommandTransport> logger)
        {
            _logger = logger;
        }

        public bool IsConnected => _stream?.IsConnected == true;

        public void Attach(NamedPipeServerStream stream)
        {
            lock (_sync)
            {
                _stream = stream;
                _reader = new StreamReader(stream, Encoding.UTF8, false, 4096, leaveOpen: true);
                _writer = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true)
                {
                    AutoFlush = true
                };
            }
        }

        public void Detach()
        {
            lock (_sync)
            {
                _reader?.Dispose();
                _writer?.Dispose();
                _stream?.Dispose();
                _reader = null;
                _writer = null;
                _stream = null;
            }
        }

        public async Task<CommandAck?> SendAsync(string json, CancellationToken cancellationToken)
        {
            StreamReader? reader;
            StreamWriter? writer;
            lock (_sync)
            {
                if (_stream == null || _writer == null || _reader == null || !_stream.IsConnected)
                {
                    return null;
                }

                reader = _reader;
                writer = _writer;
            }

            try
            {
                await writer.WriteLineAsync(json.AsMemory(), cancellationToken);
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
                var ackLine = await reader.ReadLineAsync(linked.Token);
                if (string.IsNullOrWhiteSpace(ackLine))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<CommandAck>(ackLine, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send command to pipe");
                return null;
            }
        }
    }

    public class CommandAck
    {
        public string Status { get; set; } = "accepted";
        public string RequestId { get; set; } = "";
        public string? Error { get; set; }
    }
}
