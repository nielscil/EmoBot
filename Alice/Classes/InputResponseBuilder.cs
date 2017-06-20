using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class InputResponseBuilder
    {
        private InputResponse inputResponse;

        public InputResponseBuilder()
        {
            inputResponse = new InputResponse();
        }

        public InputResponseBuilder AddPattern(string pattern)
        {
            inputResponse.addPattern(pattern);
            return this;
        }

        public InputResponseBuilder AddTemplate(TemplateBuilder builder)
        {
            inputResponse.SetTemplate(builder.Build());
            return this;
        }

        public InputResponseBuilder AddTemplate(EmotionTemplateBuilder builder)
        {
            inputResponse.SetTemplate(builder.Build());
            return this;
        }

        public InputResponse Build()
        {
            return inputResponse;
        }
    }
}
