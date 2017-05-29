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

namespace AliceWPF.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
        public DebugViewModel()
        {
            EmotionDetector.Instance.NewFrameEvent += Instance_NewFrameEvent;
        }

        private void Instance_NewFrameEvent(NewFrameEventArgs args)
        {
            CameraSource = args.Frame;
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

        private Bitmap _cameraSource;
        public Bitmap CameraSource
        {
            get
            {
                return _cameraSource;
            }
            set
            {
                _cameraSource = value;
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
