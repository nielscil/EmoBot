using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
{
    public abstract class TemplateBase
    {
        protected GlobalTemplateAction _globalAction;

        public TemplateBase(GlobalTemplateAction globalAction)
        {
            _globalAction = globalAction;
        }

        public TemplateBase() { }

        public void SetTemplateExecute(GlobalTemplateAction globalAction)
        {
            _globalAction = globalAction;
        }

        public abstract string GetResponse(InputResponseData inputResponseData);
    }
}
