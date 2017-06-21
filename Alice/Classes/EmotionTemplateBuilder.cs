using Alice.Models.InputResponses;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class EmotionTemplateBuilder
    {
        private EmotionTemplate _template;

        public EmotionTemplateBuilder()
        {
            _template = new EmotionTemplate();
        }

        public EmotionTemplateBuilder SetGlobalTemplateAction(GlobalTemplateAction execute)
        {
            _template.SetTemplateExecute(execute);
            return this;
        }

        public EmotionTemplateBuilder AddResponse(EmotionEnum emotion,Response response)
        {
            _template.AddResponse(emotion, response);
            return this;
        }

        public EmotionTemplate Build()
        {
            return _template;
        }
    }
}
