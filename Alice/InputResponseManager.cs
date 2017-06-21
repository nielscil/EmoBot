using Alice.Models;
using Alice.Models.InputResponses;
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

        private static List<InputResponse> _inputResponses = new List<InputResponse>();
        private static List<InputResponseData> _history = new List<InputResponseData>();

        public static void AddInputResponses(IInputResponseCollection collection)
        {
            _inputResponses.AddRange(collection.GetInputResponses());
        }

        internal static void GetResponse(InputResponseData inputResponseData)
        {
            foreach(var item in _inputResponses)
            {
                if (!inputResponseData.Found)
                {
                    item.GetResponse(inputResponseData);
                }
                else
                {
                    break;
                }
            }

            if(!inputResponseData.Found)
            {
                inputResponseData.response = DefaultResponse;
            }

            _history.Add(inputResponseData);
        }

        internal static bool IsMatchingPreviousResponses(List<Tuple<int,string>> previousResponses, out List<InputResponseData> data)
        {
            data = new List<InputResponseData>();
            foreach(var previousResponse in previousResponses)
            {
                int index = _history.Count - previousResponse.Item1 + 1;
                if(index < 0)
                {
                    return false;
                }

                var inputResponse = _history[index];
                if(Regex.IsMatch(inputResponse.response,previousResponse.Item2))
                {
                    data.Add(inputResponse);
                }
                else
                {
                    return false;
                }

            }
            return true;
        }
    }
}
