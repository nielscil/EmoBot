using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Classes
{
    class StandardCategories : ICategoryCollection
    {
        public IEnumerable<Category> GetCategories()
        {
            yield return new CategoryBuilder().AddPattern(@".*hello.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@".*hello.*")
                .AddTemplate((match) =>
                    {
                        return "Hello, how are you doing?";
                    })
                    .AddTemplate((match) =>
                    {
                        return "Hi";
                    }))
                .Build();
            yield return new CategoryBuilder().AddPattern(@".*name.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@"my name is (?'name'.*)")
                .AddTemplate((match) =>
                {
                    return $"Hello {RegexHelper.GetValue(match,"name")}, how are you doing?";
                })).Build();
        }
    }
}
