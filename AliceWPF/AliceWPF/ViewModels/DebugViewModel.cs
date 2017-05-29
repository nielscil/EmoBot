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

namespace AliceWPF.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
        public DebugViewModel()
        {
            EmotionDetector.Instance.NewFrameEvent += Instance_NewFrameEvent;
        }

        private DebugView View
        {
            get
            {
                return GetView() as DebugView;
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

        private UserEmotion _selectedEmotion;
        public UserEmotion SelectedEmotion
        {
            get
            {
                return _selectedEmotion;
            }
            set
            {
                _selectedEmotion = value;
                NotifyOfPropertyChange("EmotionFeedback");
                NotifyOfPropertyChange();
            }
        }

        public string EmotionFeedback
        {
            get
            {
                return "User is feeling " + SelectedEmotion.ToString() ;
            }            
        }
    }
}
