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
        public static string DefaultResponse { get; set; } = "\"Huh??!\" - Kjeld";

        private static List<Category> _categories = new List<Category>();

        public static void AddCategories(ICategoryCollection collection)
        {
            _categories.AddRange(collection.GetCategories());
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

            if(!finder.Found)
            {
                finder.Response = DefaultResponse;
            }
        }
    }
}
