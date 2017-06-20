using Alice.Models;
using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice
{
    internal static class InputResponseManager
    {
        public static string DefaultResponse { get; set; } = "\"Huh??!\" - Kjeld";

        internal static List<InputResponseData> PreviousResponseData { get; private set; }

        private static List<InputResponse> inputResponses = new List<InputResponse>();

        private static List<InputResponseData> historyInputResponses = new List<InputResponseData>();

        public static void addInputResponses(IInputResponseCollection collection)
        {
            inputResponses.AddRange(collection.GetInputResponses());
        }

        internal static void GetResponse(InputResponseData finder)
        {
            PreviousResponseData = null;

            foreach(var item in inputResponses)
            {
                if (!finder.found)
                {
                    item.GetResponse(finder);
                }
                else
                {
                    break;
                }
            }

            if(!finder.found)
            {
                finder.response = DefaultResponse;
            }

            historyInputResponses.Add(finder);
        }

        internal static bool IsMatchingPreviousResponses(List<Tuple<int,string>> previousResponses)
        {
            List<InputResponseData> data = new List<InputResponseData>();
            foreach(var previousResponse in previousResponses)
            {
                int index = historyInputResponses.Count - previousResponse.Item1 + 1;
                if(index < 0)
                {
                    return false;
                }

                var inputResponse = historyInputResponses[index];
                if(Regex.IsMatch(inputResponse.response,previousResponse.Item2))
                {
                    data.Add(inputResponse);
                }
                else
                {
                    return false;
                }

            }

            PreviousResponseData = data;
            return true;
        }
    }
}
