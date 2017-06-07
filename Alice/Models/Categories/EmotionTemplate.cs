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
        private List<Response> _responses = new List<Response>();

        public EmotionTemplate(GlobalTemplateAction globalAction) : base(globalAction)
        {
        }

        public EmotionTemplate() : base() { }

        public void AddResponse(EmotionEnum emotion,Response response)
        {
            _responses.Add(response);
        }

        public override string GetResponse(Match match)
        {
            GlobalActionResponse globalActionResponse = _globalAction?.Invoke(match);

            Response response = ResponseChooser.Choose(_responses);

            return response.Invoke(match, globalActionResponse);
        }
    }
}
