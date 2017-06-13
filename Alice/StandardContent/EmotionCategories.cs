using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Classes;
using Alice.Models.Categories;
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
    internal class EmotionCategories : ICategoryCollection
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
                }).AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
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
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
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
                })
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Hello {globalResponse.Get("name")}, what can you do?";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Do you look happy today! What is your name?";
                    }
                    else
                    {
                        if (DateTime.Now.TimeOfDay.Hours <= 6)
                        {
                            return $"Good night, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                        }
                        else if (DateTime.Now.TimeOfDay.Hours > 6 && DateTime.Now.TimeOfDay.Hours < 12)
                        {
                            return $"Good morning, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                        }
                        else if (DateTime.Now.TimeOfDay.Hours > 12 && DateTime.Now.TimeOfDay.Hours <= 18)
                        {
                            return $"Good afternoon, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                        }
                        else
                        {
                            return $"Good evening, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                        }
                    }
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Whoah, did I say something wrong?";
                    }
                    return $"{globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I feel oncomfortable with your anger. What's wrong?";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Don't be scared, I won't bite. What is your name?";
                    }
                    return $"Relax {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, it's just me";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Whoah! I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Whoah {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, relax, it's me.";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Hello, I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Hi {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I remember you more beautifull with a smile on your face";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "You look disgusted, do I have someting between my teeth?";
                    }
                    return $"Hi {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I remember you more beautifull with a smile on your face";
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

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
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
                .AddResponse((match, gr) =>
                {
                    if (gr.Success)
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
                .AddResponse((match, gr) =>
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
                .AddPattern("The (?'item1'.*) is (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Oh, {gr.Get("item1")} is {gr.Get("item2")}";
                })
                .AddResponse((match, gr) =>
                {
                    return $"What specifically brings {gr.Get("item2")} to mind?";
                })
                .AddResponse((match, gr) =>
                {
                    return $"Is {gr.Get("item1")} also {gr.Get("item1")}";
                })
                )).Build();
        }
    }
}

