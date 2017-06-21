using Alice.Classes;
using EmotionLib;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
{
    public class EmotionTemplate : TemplateBase
    {
        private List<Response>[] _responses = new List<Response>[Enum.GetNames(typeof(Emotion)).Length];

        public EmotionTemplate(GlobalTemplateAction globalAction) : base(globalAction)
        {
            InitalizeResponses();
        }

        public EmotionTemplate() : this(null)
        {
        }

        public void AddResponse(Emotion emotion, Response response)
        {
            _responses[(int)emotion].Add(response);
        }

        public override string GetResponse(InputResponseData inputResponseData)
        {
            inputResponseData.GlobalActionResponse = _globalAction?.Invoke(inputResponseData);
            Response response = ResponseChooser.Choose(_responses[(int)EmotionDetector.Instance.Emotion]);

            return response.Invoke(inputResponseData);
        }

        private void InitalizeResponses()
        {
            for (int index = 0; index < _responses.Length; index++)
            {
                _responses[index] = new List<Response>();
            }
        }
    }
}
