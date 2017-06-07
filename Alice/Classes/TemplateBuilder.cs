using Alice.Models.Categories;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class TemplateBuilder
    {
        private Template _template;

        public TemplateBuilder()
        {
            _template = new Template();
        }

        public TemplateBuilder SetGlobalTemplateAction(GlobalTemplateAction execute)
        {
            _template.SetTemplateExecute(execute);
            return this;
        }

        public TemplateBuilder AddResponse(Response response)
        {
            _template.AddResponse(response);
            return this;
        }

        public Template Build()
        {
            return _template;
        }
    }
}
