using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("cmd")]
    public class CommandController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SceneCommandRequest request)
        {
            if (request.Command == null || string.IsNullOrWhiteSpace(request.Command.Type))
            {
                var errorResponse = new CommandResponse
                {
                    Status = "rejected",
                    RequestId = request.RequestId ?? "",
                    Error = "Validation failed: command.type is required"
                };
                return BadRequest(errorResponse);
            }

            var requestId = string.IsNullOrWhiteSpace(request.RequestId)
                ? $"req_{Guid.NewGuid():N}"
                : request.RequestId;

            var response = new CommandResponse
            {
                Status = "accepted",
                RequestId = requestId
            };

            return Accepted(response);
        }
    }
}
