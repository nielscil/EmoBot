using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Alice.Models;

namespace Alice
{
    public static class ChatBot
    {
        public static void LoadFacts(string path)
        {
            FactManager.LoadFacts(path);
        }

        public static void SetDefaultResponse(string response)
        {
            CategoryManager.DefaultResponse = response;
        }

        public static void AddCategories(ICategoryCollection collection)
        {
            CategoryManager.AddCategories(collection);
        }

        public static async Task<string> GetResponse(string input)
        {
            return await Task.Run(() =>
            {
                ResponseFinder finder = new ResponseFinder(input);

                CategoryManager.GetResponse(finder);

                return finder.Response;
            });
        }
    }
}
