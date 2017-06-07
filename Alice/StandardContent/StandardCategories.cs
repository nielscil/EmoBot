using Alice.Classes;
using Alice.Models.Categories;
using Alice.Models.Facts;
using EmotionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.StandardContent
{
    class StandardCategories : ICategoryCollection
    {
        public IEnumerable<Category> GetCategories()
        {
            yield return new CategoryBuilder().AddPattern(@".*hello.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@".*hello.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                    {
                        var response = new GlobalActionResponse();
                        Fact name = FactManager.FindFacts("name").FirstOrDefault();
                        response.Add("name", name);

                        return response;
                    }).AddResponse(EmotionEnum.Neutral,(match, globalResponse) => 
                    {
                        if(globalResponse.IsEmpty())
                        {
                            return "Hello, I don't think I know you. What is your name?";
                        }
                        return $"Hello {globalResponse.Get<Fact>("name").Values.FirstOrDefault()}, how are you doing?";
                    }).AddResponse(EmotionEnum.Neutral,(match, globalResponse) => 
                    {
                        if (globalResponse.IsEmpty())
                        {
                            return "Hi there, what is your name?";
                        }
                        return $"Hi {globalResponse.Get<Fact>("name").Values.FirstOrDefault()}, how can I help you today?";
                    })
                    )).Build();

            yield return new CategoryBuilder().AddPattern(@".*name.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@"my name is (?'name'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "name");
                    FactManager.RemoveFacts("name");
                    FactManager.AddFact(new Fact("name", name));
                    response.Add("name", name);

                    return response;
                }).AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Hello {globalResponse.Get("name")}, what can you do?";
                })
                )).Build();
        }
    }
}
