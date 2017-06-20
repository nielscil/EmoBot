using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alice.Models.Categories;

namespace Alice.Classes
{
    public static class RegexHelper
    {
        public static string GetValue(InputResponseData finder,string groupName)
        {
            var group = finder.matchData.match.Groups[groupName];
            if (group != null && group.Success)
            {
                return group.Value;
            }

            return string.Empty;
        }
    }
}
