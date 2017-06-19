using Affdex;
using AForge.Video.DirectShow;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmotionLib.Extensions;
using System.Diagnostics;

namespace EmotionLib
{
    public class EmotionDetector : IDisposable, ImageListener, ProcessStatusListener, FaceListener
    {
        #region singleton

        private static object _instanceLock = new object();
        private static EmotionDetector _instance;
        public static EmotionDetector Instance
        {
            get
            {
                lock(_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new EmotionDetector();
                    }
                }

                return _instance;
            }
        }

        private EmotionDetector() { }

        #endregion

        #region Properties

        private object _isInitializedLock = new object();
        private bool _isInitialized = false;
        private bool Initialized
        {
            get
            {
                lock (_isInitializedLock)
                {
                    return _isInitialized;
                }
            }
            set
            {
                lock (_isInitializedLock)
                {
                    _isInitialized = value;
                }
            }
        }

        private object _isRunningLock = new object();
        private bool _isRunning = false;
        public bool IsRunning
        {
            get
            {
                lock(_isRunningLock)
                {
                    return _isRunning;
                }
            }
            set
            {
                lock(_isRunningLock)
                {
                    _isRunning = value;
                }
            }
        }

        private FrameDetector _detector;
        private VideoCaptureDevice _videoCaptureDevice;

        private object _cameraLock = new object();
        private Camera _camera;
        public Camera Camera
        {
            get
            {
                lock(_cameraLock)
                {
                    return _camera;
                }
            }
            set
            {
                lock(_cameraLock)
                {
                    if(IsRunning)
                    {
                        throw new Exception("Can't set Camera while in use, first call Stop()");
                    }
                    _camera = value;
                }
            }
        }

        private object _emotionLock = new object();
        private EmotionEnum _emotion;
        public EmotionEnum Emotion
        {
            get
            {
                lock(_emotionLock)
                {
                    return _emotion;
                }
            }
            set
            {
                lock(_emotionLock)
                {
                    SendEvent(_emotion, value);
                    _emotion = value;
                }
            }
        }

        public delegate void NewEmotionDetected(Classes.NewEmotionDetectedEventArgs args);
        private event NewEmotionDetected _newEmotionDetectedEvent;
        public event NewEmotionDetected NewEmotionDetectedEvent
        {
            add
            {
                _newEmotionDetectedEvent += value;
            }
            remove
            {
                _newEmotionDetectedEvent -= value;
            }
        }

        public delegate void NewFrame(Classes.NewFrameEventArgs args);
        private event NewFrame _newFrameEvent;
        public event NewFrame NewFrameEvent
        {
            add
            {
                _newFrameEvent += value;
            }
            remove
            {
                _newFrameEvent -= value;
            }
        }

        #endregion

        #region Start/Stop Methods

        public async Task StartAsync()
        {
            if (IsRunning)
            {
                throw new Exception("Already Started");
            } 

            if (Camera == null)
            {
                throw new Exception("No Camera Selected");
            }
            IsRunning = true;

            try
            {
                _timestamp = 0f;
                _frameCount = 0;
                await InitDetector();
                await InitVideoCaptureDevice(Camera);
            }
            catch(Exception ex)
            {
                IsRunning = false;
                throw new Exception("Could not start",ex);               
            }

        }

        public async Task StopAsync()
        {
            CheckInitalized();

            if(!IsRunning)
            {
                throw new Exception("Not started");
            }

            IsRunning = false;

            try
            {
                await Task.Run(() =>
                {
                    _detector.stop();
                    _videoCaptureDevice.SignalToStop();
                    _videoCaptureDevice.WaitForStop();
                });
                
            }
            catch (Exception ex)
            {
                IsRunning = true;
                throw new Exception("Could not stop", ex);
            }

        }

        public async void Dispose()
        {
            if (IsRunning)
            {
                await StopAsync();
            }
        }

        private async Task InitVideoCaptureDevice(Camera camera)
        {
            await Task.Run(() =>
            {
                _videoCaptureDevice = new VideoCaptureDevice(camera.MonikerString);
                _videoCaptureDevice.NewFrame += _videoCaptureDevice_NewFrame;
                _videoCaptureDevice.Start();
            });
        }

        private async Task InitDetector()
        {
            if (!Initialized)
            {
                await Task.Run(() =>
                {
                    _detector = new FrameDetector(5);
                    _detector.setClassifierPath(GetClasifierPath());
                    _detector.setDetectAllEmotions(true);
                    _detector.setImageListener(this);
                    _detector.setProcessStatusListener(this);
                    _detector.setFaceListener(this);

                    _detector.start();
                    Initialized = true;
                });
            }
        }

        private async Task CloseVideoCaptureDevice()
        {
            await Task.Run(() =>
            {
                _videoCaptureDevice.NewFrame -= _videoCaptureDevice_NewFrame;
                _videoCaptureDevice.SignalToStop();
                _videoCaptureDevice.WaitForStop();
                _videoCaptureDevice = null;
            });
        }

