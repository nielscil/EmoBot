using Alice.Classes;
using Alice.Models.InputResponses;
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
    internal class StandardCategories : IInputResponseCollection
    {
        public IEnumerable<InputResponse> GetInputResponses()
        {
            yield return new InputResponseBuilder()
                .AddPattern(@".*hello.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                    {
                        var response = new GlobalActionResponse();
                        Fact name = FactManager.FindFacts("name").FirstOrDefault();
                        response.Add("name", name);

                        return response;
                    }).AddResponse(EmotionEnum.Neutral,(i) => 
                    {
                        if(i.GlobalActionResponse.Empty)
                        {
                            return "Hello, I don't think I know you. What is your name?";
                        }
                        return $"Hello {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, how are you doing?";
                    })
                    .AddResponse(EmotionEnum.Neutral, (i) =>
                    {
                        if (i.GlobalActionResponse.Empty)
                        {
                            return "Hello I'am Alice, I don't think I know you. What is your name?";
                        }
                        return $"Hello {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, my name is Alice. How are you doing?";
                    })
                    .AddResponse(EmotionEnum.Neutral,(i) => 
                    {
                        if (i.GlobalActionResponse.Empty)
                        {
                            return "Hi there, what is your name?";
                        }
                        return $"Hi {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, how can I help you today?";
                    })
                    .AddResponse(EmotionEnum.Neutral, (i) =>
                    {
                        if (i.GlobalActionResponse.Empty)
                        {
                            return "Hi there, what is your name?";
                        }
                        return $"Hi {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, my name is Alice. How can I help you today?";
                    })
                    ).Build();

            yield return new InputResponseBuilder()
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
                }).AddResponse(EmotionEnum.Neutral, (i) =>
                {
                    return $"Hello {i.GlobalActionResponse.Get("name")}, what can you do?";
                })
                ).Build();
            yield return new InputResponseBuilder()
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
                        FactManager.AddFact(fact);

                        response.Add("fact", fact);
                    }
                    else
                    {
                        response.Success = false;
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    if(i.GlobalActionResponse.Success)
                    {
                        Fact fact = i.GlobalActionResponse.Get<Fact>("fact");
                        return $"Okay, I can remember that";
                    }
                    return "I can't remember that";
                })
                ).Build();
            yield return new InputResponseBuilder()
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
                .AddResponse((i) =>
                {
                    if (i.GlobalActionResponse.Success)
                    {
                        Fact fact = i.GlobalActionResponse.Get<Fact>("fact");
                        return $"'{i.GlobalActionResponse.Get<string>("name")}' is on {fact.Values.First()}";
                    }
                    return $"I can't remember '{i.GlobalActionResponse.Get<string>("name")}'";
                })
                ).Build();
            yield return new InputResponseBuilder()
                .AddPattern("I could give you .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return "Do I want it?";
                })
                .AddResponse((i) =>
                {
                    return "Do I need it?";
                })
                .AddResponse((i) =>
                {
                    return "What would I do with it?";
                })
                .AddResponse((i) =>
                {
                    return "I am unsure if I need that";
                })).Build();
            yield return new InputResponseBuilder()
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
                .AddResponse((i) =>
                {
                    return $"Oh, {i.GlobalActionResponse.Get("item1")} is {i.GlobalActionResponse.Get("item2")}";
                })
                .AddResponse((i) =>
                {
                    return $"What specifically brings {i.GlobalActionResponse.Get("item2")} to mind?";
                })
                .AddResponse((i) =>
                {
                    return $"Is {i.GlobalActionResponse.Get("item1")} also {i.GlobalActionResponse.Get("item1")}";
                })
                ).Build();
        }
    }
}
