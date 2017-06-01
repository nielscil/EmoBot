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

namespace Alice
{
    public static class ChatBot
    {
        private static bool _initialized = false;

        public static void Init(System.Windows.Application app)
        {
            if(_initialized)
            {
                throw new Exception("Chatbot already initalized");
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

            //Fact fact = new Fact("working",null, "joris");
            //FactManager.Facts.Add(fact);
            //fact = new Fact("happy", new Condition("working", "joris"), "kjeld");
            //FactManager.Facts.Add(fact);
            //fact = new Fact("happy", new Condition("working", "kjeld"), "joris");
            //FactManager.Facts.Add(fact);
            //var and = new AndCondition();
            //and.Conditions.Add(new Condition("working", "joris"));
            //and.Conditions.Add(new NotCondition(new Condition("working", "kjeld")));
            //fact = new Fact("happy",and,"niels");
            //FactManager.Facts.Add(fact);
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
