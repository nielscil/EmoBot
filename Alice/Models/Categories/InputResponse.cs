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
        private List<string> patternsToMatch = new List<string>();
        List<Tuple<int, string>> previousResponsePatternsToMatch = new List<Tuple<int, string>>();

        private TemplateBase _template;

        public void addPattern(string pattern)
        {
            patternsToMatch.Add(pattern);
        }

        public void AddPreviousResponsePattern(int dept, string pattern)
        {
            previousResponsePatternsToMatch.Add(new Tuple<int, string>(dept, pattern));
        }

        public bool isMatch(InputResponseData inputResponseData)
        {
            foreach (var pattern in patternsToMatch)
            {
                Match match = Regex.Match(inputResponseData.input, pattern);
                List<InputResponseData> data;

                if (match.Success && InputResponseManager.IsMatchingPreviousResponses(previousResponsePatternsToMatch, out data))
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
            if (isMatch(finder))
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

        public InputResponseData(string _input)
        {
            input = trimInput(_input);
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
