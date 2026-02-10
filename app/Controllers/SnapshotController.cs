using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("snapshot")]
    public class SnapshotController : ControllerBase
    {
        [HttpGet("{formatId}/{profile}.png")]
        public IActionResult Get(string formatId, string profile)
        {
            return StatusCode(503, "No frame available");
        }
    }
}
