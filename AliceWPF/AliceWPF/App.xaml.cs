using EmotionLib;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AliceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StartEmotionDector();
            Exit += App_Exit;
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            Exit -= App_Exit;
            await EmotionDetector.Instance.StopAsync();
        }

        private async void StartEmotionDector()
        {
            List<Camera> cameras = CameraHelper.GetCameras();
            EmotionDetector.Instance.Camera = cameras[0];
            await EmotionDetector.Instance.StartAsync();
        }
    }
}
