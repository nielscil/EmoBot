using AliceWPF.Classes;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceWPF.ViewModels
{
    class MainViewModel : PropertyChangedBase
    {
        private string _input;
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
                NotifyOfPropertyChange();
            }
        }

        private ObservableCollection<ConversationItem> _converstation = new ObservableCollection<ConversationItem>();
        public ObservableCollection<ConversationItem> Conversation
        {
            get
            {
                return _converstation;
            }
            set
            {
                _converstation = value;
                NotifyOfPropertyChange();
            }
        }

        public MainViewModel()
        {
            Conversation.Add(new ConversationItem(SenderEnum.Bot, "I'm a bot"));
            Conversation.Add(new ConversationItem(SenderEnum.User, "I'm not a bot"));
        }
    }
}
