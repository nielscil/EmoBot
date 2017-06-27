using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    internal static class ListExtentions
    {
        internal static Dictionary<int, List<string>> ToSortedDictonary(this List<Tuple<int, string>> input)
        {
            var response = new Dictionary<int, List<string>>();

            foreach (var item in input)
            {
                if (!response.ContainsKey(item.Item1))
                {
                    response[item.Item1] = new List<string>();
                }

                response[item.Item1].Add(item.Item2);
            }

            return response;
        }
    }
}
