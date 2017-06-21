using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
{
    public class InputResponseData
    {
        public string Input { get; private set; }
        public bool Found { get; private set; }
        public Match Match { get; set; }
        public List<InputResponseData> PreviousResponseData { get; set; }
        public GlobalActionResponse GlobalActionResponse { get; set; }

        private string _response = string.Empty;
        public string response
        {
            get
            {
                return _response;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Found = true;
                    _response = AddPunctuation(value);
                }
            }
        }

        public InputResponseData(string input)
        {
            this.Input = TrimInput(input);
        }

        private string TrimInput(string input)
        {
            return input.Trim(' ', '?', '.', '!').ToLower().Replace(",","").Replace(".","");
        }

        private string AddPunctuation(string input)
        {
            if (!input.EndsWith("?") && !input.EndsWith("!") && !input.EndsWith("."))
            {
                input += ".";
            }
            return input;
        }
    }
}
