using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("stream")]
    public class StreamController : ControllerBase
    {
        [HttpGet("{formatId}/{profile}.mjpg")]
        public IActionResult Get(string formatId, string profile)
        {
            return StatusCode(503, "No frame available");
        }
    }
}
