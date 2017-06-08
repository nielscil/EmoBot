using Alice.Classes;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public class EmotionTemplate : TemplateBase
    {
        private List<Response>[] _responses = new List<Response>[Enum.GetNames(typeof(EmotionEnum)).Length];

        public EmotionTemplate(GlobalTemplateAction globalAction) : base(globalAction)
        {
            InitalizeResponses();
        }

        public EmotionTemplate() : base()
        {
            InitalizeResponses();
        }

        public void AddResponse(EmotionEnum emotion, Response response)
        {
            _responses[(int)emotion].Add(response);
        }

        public override string GetResponse(Match match)
        {
            GlobalActionResponse globalActionResponse = _globalAction?.Invoke(match);
            Response response = ResponseChooser.Choose(_responses[(int)EmotionLib.EmotionDetector.Instance.Emotion]);

            return response.Invoke(match, globalActionResponse);
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
