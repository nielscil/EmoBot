using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Alice.Models;
using Alice.Models.Categories;
using Alice.Models.Facts;
using Alice.Models.Conditions;
using Alice.Classes;
using Alice.StandardContent;
using AIMLbot;

namespace Alice
{
    public static class ChatBot
    {
        private static bool _initialized = false;
             
        public static void Init(System.Windows.Application app,bool useStandardCategories)
        {
            if(_initialized)
            {
                throw new Exception("Chatbot already initalized");
            }

            if(useStandardCategories)
            {
                CategoryManager.AddCategories(new StandardCategories());
                CategoryManager.AddCategories(new DateCategories());
                CategoryManager.InitAiml();
            }

            app.Exit += App_Exit;
            _initialized = true;
        }

        private static void App_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            FactManager.SaveFacts();
        }

        private static void CheckInitialized()
        {
            if(!_initialized)
            {
                throw new Exception("Chatbot not initialized");
            }
        }

        public static void LoadFacts(string path)
        {
            CheckInitialized();

            FactManager.LoadFacts(path);
        }

        public static void SetDefaultResponse(string response)
        {
            CheckInitialized();

            CategoryManager.DefaultResponse = response;
        }

        public static void AddCategories(ICategoryCollection collection)
        {
            CheckInitialized();

            CategoryManager.AddCategories(collection);
        }

        public static async Task<string> GetResponse(string input)
        {
            CheckInitialized();

            return await Task.Run(() =>
            {
                ResponseFinder finder = new ResponseFinder(input);

                CategoryManager.GetResponse(finder);

                return finder.Response;
            });
        }


    }
}
