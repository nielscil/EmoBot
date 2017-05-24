using Affdex;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib.Extensions
{
    internal static class BitmapExtensions
    {
        internal static byte[] ToByteArray(this Bitmap bitmap, ImageFormat format)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return stream.ToArray();
            }
        }

        internal static byte[] ToByteArray(this Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(rect,ImageLockMode.ReadOnly,PixelFormat.Format24bppRgb);
            int length = bitmapData.Stride * bitmapData.Height;

            byte[] bytes = new byte[length];
            Marshal.Copy(bitmapData.Scan0, bytes, 0, length);
            bitmap.UnlockBits(bitmapData);

            return bytes;
        }
        internal static Frame ToFrame(this Bitmap bitmap)
        {
            byte[] data = bitmap.ToByteArray();
            return new Frame(bitmap.Width, bitmap.Height, data, Frame.COLOR_FORMAT.RGB);
        }
    }
}
