using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alice.Models.InputResponses;

namespace Alice.Classes
{
    public static class RegexHelper
    {
        public static string GetValue(InputResponseData inputResponseData,string groupName)
        {
            var group = inputResponseData.Match.Groups[groupName];
            if (group != null && group.Success)
            {
                return group.Value;
            }

            return string.Empty;
        }
    }
}
