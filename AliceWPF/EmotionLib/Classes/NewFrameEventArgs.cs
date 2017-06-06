using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib.Classes
{
    public sealed class NewFrameEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        internal NewFrameEventArgs(Bitmap frame)
        {
            Frame = frame;
        }
    }
}
