namespace app.Models
{
    public class RequestContext
    {
        public string RequestId { get; set; } = "";
        public string Source { get; set; } = "";
        public string TimestampUtc { get; set; } = "";
    }

    public class SceneCommandRequest
    {
        public string? RequestId { get; set; }
        public string? Source { get; set; }
        public string? TimestampUtc { get; set; }
        public RequestContext? Context { get; set; }
        public SceneCommand? Command { get; set; }
    }

    public class SceneCommand
    {
        public string? Type { get; set; }
        public object? Payload { get; set; }
        public RequestContext? Context { get; set; }
    }

    public class CommandResponse
    {
        public string Status { get; set; } = "accepted";
        public string RequestId { get; set; } = "";
        public string? Error { get; set; }
    }
}
