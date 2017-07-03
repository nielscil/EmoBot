using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
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

        private bool IsMatch(InputResponseData inputResponseData)
        {
            foreach (string pattern in _patternsToMatch)
            {
                Match match = Regex.Match(inputResponseData.Input, pattern, RegexOptions.IgnoreCase);
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

        public void GetResponse(InputResponseData inputResponseData)
        {
            if (IsMatch(inputResponseData))
            {
                ExecuteTemplate(inputResponseData);
            }
                
        }

        private void ExecuteTemplate(InputResponseData inputResponseData)
        {
            string response = _template.GetResponse(inputResponseData);

            if (string.IsNullOrWhiteSpace(response))
            {
                response = string.Empty;
            }

            inputResponseData.Response = response;
        }
    }
}
