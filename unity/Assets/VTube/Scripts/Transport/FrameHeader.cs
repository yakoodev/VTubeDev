using System;

namespace VTube.Transport
{
    [Serializable]
    public class FrameHeader
    {
        public string formatId = "";
        public string profile = "";
        public int width;
        public int height;
        public string pixelFormat = "RGBA32";
        public long frameIndex;
        public string timestampUtc = "";
        public int payloadLength;
    }
}
