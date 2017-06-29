using Alice.Models;
using Alice.Models.InputResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    internal static class ResponseChooser
    {
        private static Random _random = new Random();

        public static Response Choose(List<Response> responses)
        {
            int responseIndex = GetIndex(responses);
            if (responseIndex < responses.Count)
            {
                return responses[GetIndex(responses)];
            }
            return null;
        }

        private static int GetIndex(List<Response> responses)
        {
            int index = 0;
            if (responses.Count > 1)
            {
                index = _random.Next(0, responses.Count);
            }
            return index;
        }

    }
}
