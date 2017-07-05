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
        public Emotion OldValue { get; private set; }
        public Emotion NewValue { get; private set; }

        internal NewEmotionDetectedEventArgs(Emotion oldValue, Emotion newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
