using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using UnityEngine;

namespace VTube.Transport
{
    public class MmfFrameWriter : MonoBehaviour
    {
        public string FormatId = "cam_main_16x9";
        public string Profile = "release";
        public int Width = TransportConstants.DefaultWidth;
        public int Height = TransportConstants.DefaultHeight;
        public int TargetFps = TransportConstants.DefaultTargetFps;

        private MemoryMappedFile _mmf;
        private MemoryMappedViewAccessor _accessor;
        private EventWaitHandle _signal;
        private byte[] _frameBuffer;
        private byte[] _headerBuffer;
        private float _nextWriteTime;
        private long _frameIndex;

        private void Start()
        {
            var capacity = TransportConstants.HeaderSizeBytes + TransportConstants.MaxFrameBytes;
            _mmf = MemoryMappedFile.CreateOrOpen(TransportConstants.FramesMmfName, capacity, MemoryMappedFileAccess.ReadWrite);
            _accessor = _mmf.CreateViewAccessor(0, capacity, MemoryMappedFileAccess.Write);
            _signal = new EventWaitHandle(false, EventResetMode.AutoReset, TransportConstants.FramesSignalName);

            _frameBuffer = new byte[Width * Height * 4];
            _headerBuffer = new byte[TransportConstants.HeaderSizeBytes];
        }

        private void Update()
        {
            if (TargetFps > 0)
            {
                if (Time.unscaledTime < _nextWriteTime)
                {
                    return;
                }

                _nextWriteTime = Time.unscaledTime + 1f / TargetFps;
            }

            FillFrame(_frameBuffer, Width, Height, _frameIndex);
            WriteFrame(_frameBuffer, Width, Height);
            _frameIndex++;
        }

        private void FillFrame(byte[] buffer, int width, int height, long frameIndex)
        {
            var t = (byte)(frameIndex % 255);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var idx = (y * width + x) * 4;
                    buffer[idx] = (byte)(x % 256);
                    buffer[idx + 1] = (byte)(y % 256);
                    buffer[idx + 2] = t;
                    buffer[idx + 3] = 255;
                }
            }
        }

        private void WriteFrame(byte[] buffer, int width, int height)
        {
            var header = new FrameHeader
            {
                formatId = FormatId,
                profile = Profile,
                width = width,
                height = height,
                pixelFormat = "RGBA32",
                frameIndex = _frameIndex,
                timestampUtc = DateTime.UtcNow.ToString("O"),
                payloadLength = buffer.Length
            };

            var headerJson = JsonUtility.ToJson(header);
            var headerBytes = Encoding.UTF8.GetBytes(headerJson);
            if (headerBytes.Length >= TransportConstants.HeaderSizeBytes)
            {
                Debug.LogWarning("MMF header too large, skipping frame");
                return;
            }

            Array.Clear(_headerBuffer, 0, _headerBuffer.Length);
            Buffer.BlockCopy(headerBytes, 0, _headerBuffer, 0, headerBytes.Length);

            _accessor.WriteArray(0, _headerBuffer, 0, _headerBuffer.Length);
            _accessor.WriteArray(TransportConstants.HeaderSizeBytes, buffer, 0, buffer.Length);
            _signal.Set();
        }

        private void OnDestroy()
        {
            _accessor?.Dispose();
            _mmf?.Dispose();
            _signal?.Dispose();
        }
    }
}
