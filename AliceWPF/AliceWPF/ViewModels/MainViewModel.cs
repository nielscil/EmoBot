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
    class MainViewModel : ViewModelBase
    {

        public DebugViewModel DebugViewModel { get; private set; }

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
            }
        }

        private bool _loading = false;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _debug = false;
        public bool Debug {
            get
            {
                return _debug;
            }
            set
            {
                _debug = value;
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
            DebugViewModel = new DebugViewModel();
        }

        public async Task SendContent()
        {
            if (!string.IsNullOrWhiteSpace(Input))
            {
                string input = Input;
                Input = string.Empty;
                Conversation.Add(new ConversationItem(SenderEnum.User, input));
                Loading = true;
                string botResponse = await Alice.InputController.Instance.GetResponse(input);
                Conversation.Add(new ConversationItem(SenderEnum.Bot, botResponse));
                Loading = false;
            }
        }
    }
}
