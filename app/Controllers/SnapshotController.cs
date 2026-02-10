using Microsoft.AspNetCore.Mvc;
using app.Transport;

namespace app.Controllers
{
    [ApiController]
    [Route("snapshot")]
    public class SnapshotController : ControllerBase
    {
        private readonly FrameStore _store;

        public SnapshotController(FrameStore store)
        {
            _store = store;
        }

        [HttpGet("{formatId}/{profile}.png")]
        public IActionResult Get(string formatId, string profile)
        {
            var frame = _store.GetLatest();
            if (frame == null)
            {
                return StatusCode(503, "No frame available");
            }

            if (!string.Equals(frame.Header.FormatId, formatId, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(frame.Header.Profile, profile, StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(503, "No frame available");
            }

            var pngBytes = FrameEncoder.EncodePng(frame);
            Response.Headers.CacheControl = "no-store";
            return File(pngBytes, "image/png");
        }
    }
}
