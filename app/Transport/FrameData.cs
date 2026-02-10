namespace app.Transport
{
    public class FrameData
    {
        public FrameHeader Header { get; }
        public byte[] RgbaBytes { get; }

        public FrameData(FrameHeader header, byte[] rgbaBytes)
        {
            Header = header;
            RgbaBytes = rgbaBytes;
        }
    }
}
