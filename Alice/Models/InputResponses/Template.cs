using Alice.Classes;
using EmotionLib;
using EmotionLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
{
    public delegate GlobalActionResponse GlobalTemplateAction(InputResponseData inputResponseData);
    public delegate string Response(InputResponseData inputResponseData);

    public class Template : TemplateBase
    {
        private List<Response> _responses = new List<Response>();

        public Template(GlobalTemplateAction globalAction) : base(globalAction)
        {
        }

        public Template() : base() { }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }

        public override string GetResponse(InputResponseData inputResponseData)
        {
            inputResponseData.GlobalActionResponse = _globalAction?.Invoke(inputResponseData);

            Response response = ResponseChooser.Choose(_responses);

            return response?.Invoke(inputResponseData);
        }

    }

}
