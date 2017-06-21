using AIMLbot;
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
        private static Bot _aimlBot;
        private static User _aimlUser;
        private static bool _useAiml = false;
        private static List<InputResponse> _inputResponses = new List<InputResponse>();
        private static List<InputResponseData> _history = new List<InputResponseData>();

        public static void AddInputResponses(IInputResponseCollection collection)
        {
            _inputResponses.AddRange(collection.GetInputResponses());
        }

        internal static void InitAiml()
        {
            _aimlBot = new Bot();
            _aimlBot.loadSettings();
            _aimlBot.loadAIMLFromFiles();
            _aimlUser = new User("currentUser", _aimlBot);
            _useAiml = true;
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

            if (!inputResponseData.Found && _useAiml)
            {
                GetAIMLResponse(inputResponseData);
            }

            if (!inputResponseData.Found)
            {
                inputResponseData.Response = DefaultResponse;
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
                if(Regex.IsMatch(inputResponse.Response,previousResponse.Item2))
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

        private static void GetAIMLResponse(InputResponseData inputResponseData)
        {
            Request request = new Request(inputResponseData.Input, _aimlUser, _aimlBot);
            Result result = _aimlBot.Chat(request);

            inputResponseData.Response = result.Output;
        }
    }
}
