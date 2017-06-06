using Affdex;
using AForge.Video.DirectShow;
using EmotionLib.Models;
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
        internal static Frame ToFrame(this Bitmap bitmap,float timestamp)
        {
            byte[] data = bitmap.ToByteArray();
            return new Frame(bitmap.Width, bitmap.Height, data, Frame.COLOR_FORMAT.RGB,timestamp);
        }
    }

    internal static class FilterInfoExtensions
    {
        internal static Camera ToCamera(this FilterInfo info)
        {
            return new Camera(info.Name, info.MonikerString);
        }
    }

    internal static class FilterInfoCollectionExtensions
    {
        internal static List<Camera> ToCameraList(this FilterInfoCollection collection)
        {
            List<Camera> cameras = new List<Camera>();

            foreach (FilterInfo info in collection)
            {
                cameras.Add(info.ToCamera());
            }

            return cameras;
        }
    }
}
