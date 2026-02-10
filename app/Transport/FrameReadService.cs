using System.IO.MemoryMappedFiles;

namespace app.Transport
{
    public class FrameReadService : BackgroundService
    {
        private readonly ILogger<FrameReadService> _logger;
        private readonly FrameMmfReader _reader;
        private readonly FrameStore _store;

        public FrameReadService(ILogger<FrameReadService> logger, FrameMmfReader reader, FrameStore store)
        {
            _logger = logger;
            _reader = reader;
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                EventWaitHandle? signal = null;
                try
                {
                    signal = EventWaitHandle.OpenExisting(TransportConstants.FramesSignalName);
                }
                catch (WaitHandleCannotBeOpenedException)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    continue;
                }

                using (signal)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        if (!signal.WaitOne(TimeSpan.FromSeconds(1)))
                        {
                            continue;
                        }

                        var frame = _reader.TryReadLatest();
                        if (frame != null)
                        {
                            _store.Update(frame);
                        }
                    }
                }
            }
        }
    }
}
