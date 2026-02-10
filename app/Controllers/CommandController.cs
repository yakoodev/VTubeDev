using app.Models;
using app.Transport;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace app.Controllers
{
    [ApiController]
    [Route("cmd")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        private readonly PipeCommandTransport _transport;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public CommandController(ILogger<CommandController> logger, PipeCommandTransport transport)
        {
            _logger = logger;
            _transport = transport;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SceneCommandRequest request, CancellationToken cancellationToken)
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

            var payloadJson = JsonSerializer.Serialize(request, _jsonOptions);
            var ack = await _transport.SendAsync(payloadJson, cancellationToken);
            if (ack == null)
            {
                return StatusCode(503, "Unity pipe is not connected");
            }

            if (!string.Equals(ack.Status, "accepted", StringComparison.OrdinalIgnoreCase))
            {
                var errorResponse = new CommandResponse
                {
                    Status = "rejected",
                    RequestId = ack.RequestId,
                    Error = ack.Error ?? "Rejected by Unity"
                };
                return BadRequest(errorResponse);
            }

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
