using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public abstract class Matcher
    {
        protected List<string> patternsToMatch = new List<string>();
        protected List<Tuple<int, string>> previousResponsePatternsToMatch = new List<Tuple<int, string>>();

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
                if (match.Success && InputResponseManager.IsMatchingPreviousResponses(previousResponsePatternsToMatch))
                {
                    inputResponseData.Match = match;
                    return true;
                }
            }
            return false;
        }
    }
}
