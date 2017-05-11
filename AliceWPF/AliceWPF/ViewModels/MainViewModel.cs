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
        private string _input = string.Empty;
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
                NotifyOfPropertyChange("Loading"); //TODO: Remove this!
            }
        }

        private bool _loading = false;
        public bool Loading
        {
            get
            {
                return Input.Length > 0; //TODO: Remove this!
                return _loading;
            }
            set
            {
                _loading = value;
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
            //TODO: Remove this
            Conversation.Add(new ConversationItem(SenderEnum.Bot, "I'm a bot"));
            Conversation.Add(new ConversationItem(SenderEnum.User, "I'm not a bot"));
        }
    }
}
