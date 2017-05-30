using AForge.Video.DirectShow;
using EmotionLib.Extensions;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib
{
    public static class CameraHelper
    {
        public static List<Camera> GetCameras()
        {
            FilterInfoCollection devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            List<Camera> cameras = devices?.ToCameraList();

            if(cameras == null)
            {
                cameras = new List<Camera>();
            }

            return cameras;
        }
    }
}
