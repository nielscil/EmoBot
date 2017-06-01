using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public abstract class CategoryBase
    {
        private List<string> _patterns = new List<string>();

        public void AddPattern(string pattern)
        {
            _patterns.Add(pattern);
        }

        public bool IsMatchingPatterns(string input)
        {
            foreach (string pattern in _patterns)
            {
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMatchingPatterns(string input, out Match match)
        {
            match = null;

            foreach (string pattern in _patterns)
            {
                match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return true;
                }
            }

            return false;
        }

        public abstract void Accept(IResponseFinder finder);
    }
}
