using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/streams")]
    public class StreamsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                streams = Array.Empty<object>()
            };

            return Ok(result);
        }
    }
}
