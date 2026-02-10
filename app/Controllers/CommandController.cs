using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("cmd")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }

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

            var context = NormalizeContext(request);
            _logger.LogInformation(
                "SceneCommand enqueued requestId={RequestId} source={Source} type={Type}",
                context.RequestId,
                context.Source,
                request.Command.Type);

            var response = new CommandResponse
            {
                Status = "accepted",
                RequestId = context.RequestId
            };

            return Accepted(response);
        }

        private static RequestContext NormalizeContext(SceneCommandRequest request)
        {
            var requestId = !string.IsNullOrWhiteSpace(request.Context?.RequestId)
                ? request.Context.RequestId!
                : request.RequestId;
            if (string.IsNullOrWhiteSpace(requestId))
            {
                requestId = $"req_{Guid.NewGuid():N}";
            }

            var source = !string.IsNullOrWhiteSpace(request.Context?.Source)
                ? request.Context.Source!
                : request.Source;
            if (string.IsNullOrWhiteSpace(source))
            {
                source = "web";
            }

            var timestampUtc = !string.IsNullOrWhiteSpace(request.Context?.TimestampUtc)
                ? request.Context.TimestampUtc!
                : request.TimestampUtc;
            if (string.IsNullOrWhiteSpace(timestampUtc))
            {
                timestampUtc = DateTimeOffset.UtcNow.ToString("O");
            }

            request.Context = new RequestContext
            {
                RequestId = requestId,
                Source = source,
                TimestampUtc = timestampUtc
            };
            if (request.Command != null)
            {
                request.Command.Context = request.Context;
            }

            return request.Context;
        }
    }
}
