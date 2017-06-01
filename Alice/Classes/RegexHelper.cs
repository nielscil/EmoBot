using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public static class RegexHelper
    {
        public static string GetValue(Match match,string groupName)
        {
            var group = match.Groups[groupName];
            if (group != null && group.Success)
            {
                return group.Value;
            }

            return string.Empty;
        }
    }
}
