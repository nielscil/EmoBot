using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public class InputResponse
    {
        private List<string> _patternsToMatch = new List<string>();
        private List<Tuple<int, string>> _previousResponsePatternsToMatch = new List<Tuple<int, string>>();

        private TemplateBase _template;

        public void AddPattern(string pattern)
        {
            _patternsToMatch.Add(pattern);
        }

        public void AddPreviousResponsePattern(int dept, string pattern)
        {
            _previousResponsePatternsToMatch.Add(new Tuple<int, string>(dept, pattern));
        }

        public bool IsMatch(InputResponseData inputResponseData)
        {
            foreach (var pattern in _patternsToMatch)
            {
                Match match = Regex.Match(inputResponseData.input, pattern);
                List<InputResponseData> data;

                if (match.Success && InputResponseManager.IsMatchingPreviousResponses(_previousResponsePatternsToMatch, out data))
                {
                    inputResponseData.Match = match;
                    inputResponseData.PreviousResponseData = data;
                    return true;
                }
            }
            return false;
        }

        public void SetTemplate(TemplateBase template)
        {
            _template = template;
        }

        public void GetResponse(InputResponseData finder)
        {
            if (IsMatch(finder))
            {
                ExecuteTemplate(finder);
            }
                
        }

        private void ExecuteTemplate(InputResponseData finder)
        {
            string response = _template.GetResponse(finder);

            if (string.IsNullOrWhiteSpace(response))
            {
                response = string.Empty;
            }

            finder.response = response;
        }
    }

    public class InputResponseData
    {
        public string input { get; private set; }
        public bool found { get; private set; }
        public Match Match { get; set; }
        public List<InputResponseData> PreviousResponseData { get; set; }

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
                    found = true;
                    _response = AddPunctuation(value);
                }
            }
        }

        public InputResponseData(string input)
        {
            this.input = trimInput(input);
        }

        private string trimInput(string input)
        {
            return input.Trim(' ', '?', '.', '!').ToLower();
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
