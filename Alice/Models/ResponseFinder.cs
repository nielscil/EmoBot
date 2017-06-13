using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models
{
    public sealed class ResponseFinder : IResponseFinder
    {
        public string Input { get; private set; }

        private string _response = string.Empty;
        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    Found = true;
                    _response = AddPunctuation(value);
                }
            }
        }

        public bool Found { get; private set; }

        public ResponseFinder(string input)
        {
            Input = TrimInput(input);
        }

        private string TrimInput(string input)
        {
            return input.Trim(' ', '?', '.', '!').ToLower().Replace(",","").Replace(".","");
        }

        private string AddPunctuation(string input)
        {
            if(!input.EndsWith("?") && !input.EndsWith("!") && !input.EndsWith("."))
            {
                input += ".";
            }
            return input;
        }

    }
}
