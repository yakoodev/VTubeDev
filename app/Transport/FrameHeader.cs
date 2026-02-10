using System.Text.Json.Serialization;

namespace app.Transport
{
    public class FrameHeader
    {
        [JsonPropertyName("formatId")]
        public string FormatId { get; set; } = "";

        [JsonPropertyName("profile")]
        public string Profile { get; set; } = "";

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("pixelFormat")]
        public string PixelFormat { get; set; } = "RGBA32";

        [JsonPropertyName("frameIndex")]
        public long FrameIndex { get; set; }

        [JsonPropertyName("timestampUtc")]
        public string TimestampUtc { get; set; } = "";

        [JsonPropertyName("payloadLength")]
        public int PayloadLength { get; set; }
    }
}
