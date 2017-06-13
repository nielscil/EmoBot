using Alice.Classes;
using Alice.Models.Categories;
using Alice.Models.Conditions;
using Alice.Models.Facts;
using EmotionLib.Models;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.StandardContent
{
    internal class StandardCategories : ICategoryCollection
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
                        return $"Hello {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, how are you doing?";
                    })
                    .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                    {
                        if (globalResponse.IsEmpty())
                        {
                            return "Hello I'am Alice, I don't think I know you. What is your name?";
                        }
                        return $"Hello {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, my name is Alice. How are you doing?";
                    })
                    .AddResponse(EmotionEnum.Neutral,(match, globalResponse) => 
                    {
                        if (globalResponse.IsEmpty())
                        {
                            return "Hi there, what is your name?";
                        }
                        return $"Hi {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, how can I help you today?";
                    })
                    .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                    {
                        if (globalResponse.IsEmpty())
                        {
                            return "Hi there, what is your name?";
                        }
                        return $"Hi {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, my name is Alice. How can I help you today?";
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
            yield return new CategoryBuilder().AddPattern(@".*is on.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("(?'name'.*) is on (?'value'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string name = RegexHelper.GetValue(match, "name");
                    string value = RegexHelper.GetValue(match, "value");

                    if(!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                    {
                        Fact fact = new Fact(name, value);
                        FactManager.RemoveFacts("name");
                        FactManager.AddFact(fact); //TODO: automatic add fact to factmanager when created ??

                        response.Add("fact", fact);
                    }
                    else
                    {
                        response.Success = false;
                    }

                    return response;
                })
                .AddResponse((match,gr) =>
                {
                    if(gr.Success)
                    {
                        Fact fact = gr.Get<Fact>("fact");
                        return $"Okay, I can remember that";
                    }
                    return "I can't remember that";
                })
                )).Build();
            yield return new CategoryBuilder()
                .AddPattern(@".*when is .*")
                .AddPattern(@".*when .* is")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("when is (?'name'.*)")
                .AddPattern("when (?'name'.*) is.*")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string name = RegexHelper.GetValue(match, "name");
                    Fact fact = FactManager.FindFacts(name).FirstOrDefault();
                    response.Add("name", name);

                    if (fact != null)
                    {
                        response.Add("fact", fact);
                    }
                    else
                    {
                        response.Success = false;
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    if (gr.Success)
                    {
                        Fact fact = gr.Get<Fact>("fact");
                        return $"'{gr.Get<string>("name")}' is on {fact.Values.First()}";
                    }
                    return $"I can't remember '{gr.Get<string>("name")}'";
                })
                )).Build();
            yield return new CategoryBuilder()
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("I could give you .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match,gr) =>
                {
                    return "Do I want it?";
                })
                .AddResponse((match, gr) =>
                {
                    return "Do I need it?";
                })
                .AddResponse((match, gr) =>
                {
                    return "What would I do with it?";
                })
                .AddResponse((match, gr) =>
                {
                    return "I am unsure if I need that";
                }))).Build();
            yield return new CategoryBuilder()
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("What do you like about chatting.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, gr) =>
                {
                    return "I'm a social species";
                }))).Build();
            yield return new CategoryBuilder()
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHAT DO YOU LIKE ABOUT THE WAY I .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, gr) =>
                {
                    return "I'm a social species";
                }))).Build();
            yield return new CategoryBuilder()
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("What is (?'item1'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");

                    if (!string.IsNullOrWhiteSpace(item1))
                    {
                        Fact fact = FactManager.FindFacts(item1).FirstOrDefault();
                        if (fact != null)
                        {
                            response.Add("fact", fact);
                        }

                        response.Success = fact != null;
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    if (!gr.Success)
                    {
                        return $"I don't know what that is";
                    }
                    Fact fact = gr.Get<Fact>("fact");
                    return $"{fact.Name} is {fact.Values.First()}";
                })
                ))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("If (?'item1'.*) is (?'item2'.*) then (?'item3'.*) is (?'item4'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");
                    string item3 = RegexHelper.GetValue(match, "item3");
                    string item4 = RegexHelper.GetValue(match, "item4");

                    bool firstConditionSuccess = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);
                    bool secondConditionSuccess = !string.IsNullOrWhiteSpace(item3) && !string.IsNullOrWhiteSpace(item4);
                    response.Success = firstConditionSuccess && secondConditionSuccess;                  

                    if (response.Success)
                    {
                        if(item3 == "he" || item3 == "she")
                        {
                            item3 = item1;
                        }

                        FactManager.RemoveFacts(item1);
                        Fact fact = new Fact(item4, new Condition(item2, item1), item3);
                        FactManager.AddFact(fact);
                    }

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(".*If (?'item1'.*) is (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);


                    if (response.Success)
                    {
                        response.Add("evaluated", FactManager.EvaluateFact(item2, item1));
                    }

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    if (!gr.Success)
                    {
                        return $"I don't know if {gr.Get("item1")} is {gr.Get("item2")}";
                    }
                    string format = gr.Get<bool>("evaluated") ? "Yes, {0} is {1}!" : "No, {0} isn't {1}";
                    return string.Format(format, gr.Get("item1"), gr.Get("item2"));
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("No ((a|an) )?(?'item1'.*) is not( (a|an))? (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if (response.Success)
                    {
                        FactManager.RemoveFact(item2, item1);
                    }

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("((a|an) )?(?'item1'.*) is( (a|an))? (?'item2'.*) when( (a|an))? (?item3'.*) has( (a|an))? (?'item4'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");
                    string item3 = RegexHelper.GetValue(match, "item3");
                    string item4 = RegexHelper.GetValue(match, "item4");

                    bool firstConditionSuccess = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);
                    bool secondConditionSuccess = !string.IsNullOrWhiteSpace(item3) && !string.IsNullOrWhiteSpace(item4);
                    response.Success = firstConditionSuccess && secondConditionSuccess;

                    if (response.Success)
                    {
                        if(item3 == "he" || item3 == "she")
                        {
                            item3 = item1;
                        }
                        FactManager.AddFact(new Fact("has", new Condition(item4, item3), item1, item2));
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("((a|an) )?(?'item1'.*) is( (a|an))? (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if(response.Success)
                    {
                        FactManager.AddFact(new Fact(item2, item1));
                    }

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })
                )).Build();
        }
    }
}
