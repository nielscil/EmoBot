using AliceWPF.Classes;
using AliceWPF.Converters;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceWPF.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
        public DebugViewModel()
        {
            
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
            }
        }

        private string _emotionFeedback;
        public string EmotionFeedback
        {
            get
            {
                return _emotionFeedback;
            }
            set
            {
                _emotionFeedback = "User is feeling " + value;
                NotifyOfPropertyChange();
            }
        }
    }
}
