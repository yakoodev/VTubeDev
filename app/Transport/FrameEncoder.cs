using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace app.Transport
{
    public static class FrameEncoder
    {
        public static byte[] EncodePng(FrameData frame)
        {
            using var image = CreateImage(frame);
            using var stream = new MemoryStream();
            image.Save(stream, new PngEncoder());
            return stream.ToArray();
        }

        public static byte[] EncodeJpeg(FrameData frame, long quality = 80)
        {
            using var image = CreateImage(frame);
            using var stream = new MemoryStream();
            var encoder = new JpegEncoder
            {
                Quality = (int)Math.Clamp(quality, 1, 100)
            };
            image.Save(stream, encoder);
            return stream.ToArray();
        }

        private static Image<Rgba32> CreateImage(FrameData frame)
        {
            var width = frame.Header.Width;
            var height = frame.Header.Height;
            var pixelCount = width * height;
            var pixels = new Rgba32[pixelCount];
            var rgba = frame.RgbaBytes;
            var offset = 0;
            for (var i = 0; i < pixelCount; i++)
            {
                pixels[i] = new Rgba32(rgba[offset], rgba[offset + 1], rgba[offset + 2], rgba[offset + 3]);
                offset += 4;
            }

            return Image.LoadPixelData<Rgba32>(pixels, width, height);
        }
    }
}
