using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceWPF.Classes
{
    class ConversationItem
    {
        // Public variables
        public enum Sender { Bot, User };

        // Private variables
        private Sender sender;
        private string content;

        public void setSender(Sender sender)
        {

        }

        public Sender getSender()
        {
            return Sender.Bot;
        }
    }
}
