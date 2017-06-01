using Alice.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public delegate GlobalActionResponse GlobalTemplateAction(Match match);
    public delegate string Response(Match match, GlobalActionResponse globalActionResponse);

    public class Template
    {
        private GlobalTemplateAction _globalAction;
        private List<Response> _responses = new List<Response>();

        public Template(GlobalTemplateAction globalAction)
        {
            _globalAction = globalAction;
        }

        public Template() { }

        public void SetTemplateExecute(GlobalTemplateAction globalAction)
        {
            _globalAction = globalAction;
        }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }

        public string GetResponse(Match match)
        {
            Response response = ResponseChooser.Choose(_responses);

            GlobalActionResponse globalActionResponse = _globalAction?.Invoke(match);

            return response.Invoke(match, globalActionResponse);
        }

    }

}
