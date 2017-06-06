using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib.Models
{
    public sealed class Camera
    {
        public string Name { get; private set; }
        public string MonikerString { get; private set; }

        internal Camera(string name, string monikerString)
        {
            Name = name;
            MonikerString = monikerString;
        }
    }
}
