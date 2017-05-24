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

namespace EmotionLib
{
    public class EmotionDetector
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

        private object _camerLock = new object();
        private Camera _camera;
        public Camera Camera
        {
            get
            {
                lock(_camerLock)
                {
                    return _camera;
                }
            }
            set
            {
                lock(_camerLock)
                {
                    if(IsRunning)
                    {
                        throw new Exception("Can't set Camera while in use, first call Stop()");
                    }
                    _camera = value;
                }
            }
        }

        public async Task InitAsync(Camera camera)
        {

            try
            {
                await InitDetector();
                await InitVideoCaptureDevice(camera); 
                
                Initialized = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not complete initialization", ex);
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
            if(!Initialized)
            {
                await Task.Run(() =>
                {
                    _detector = new FrameDetector(5);
                    _detector.setClassifierPath(GetClasifierPath());
                    _detector.setDetectAllEmotions(true);
                    //TODO: add listeners

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

        private void _videoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Frame frame = eventArgs.Frame.ToFrame();
            _detector.process(frame);
        }

        public async Task StartAsync()
        {
            if(IsRunning)
            {
                throw new Exception("Already Started");
            }

            if(Camera == null)
            {
                throw new Exception("No Camera Selected");
            }
            
            try
            {
                await InitDetector();
                await InitVideoCaptureDevice(Camera);

                IsRunning = true;

            }
            catch(Exception ex)
            {
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

            try
            {
                await Task.Run(() =>
                {
                    _detector.stop();
                    _videoCaptureDevice.SignalToStop();
                    _videoCaptureDevice.WaitForStop();
                });
                IsRunning = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not start", ex);
            }

        }

        private void CheckInitalized()
        {
            if(!Initialized)
            {
                throw new Exception("EmotionDetector is not Initialized, call Init() first");
            }
        }

        private string GetClasifierPath()
        {
            string dir = Path.GetDirectoryName(typeof(EmotionDetector).Assembly.Location);
            return $"{new Uri(dir).LocalPath}//Classifier";
        }

    }
}
