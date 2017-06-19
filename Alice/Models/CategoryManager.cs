using AIMLbot;
using Alice.Models;
using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice
{
    internal static class CategoryManager
    {
        public static string DefaultResponse { get; set; } = "Huh??!";
        private static Bot _aimlBot;
        private static User _aimlUser;
        private static bool _useAiml = false;
        private static List<Category> _categories = new List<Category>();

        internal static void AddCategories(ICategoryCollection collection)
        {
            _categories.AddRange(collection.GetCategories());
        }

        internal static void InitAiml()
        {
            _aimlBot = new Bot();
            _aimlBot.loadSettings();
            _aimlBot.loadAIMLFromFiles();
            _aimlUser = new User("currentUser", _aimlBot);
            _useAiml = true;
        }

        internal static void GetResponse(IResponseFinder finder)
        {
            foreach(var item in _categories)
            {
                if(!finder.Found)
                {
                    item.Accept(finder);
                }
            }

            if(!finder.Found && _useAiml)
            {
                GetAIMLResponse(finder);
            }

            if(!finder.Found)
            {
                finder.Response = DefaultResponse;
            }
        }

        private static void GetAIMLResponse(IResponseFinder finder)
        {
            Request request = new Request(finder.Input, _aimlUser, _aimlBot);
            Result result =_aimlBot.Chat(request);

            finder.Response = result.Output;
        }
    }
}
