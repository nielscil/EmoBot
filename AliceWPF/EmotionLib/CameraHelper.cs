using AForge.Video.DirectShow;
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

            List<Camera> cameras = new List<Camera>();

            foreach(FilterInfo info in devices)
            {
                cameras.Add(FilterInfoToCamera(info));
            }

            return cameras;
        }

        private static Camera FilterInfoToCamera(FilterInfo info)
        {
            return new Camera(info.Name, info.MonikerString);
        }
    }
}
