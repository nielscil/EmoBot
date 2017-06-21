using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice;
using System.IO;

namespace AliceWPF.Classes
{
    public class ConversationItem
    {
        public SenderEnum Sender { get; private set; }
        public string Content { get; private set; }

        public ConversationItem(SenderEnum sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
