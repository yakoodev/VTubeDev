using System.IO.MemoryMappedFiles;
using System.Text;
using System.Text.Json;

namespace app.Transport
{
    public class FrameMmfReader
    {
        private readonly ILogger<FrameMmfReader> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public FrameMmfReader(ILogger<FrameMmfReader> logger)
        {
            _logger = logger;
        }

        public FrameData? TryReadLatest()
        {
            try
            {
                using var mmf = MemoryMappedFile.OpenExisting(TransportConstants.FramesMmfName);
                using var accessor = mmf.CreateViewAccessor(0, TransportConstants.HeaderSizeBytes + TransportConstants.MaxFrameBytes, MemoryMappedFileAccess.Read);

                var headerBytes = new byte[TransportConstants.HeaderSizeBytes];
                accessor.ReadArray(0, headerBytes, 0, headerBytes.Length);

                var headerJson = ExtractHeaderJson(headerBytes);
                if (string.IsNullOrWhiteSpace(headerJson))
                {
                    return null;
                }

                var header = JsonSerializer.Deserialize<FrameHeader>(headerJson, _jsonOptions);
                if (header == null || header.PayloadLength <= 0 || header.PayloadLength > TransportConstants.MaxFrameBytes)
                {
                    return null;
                }

                var payload = new byte[header.PayloadLength];
                accessor.ReadArray(TransportConstants.HeaderSizeBytes, payload, 0, payload.Length);

                return new FrameData(header, payload);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "MMF read failed");
                return null;
            }
        }

        private static string ExtractHeaderJson(byte[] headerBytes)
        {
            var zeroIndex = Array.IndexOf(headerBytes, (byte)0);
            var length = zeroIndex >= 0 ? zeroIndex : headerBytes.Length;
            return Encoding.UTF8.GetString(headerBytes, 0, length).Trim();
        }
    }
}
