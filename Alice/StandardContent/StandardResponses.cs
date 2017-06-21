using Alice.Classes;
using Alice.Models.InputResponses;
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
    internal class StandardResponses : IInputResponseCollection
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
                .AddPattern("What has ((a|an) )?(?'item1'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");

                    if (!string.IsNullOrWhiteSpace(item1))
                    {
                        List<Fact> facts = FactManager.FindFactsWithGivenValues(item1, true, "has", null);
                        if (facts.Count > 0)
                        {
                            response.Add("facts", facts);
                        }

                        response.Success = facts.Count > 0;
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    if (i.GlobalActionResponse.Success)
                    {
                        List<Fact> facts = i.GlobalActionResponse.Get<List<Fact>>("facts");
                        string response = string.Empty;
                        for (int j = 0; j < facts.Count; j++)
                        {
                            response += facts[j].Values[1];

                            if (j + 2 == facts.Count)
                                response += " and ";
                            else if (j + 1 != facts.Count)
                                response += ", ";
                        }
                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            return "Well, according to me " + response;
                        }
                    }
                    return $"I don't know";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("What is ((a|an) )?(?'item1'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");

                    if (!string.IsNullOrWhiteSpace(item1))
                    {
                        List<Fact> facts = FactManager.FindFacts(item1, true);
                        if (facts.Count > 0)
                        {
                            response.Add("facts", facts);
                        }

                        response.Success = facts.Count > 0;
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    if (i.GlobalActionResponse.Success)
                    {
                        List<Fact> facts = i.GlobalActionResponse.Get<List<Fact>>("facts");
                        string response = string.Empty;
                        for (int j = 0; j < facts.Count; j++)
                        {
                            response += facts[j].Values.First();

                            if (j + 2 == facts.Count)
                                response += " and ";
                            else if (j + 1 != facts.Count)
                                response += ", ";
                        }
                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            return "Well, according to me " + response;
                        }
                    }
                    return $"I don't know";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("If (?'item1'.*) is (?'item2'.*) then (?'item3'.*) is (?'item4'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");
                    string item3 = RegexHelper.GetValue(match, "item3").Trim();
                    string item4 = RegexHelper.GetValue(match, "item4");

                    bool firstConditionSuccess = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);
                    bool secondConditionSuccess = !string.IsNullOrWhiteSpace(item3) && !string.IsNullOrWhiteSpace(item4);
                    response.Success = firstConditionSuccess && secondConditionSuccess;

                    if (response.Success)
                    {
                        if (item3 == "he" || item3 == "she" || item3 == "it")
                        {
                            item3 = item1;
                        }
                        Fact fact = new Fact(item3, new Condition(item1, item2), item2);
                        FactManager.AddFact(fact);
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    return $"Alright";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("Can.*If (?'item1'.*) has (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    bool evaluate = response.Success && (FactManager.EvaluateFact(item1, "has", item2) || new DynamicForeachCondition(item1, new Condition(null, item2), "has", null).Evaluate());
                    response.Add("evaluated", evaluate);

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((i) =>
                {
                    if (!i.GlobalActionResponse.Success)
                    {
                        return $"I don't know if {i.GlobalActionResponse.Get("item1")} has {i.GlobalActionResponse.Get("item2")}";
                    }
                    string format = i.GlobalActionResponse.Get<bool>("evaluated") ? "Yes, {0} has {1}!" : "No, {0} hasn't got {1}";
                    return string.Format(format, i.GlobalActionResponse.Get("item1"), i.GlobalActionResponse.Get("item2"));
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("Can.*If (?'item1'.*) is (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);


                    if (response.Success)
                    {
                        response.Add("evaluated", FactManager.EvaluateFact(item1, item2));
                    }

                    response.Add("item1", item1);
                    response.Add("item2", item2);

                    return response;
                })
                .AddResponse((i) =>
                {
                    if (!i.GlobalActionResponse.Success)
                    {
                        return $"I don't know if {i.GlobalActionResponse.Get("item1")} is {i.GlobalActionResponse.Get("item2")}";
                    }
                    string format = i.GlobalActionResponse.Get<bool>("evaluated") ? "Yes, {0} is {1}!" : "No, {0} isn't {1}";
                    return string.Format(format, i.GlobalActionResponse.Get("item1"), i.GlobalActionResponse.Get("item2"));
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("No (?'item1'.*) is not (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if (response.Success)
                    {
                        FactManager.RemoveFact(item1, item2);
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    return $"Alright";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("(?'item1'.*) has (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if (response.Success)
                    {
                        FactManager.AddFact(new Fact(item1, "has", item2));
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    return $"Alright";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("(?'item1'.*) is (?'item2'.*) when (?'item3'.*) has (?'item4'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");
                    string item3 = RegexHelper.GetValue(match, "item3").Trim();
                    string item4 = RegexHelper.GetValue(match, "item4");

                    bool firstConditionSuccess = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);
                    bool secondConditionSuccess = !string.IsNullOrWhiteSpace(item3) && !string.IsNullOrWhiteSpace(item4);
                    response.Success = firstConditionSuccess && secondConditionSuccess;

                    if (response.Success)
                    {
                        if (item3 == "he" || item3 == "she" || item3 == "it")
                        {
                            item3 = item1;
                        }
                        FactManager.AddFact(new Fact(item1, new OrCondition(new Condition(item3, "has", item4), new DynamicForeachCondition(item3, new Condition(null, item4), "has", null)), item2));
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    return $"Alright";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern("((a|an) )?(?'item1'.*) is (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if (response.Success)
                    {
                        FactManager.AddFact(new Fact(item1, item2));
                    }

                    return response;
                })
                .AddResponse((i) =>
                {
                    return $"Alright";
                })).Build();
        }
    }
}
