using AliceWPF.Classes;
using System.Drawing;
using AliceWPF.Converters;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmotionLib;
using EmotionLib.Classes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Drawing.Imaging;
using AliceWPF.Views;
using System.Windows;
using EmotionLib.Models;
using System.Diagnostics;

namespace AliceWPF.ViewModels
{
    public class DebugViewModel : ViewModelBase, IDisposable
    {
        public DebugViewModel()
        {
            EmotionDetector.Instance.NewFrameEvent += Instance_NewFrameEvent;
            EmotionDetector.Instance.NewEmotionDetectedEvent += Instance_NewEmotionDetectedEvent;
        }

        private DebugView View
        {
            get
            {
                return GetView() as DebugView;
            }
        }

        private EmotionEnum _selectedEmotion;
        public EmotionEnum SelectedEmotion
        {
            get
            {
                return _selectedEmotion;
            }
            set
            {
                _selectedEmotion = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("EmotionFeedback");
            }
        }

        private bool _useCamera;
        public bool UseCamera
        {
            get
            {
                return _useCamera;
            }
            set
            {
                _useCamera = value;
                NotifyOfPropertyChange();
                SetupCamera();
            }
        }

        public string EmotionFeedback
        {
            get
            {
                return "User is feeling " + SelectedEmotion.ToString() ;
            }            
        }

        private async void SetupCamera()
        {
            if (UseCamera)
            {
                await EmotionDetector.Instance.StartAsync();
            }
            else
            {
                await EmotionDetector.Instance.StopAsync();
            }
        }

        public void Dispose()
        {
            EmotionDetector.Instance.NewFrameEvent -= Instance_NewFrameEvent;
            EmotionDetector.Instance.NewEmotionDetectedEvent -= Instance_NewEmotionDetectedEvent;
        }

        private void Instance_NewEmotionDetectedEvent(NewEmotionDetectedEventArgs args)
        {
            //TODO: use oldvalue for something?
            SelectedEmotion = args.NewValue;
            if(Debugger.IsAttached)
            {
                Console.WriteLine($"Changed value to {SelectedEmotion}");
            }
        }

        private void Instance_NewFrameEvent(NewFrameEventArgs args)
        {
            BitmapImage bi;
            using (var bitmap = (Bitmap)args.Frame.Clone())
            {
                bi = bitmap.ToBitmapImage();
            }
            bi.Freeze();
            View.Dispatcher.BeginInvoke(new ThreadStart(delegate { View.CameraSource.Source = bi; }));
        }

    }
}
