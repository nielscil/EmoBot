using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionLib
{
    public class EmotionDetector
    {
        #region singleton

        private static EmotionDetector _instance;
        public static EmotionDetector Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EmotionDetector();
                }
                return _instance;
            }
        }

        private EmotionDetector() { }

        #endregion

        private bool _isInitialized = false;

        public void Init()
        {
            if(_isInitialized)
            {
                throw new Exception("EmotionDetector is already Initialized");
            }

        }

        public void Start()
        {
            CheckInitalized();
        }

        public void Stop()
        {
            CheckInitalized();
        }

        private void CheckInitalized()
        {
            if(!_isInitialized)
            {
                throw new Exception("EmotionDetector is not Initialized, call Init() first");
            }
        }

    }
}
