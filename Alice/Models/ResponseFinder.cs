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

        private string _response;
        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;

                if(!string.IsNullOrWhiteSpace(_response))
                {
                    Found = true;
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
            return input.Trim(' ', '?', '.', '!').ToLower();
        }

    }
}
