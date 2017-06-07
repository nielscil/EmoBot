using EmotionLib;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Alice;
using System.IO;

namespace AliceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ChatBot.Init(this,true);

            string codeBasePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string path = new Uri(codeBasePath).LocalPath + "\\Resources\\test.json";
            ChatBot.LoadFacts(path);

            StartEmotionDector();
            Exit += App_Exit;
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            Exit -= App_Exit;

            if(EmotionDetector.Instance.IsRunning)
            {
                await EmotionDetector.Instance.StopAsync();
            }
        }

        private void StartEmotionDector()
        {
            List<Camera> cameras = CameraHelper.GetCameras();
            EmotionDetector.Instance.Camera = cameras[0];
        }
    }
}
