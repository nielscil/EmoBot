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
        private InputResponse _inputResponse;

        public InputResponseBuilder()
        {
            _inputResponse = new InputResponse();
        }

        public InputResponseBuilder AddPattern(string pattern)
        {
            _inputResponse.AddPattern(pattern);
            return this;
        }

        public InputResponseBuilder AddPreviousResponse(int dept,string pattern)
        {
            _inputResponse.AddPreviousResponsePattern(dept, pattern);
            return this;
        }

        public InputResponseBuilder AddTemplate(TemplateBuilder builder)
        {
            _inputResponse.SetTemplate(builder.Build());
            return this;
        }

        public InputResponseBuilder AddTemplate(EmotionTemplateBuilder builder)
        {
            _inputResponse.SetTemplate(builder.Build());
            return this;
        }

        public InputResponse Build()
        {
            return _inputResponse;
        }
    }
}
