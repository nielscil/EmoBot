using Alice.Models;
using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice
{
    internal static class InputResponseManager
    {
        public static string DefaultResponse { get; set; } = "\"Huh??!\" - Kjeld";

        private static List<InputResponse> inputResponses = new List<InputResponse>();

        public static List<InputResponseData> historyInputResponses = new List<InputResponseData>();

        public static void addInputResponses(InputResponseCollection collection)
        {
            inputResponses.AddRange(collection.GetInputResponses());
        }

        internal static void GetResponse(InputResponseData finder)
        {
            foreach(var item in inputResponses)
            {
                if (!finder.found)
                {
                    GetResponse(finder);
                }
                else
                    break;
            }

            if(!finder.found)
            {
                finder.response = DefaultResponse;
            }

            historyInputResponses.Add(finder);
        }
    }
}
