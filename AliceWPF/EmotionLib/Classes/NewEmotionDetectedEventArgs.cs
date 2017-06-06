using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib.Classes
{
    public sealed class NewEmotionDetectedEventArgs : EventArgs
    {
        public EmotionEnum OldValue { get; private set; }
        public EmotionEnum NewValue { get; private set; }

        internal NewEmotionDetectedEventArgs(EmotionEnum oldValue, EmotionEnum newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
