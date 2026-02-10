using System.Drawing;
using System.Drawing.Imaging;

namespace app.Transport
{
    public static class FrameEncoder
    {
        public static byte[] EncodePng(FrameData frame)
        {
            using var bitmap = CreateBitmap(frame);
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }

        public static byte[] EncodeJpeg(FrameData frame, long quality = 80)
        {
            using var bitmap = CreateBitmap(frame);
            using var stream = new MemoryStream();

            var encoder = GetEncoder(ImageFormat.Jpeg);
            if (encoder == null)
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            bitmap.Save(stream, encoder, encoderParams);
            return stream.ToArray();
        }

        private static Bitmap CreateBitmap(FrameData frame)
        {
            var width = frame.Header.Width;
            var height = frame.Header.Height;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                var converted = ConvertRgbaToBgra(frame.RgbaBytes);
                System.Runtime.InteropServices.Marshal.Copy(converted, 0, data.Scan0, converted.Length);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }

        private static byte[] ConvertRgbaToBgra(byte[] rgba)
        {
            var converted = new byte[rgba.Length];
            for (var i = 0; i < rgba.Length; i += 4)
            {
                converted[i] = rgba[i + 2];
                converted[i + 1] = rgba[i + 1];
                converted[i + 2] = rgba[i];
                converted[i + 3] = rgba[i + 3];
            }
            return converted;
        }

        private static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