        private void CheckInitalized()
        {
            if (!Initialized)
            {
                throw new Exception("EmotionDetector is not Initialized, call Start() first");
            }
        }

        private string GetClasifierPath()
        {
            string dir = Path.GetDirectoryName(typeof(EmotionDetector).Assembly.Location);
            return $"{new Uri(dir).LocalPath}//Classifier";
        }

        #endregion

        #region ImageListener

        private int[] _frameEmotions = new int[7];
        private int _frameCount = 0;
        public void onImageResults(Dictionary<int, Face> faces, Frame frame)
        {
            if(faces != null & faces.Count > 0)
            {
                EmotionEnum emotion = EmotionEnum.Neutral;

                //faces becomes, for a strange reason, null between the check and the next call, so catch the error...
                try
                {
                    int faceID = GetFaceId();

                    emotion = GetEmotionValue(faces, faceID);
                }
                catch{}

                _frameEmotions[(int)emotion]++;
                _frameCount++;

                if (_frameCount == 10)
                {
                    CalculateEmotion();
                }
            }

        }

        public void onImageCapture(Frame frame)
        {

        }

        private int GetFaceId()
        {
            int face = -1;

            lock (_faceIdsLock)
            {
                if (_faceIds.Count > 0)
                {
                    face = _faceIds.First();
                }
            }

            return face;
        }

        private EmotionEnum GetEmotionValue(Dictionary<int, Face> faces, int face)
        {
            EmotionEnum val = EmotionEnum.Neutral;

            if (face != -1)
            {
                val = GetFrameValue(faces[face].Emotions);

                if (Debugger.IsAttached)
                {
                    WriteValues(faces[face].Emotions);
                }

            }

            return val;
        }

        private void CalculateEmotion()
        { 
            int maxVal = _frameEmotions.Max();

            Emotion =  (EmotionEnum)_frameEmotions.ToList().IndexOf(maxVal);

            Array.Clear(_frameEmotions, 0, _frameEmotions.Length);

            _frameCount = 0;
        }

        private void WriteValues(Emotions emotions)
        {
            Console.WriteLine($"Anger: {emotions.Anger} | Fear: {emotions.Fear} | Happy: {emotions.Joy} | Sad: {emotions.Sadness} | Suprise: {emotions.Surprise} | Disgust: {emotions.Disgust} |");
        }

        #endregion

        #region Processlistener

        public async void onProcessingException(AffdexException ex)
        {
            await StopAsync();
            throw new Exception("Processing is stopped", ex);
        }

        public void onProcessingFinished()
        {

        }

        #endregion

        #region FaceListener

        private int _faceCount = 0;
        private object _faceIdsLock = new object();
        private List<int> _faceIds = new List<int>();
        public void onFaceFound(float timestamp, int faceId)
        {
            lock(_faceIdsLock)
            {
                _faceIds.Add(faceId);
                _faceCount++;
            }
        }

        public void onFaceLost(float timestamp, int faceId)
        {
            lock(_faceIdsLock)
            {
                _faceIds.Remove(faceId);
                _faceCount--;

                if(_faceCount == 0)
                {
                    Emotion = EmotionEnum.None;
                }
            }
        }

        #endregion

        private EmotionEnum GetFrameValue(Emotions emotions)
        {
            EmotionEnum val = EmotionEnum.Neutral;
            float exactVal = 0f;

            if (emotions.Anger > exactVal && emotions.Anger > 5f)
            {
                val = EmotionEnum.Anger;
                exactVal = emotions.Anger;
            }

            if (emotions.Fear > exactVal && emotions.Fear > 10f)
            {
                val = EmotionEnum.Fear;
                exactVal = emotions.Fear;
            }

            if (emotions.Joy > exactVal && emotions.Joy > 10f)
            {
                val = EmotionEnum.Happy;
                exactVal = emotions.Joy;
            }

            if (emotions.Sadness > exactVal && emotions.Sadness > 10f)
            {
                val = EmotionEnum.Sad;
                exactVal = emotions.Sadness;
            }

            if (emotions.Disgust > exactVal && emotions.Disgust > 10f)
            {
                val = EmotionEnum.Disgust;
                exactVal = emotions.Disgust;
            }

            if (emotions.Surprise > exactVal && emotions.Surprise > 10f)
            {
                val = EmotionEnum.Suprise;
                exactVal = emotions.Sadness;
            }

            return val;
        }

        private void SendEvent(EmotionEnum oldValue, EmotionEnum newValue)
        {
            if(oldValue != newValue)
            {
                _newEmotionDetectedEvent?.Invoke(new Classes.NewEmotionDetectedEventArgs(oldValue, newValue));
            }
        }

        private float _timestamp;
        private void _videoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Frame frame = eventArgs.Frame.ToFrame(_timestamp);
            _newFrameEvent?.Invoke(new Classes.NewFrameEventArgs(eventArgs.Frame));
            _detector.process(frame);
            _timestamp += 1f;
        }
    }
}
