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
        MatchData matchData = null;

        public void addPattern(string pattern)
        {
            patternsToMatch.Add(pattern);
        }

        public bool isMatch(string input)
        {
            matchData = new MatchData(input, patternsToMatch);
            return matchData.isMatch;
        }
    }

    public class MatchData
    {
        private List<string> patternsToMatch;
        public string input;
        public bool isMatch = false;
        public Match match = null;

        public MatchData(string _input, List<string> _patternsToMatch)
        {
            input = _input;
            patternsToMatch = _patternsToMatch;
            isMatch = generateData();
        }

        private bool generateData()
        {
            foreach(string pattern in patternsToMatch)
            {
                match = Regex.Match(input, pattern);

                if (match.Success)
                    return true;
            }

            return false;
        }
    }
}
