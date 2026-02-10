using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                status = "ok",
                build = "dev",
                timeUtc = DateTime.UtcNow.ToString("O")
            };

            return Ok(result);
        }
    }
}
