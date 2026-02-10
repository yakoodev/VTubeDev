using Microsoft.AspNetCore.Mvc;
using app.Transport;

namespace app.Controllers
{
    [ApiController]
    [Route("stream")]
    public class StreamController : ControllerBase
    {
        private readonly FrameStore _store;

        public StreamController(FrameStore store)
        {
            _store = store;
        }

        [HttpGet("{formatId}/{profile}.mjpg")]
        public async Task Get(string formatId, string profile, CancellationToken cancellationToken)
        {
            var frame = _store.GetLatest();
            if (frame == null)
            {
                Response.StatusCode = 503;
                await Response.WriteAsync("No frame available", cancellationToken);
                return;
            }

            if (!string.Equals(frame.Header.FormatId, formatId, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(frame.Header.Profile, profile, StringComparison.OrdinalIgnoreCase))
            {
                Response.StatusCode = 503;
                await Response.WriteAsync("No frame available", cancellationToken);
                return;
            }

            Response.ContentType = "multipart/x-mixed-replace; boundary=frame";
            Response.Headers.CacheControl = "no-store";

            var lastVersion = -1L;
            while (!cancellationToken.IsCancellationRequested)
            {
                var version = _store.Version;
                if (version == lastVersion)
                {
                    await Task.Delay(50, cancellationToken);
                    continue;
                }

                lastVersion = version;
                var latest = _store.GetLatest();
                if (latest == null)
                {
                    await Task.Delay(100, cancellationToken);
                    continue;
                }

                if (!string.Equals(latest.Header.FormatId, formatId, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(latest.Header.Profile, profile, StringComparison.OrdinalIgnoreCase))
                {
                    await Task.Delay(100, cancellationToken);
                    continue;
                }

                var jpeg = FrameEncoder.EncodeJpeg(latest);
                await WriteFrameAsync(jpeg, cancellationToken);
            }
        }

        private async Task WriteFrameAsync(byte[] jpeg, CancellationToken cancellationToken)
        {
            await Response.WriteAsync("--frame\r\n", cancellationToken);
            await Response.WriteAsync("Content-Type: image/jpeg\r\n", cancellationToken);
            await Response.WriteAsync($"Content-Length: {jpeg.Length}\r\n\r\n", cancellationToken);
            await Response.Body.WriteAsync(jpeg, cancellationToken);
            await Response.WriteAsync("\r\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}
