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

namespace Alice.Models.Categories
{
    public delegate GlobalActionResponse GlobalTemplateAction(InputResponseData finder);
    public delegate string Response(InputResponseData finder, GlobalActionResponse globalActionResponse);

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

        public override string GetResponse(InputResponseData finder)
        {
            finder.globalAction = _globalAction;
            GlobalActionResponse globalActionResponse = _globalAction?.Invoke(finder);

            Response response = ResponseChooser.Choose(_responses);

            return response.Invoke(finder, globalActionResponse);
        }

    }

}
