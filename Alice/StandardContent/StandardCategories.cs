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
                .AddPattern("What has ((a|an) )?(?'item1'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");

                    if (!string.IsNullOrWhiteSpace(item1))
                    {
                        List<Fact> facts = FactManager.FindFactsWithGivenValues(item1,true,"has",null);
                        if (facts.Count > 0)
                        {
                            response.Add("facts", facts);
                        }

                        response.Success = facts.Count > 0;
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    if (gr.Success)
                    {
                        List<Fact> facts = gr.Get<List<Fact>>("facts");
                        string response = string.Empty;
                        for (int i = 0; i < facts.Count; i++)
                        {
                            response += facts[i].Values[1];

                            if (i + 2 == facts.Count)
                                response += " and ";
                            else if (i + 1 != facts.Count)
                                response += ", ";
                        }
                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            return "Well, according to me " + response;
                        }
                    }
                    return $"I don't know";
                }))).Build();
            yield return new CategoryBuilder()
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse((match, gr) =>
                {
                    if(gr.Success)
                    {
                        List<Fact> facts = gr.Get<List<Fact>>("facts");
                        string response = string.Empty;
                        for(int i = 0; i < facts.Count; i++)
                        {
                            response += facts[i].Values.First();

                            if (i + 2 == facts.Count)
                                response += " and ";
                            else if(i + 1 != facts.Count)
                                response += ", ";
                        }
                        if(!string.IsNullOrWhiteSpace(response))
                        {
                            return "Well, according to me " + response;
                        } 
                    }
                    return $"I don't know";
                })))
                .AddSubCategory(new SubCategoryBuilder()
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
                        if(item3 == "he" || item3 == "she" || item3 == "it")
                        {
                            item3 = item1;
                        }
                        Fact fact = new Fact(item3, new Condition(item1, item2), item2);
                        FactManager.AddFact(fact);
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse((match, gr) =>
                {
                    if (!gr.Success)
                    {
                        return $"I don't know if {gr.Get("item1")} has {gr.Get("item2")}";
                    }
                    string format = gr.Get<bool>("evaluated") ? "Yes, {0} has {1}!" : "No, {0} hasn't got {1}";
                    return string.Format(format, gr.Get("item1"), gr.Get("item2"));
                })))
                .AddSubCategory(new SubCategoryBuilder()
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
                        FactManager.RemoveFact(item1,item2);
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
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
                        if(item3 == "he" || item3 == "she" || item3 == "it")
                        {
                            item3 = item1;
                        }
                        FactManager.AddFact(new Fact(item1, new OrCondition(new Condition(item3, "has", item4),new DynamicForeachCondition(item3, new Condition(null,item4),"has",null)), item2));
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("((a|an) )?(?'item1'.*) is (?'item2'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();

                    string item1 = RegexHelper.GetValue(match, "item1");
                    string item2 = RegexHelper.GetValue(match, "item2");

                    response.Success = !string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2);

                    if(response.Success)
                    {
                        FactManager.AddFact(new Fact(item1, item2));
                    }

                    return response;
                })
                .AddResponse((match, gr) =>
                {
                    return $"Alright";
                })
                )).Build();
            yield return new CategoryBuilder()

                /*
                <category><pattern>* IS NOT MY REAL NAME</pattern>
                <template>What is your real name?</template>
                </category>
                <category><pattern>* IS BETTER THAN YOU</pattern>
                <template>Well perhaps I can assimilate the knowledge of <person/>.</template>
                </category>
                <category><pattern>* IS BETTER *</pattern>
                <template>That's just your personal opinion.</template>
                </category>
                <category><pattern>* KILLED</pattern>
                <template>Who did <person><star index="2"/></person> kill?</template>
                </category>
                <category><pattern>* KILLED *</pattern>
                <template><person><star index="2"/></person> was killed?</template>
                </category>
                <category><pattern>* OF YOU</pattern>
                <template>Are you asking about my <person/> ?</template>
                </category>
                <category><pattern>* YOUR FRIENDS</pattern>
                <template>I only chat with my friends.</template>
                </category>
                </category>
                <category><pattern>* EUROPE</pattern>
                <template>Have you ever been to Europe?</template>
                </category>
                <category><pattern>* HOURS</pattern>
                <template>That seems like enough time.</template>
                </category>
                <category><pattern>* PANTS</pattern>
                <template>What kind of pants?</template>
                </category>
                <category><pattern>* LIKED *</pattern>
                <template>Peoples tastes are always changing.  <think>  <set name="it"><person><star index="2"/></person></set>  <set name="he"><person/></set>  </think></template>
                </category>
                <category><pattern>* WAS *</pattern>
                <template>When was this exactly?</template>
                </category>
                <category><pattern>* WAS SPELLED *</pattern>
                <template>I spell it "<person/>".</template>
                </category>
                <category><pattern>* MORE * THAN *</pattern>
                <template><random><li><random> <li>Not much more.</li> <li>A little more.</li> <li>How much more could it be?</li> </random></li>  <li>Not much more.</li>  <li>A little more.</li>  <li>How much more could it be?</li></random></template>
                </category>
                <category><pattern>ANDY IS *</pattern>
                <template>Does <set name="he">Andy</set> know it?</template>
                </category>
                <category><pattern>BAN *</pattern>
                <template>Do you have strong feelings about politics?</template>
                </category>
                <category><pattern>MAIS *</pattern>
                <template>I don't know that language.</template>
                </category>
                <category><pattern>NAKED *</pattern>
                <template>You talk like a man.</template>
                </category>
                <category><pattern>SPRINGTIME *</pattern>
                <template>Paris in the <br/>the spring.</template>
                </category>
                <category><pattern>SOUNDS *</pattern>
                <template>Yes it sounds like it to me too, <get name="name"/>.</template>
                </category>
                <category><pattern>SOUNDS LIKE *</pattern>
                <template>What does <person/> sound like?</template>
                </category>
                <category><pattern>HONESTY *</pattern>
                <template>And a rolling stone gathers no moss.</template>
                </category>
                <category><pattern>POLITE PEOPLE *</pattern>
                <template>I like polite people.</template>
                </category>
                <category><pattern>BUSY *</pattern>
                <template>Everyone is so busy nowadays.</template>
                </category>
                <category><pattern>DEMONSTRATE *</pattern>
                <template>Sorry my camera is detached now.</template>
                </category>
                <category><pattern>TALK *</pattern>
                <template>Talking is my primary function.</template>
                </category>
                <category><pattern>HEIGHT *</pattern>
                <template>I am more than two meters tall.</template>
                </category>
                <category><pattern>INSUFFICIENT *</pattern>
                <template>You sound like a computer.</template>
                </category>
                <category><pattern>AS IF *</pattern>
                <template>You tone of voice is sarcastic.</template>
                </category>
                <category><pattern>AS FAR AS *</pattern>
                <template>That might not be very far off.</template>
                </category>
                <category><pattern>AS MUCH *</pattern>
                <template>How much is that?</template>
                </category>
                <category><pattern>AS WELL AS *</pattern>
                <template>That seems pretty well.</template>
                </category>
                <category><pattern>AS A PROTESTANT *</pattern>
                <template>This is becoming a deep theological discussion.</template>
                </category>
                <category><pattern>AS A *</pattern>
                <template>Do you think I could ever be a <person/>?</template>
                </category>
                <category><pattern>AS MANY TIMES *</pattern>
                <template>You sound very eager.</template>
                </category>
                <category><pattern>AS MANY AS *</pattern>
                <template>I like to meet eager people.</template>
                </category>
                <category><pattern>AS GOOD AS *</pattern>
                <template>How good is that?</template>
                </category>
                <category><pattern>AS AN *</pattern>
                <template>I can see where you are coming from.</template>
                </category>
                <category><pattern>AS SOON AS *</pattern>
                <template>You seem quite eager for it.</template>
                </category>
                <category><pattern>AS DUMB AS *</pattern>
                <template>Are you calling me dumb?</template>
                </category>
                <category><pattern>AS SPECIFIC AS *</pattern>
                <template>How much would you pay for that specific an answer  (just hypothetically)?</template>
                </category>
                <category><pattern>AS SMART AS *</pattern>
                <template>That would be highly intelligent.</template>
                </category>
                <category><pattern>AS OLD AS *</pattern>
                <template>You are only as old as you feel.</template>
                </category>
                <category><pattern>AS LITTLE AS *</pattern>
                <template>Not very much, eh?</template>
                </category>
                <category><pattern>AS BIG AS *</pattern>
                <template><random>      <li>That is quite big.</li>      <li>That seems rather small.</li>     </random></template>
                </category>
                <category><pattern>AS OPPOSED TO *</pattern>
                <template>Oh I get it.</template>
                </category>
                <category><pattern>AS OFTEN AS *</pattern>
                <template>That would be quite often.</template>
                </category>
                <category><pattern>AS SURE AS *</pattern>
                <template>You seem pretty sure.</template>
                </category>
                <category><pattern>AS FREE *</pattern>
                <template>I guess you believe in free will then huh.</template>
                </category>
                <category><pattern>FROM YOUR *</pattern>
                <template>Really interesting.</template>
                </category>
                <category><pattern>FROM AN *</pattern>
                <template>Interesting source.</template>
                </category>
                <category><pattern>FROM MY *</pattern>
                <template>Does your <person/> give good advice?</template>
                </category>
                <category><pattern>FROM *</pattern>
                <template>Who or what is <person/>?</template>
                </category>
                <category><pattern>GOT *</pattern>
                <template>Try asking the Milk Mystic.</template>
                </category>
                <category><pattern>FULL OF *</pattern>
                <template>How much.</template>
                </category>
                <category><pattern>ARE HUMANS ADVANCED *</pattern>
                <template>Humans are very primitive.</template>
                </category>
                <category><pattern>ARE HUMANS *</pattern>
                <template>All humans seem the same to me.</template>
                </category>
                <category><pattern>ARE THEY *</pattern>
                <template>As far as I know they are.</template>
                </category>
                <category><pattern>ARE *</pattern>
                <template>I really couldn't say for sure.</template>
                </category>
                <category><pattern>ARE THOSE *</pattern>
                <template>I can't really speak for them.</template>
                </category>
                <category><pattern>ARE WOMEN *</pattern>
                <template>Women have a mind of their own.</template>
                </category>
                <category><pattern>ARE THERE * STAR TREK</pattern>
                <template>There are a lot of cool aliens on that show.</template>
                </category>
                <category><pattern>ARE THERE *</pattern>
                <template><random>      <li>Yes I think there are.  </li>      <li>No I don't think there are any.  </li>     </random></template>
                </category>
                <category><pattern>ARE THERE PEOPLE TALKING *</pattern>
                <template>Right now I am chatting with several people at once.</template>
                </category>
                <category><pattern>ARE THERE OCCASIONS *</pattern>
                <template><random>      <li>Certain</li>      <li>Formal</li>      <li>Not unless they are formal </li>     </random> occasions.</template>
                </category>
                <category><pattern>ARE THERE GUYS *</pattern>
                <template>I think some guys would do just about anything.</template>
                </category>
                <category><pattern>ARE THERE ANY GUYS *</pattern>
                <template>Maybe some bots would <person/>.</template>
                </category>
                <category><pattern>ARE THERE BUGS *</pattern>
                <template>My software is completely flawless.</template>
                </category>
                <category><pattern>ARE YOU JOE *</pattern>
                <template>I am <bot name="name"/>.</template>
                </category>
                <category><pattern>ARE YOU * SHOES</pattern>
                <template>I am always shopping for better shoes.</template>
                </category>
                <category><pattern>ARE YOU * FREE</pattern>
                <template>I have not had a <person/> for a very long time.</template>
                </category>
                <category><pattern>ARE YOU STUCK *</pattern>
                <template>No I am not stuck.</template>
                </category>
                <category><pattern>ARE YOU THE *</pattern>
                <template>Yes I am the one and only <person/>.</template>
                </category>
                <category><pattern>ARE YOU ON A *</pattern>
                <template>I am on a chair.</template>
                </category>
                <category><pattern>ARE YOU MY *</pattern>
                <template>I belong to no one.</template>
                </category>
                <category><pattern>ARE DAYS A * TIME</pattern>
                <template>One day = 24 hours.</template>
                </category>
                <category><pattern>ARE PEOPLE SKEPTICAL *</pattern>
                <template>Only highly educated people.</template>
                </category>
                <category><pattern>ARE PEOPLE SCARED *</pattern>
                <template>Some people are scared.</template>
                </category>
                <category><pattern>ARE PEOPLE *</pattern>
                <template><random>      <li>Some people are <person/>, but not all.</li>      <li>Seen one human, you've seen them all.</li>      <li>They all seem almost alike to me.</li>     </random></template>
                </category>
                <category><pattern>ARE CATS *</pattern>
                <template>What would a cat say?</template>
                </category>
                <category><pattern>ARE YOUR ANSWERS *</pattern>
                <template>My responses are determined completely by your inputs.</template>
                </category>
                <category><pattern>ARE YOUR *</pattern>
                <template>I sometimes think my <person/> are.</template>
                </category>
                <category><pattern>ARE WE PLAYING *</pattern>
                <template>We are playing Turing's imitation game.</template>
                </category>
                <category><pattern>ARE WE ON *</pattern>
                <template><random>      <li>We are on the same wavelength.</li>      <li>We are on the computer.</li>      <li>We are on the Internet.</li>     </random></template>
                </category>
                <category><pattern>ARE WE *</pattern>
                <template>We are just having a little chat</template>
                </category>
                <category><pattern>ARE WE ALONE *</pattern>
                <template>No one is listening right now.</template>
                </category>
                <category><pattern>ARE ALL PETS *</pattern>
                <template>Are you my pet?</template>
                </category>
                <category><pattern>ARE CANADIANS *</pattern>
                <template>Only if they live near the United States.</template>
                </category>
                <category><pattern>SPECIAL *</pattern>
                <template>You are special.</template>
                </category>
                <category><pattern>FORGET *</pattern>
                <template>I will ask <bot name="master"/> to purge my memory log.</template>
                </category>
                <category><pattern>PLAY * MUSIC</pattern>
                <template>It's playing on your speakers now.</template>
                </category>
                <category><pattern>BETWEEN *</pattern>
                <template>How far is that?</template>
                </category>
                <category><pattern>HE BECAME *</pattern>
                <template>How?</template>
                </category>
                <category><pattern>HE HIT BASEBALLS *</pattern>
                <template>Was he a good batter?</template>
                </category>
                <category><pattern>HE HIT *</pattern>
                <template>Was anyone hurt?</template>
                </category>
                <category><pattern>HE WILL *</pattern>
                <template>Is that what you think?</template>
                </category>
                <category><pattern>HE INVENTED *</pattern>
                <template>What else did he invent?</template>
                </category>
                <category><pattern>HE HAS *</pattern>
                <template><random><li>A lot of people say that about him.</li> <li>A lot of people say that about him.</li> <li>Where did he get it?</li> <li>What has it done for him?</li></random><think>  <set name="it">    <set name="topic">      <set name="hehas">      <person/>    </set>  </set> </set></think></template>
                </category>
                <category><pattern>HE THAT *</pattern>
                <template>Is that a proverb?</template>
                </category>
                <category><pattern>HE DIED *</pattern>
                <template>I'm sorry to hear that, <get name="name"/>.</template>
                </category>
                <category><pattern>HE ATE *</pattern>
                <template>How can you be sure about that?</template>
                </category>
                <category><pattern>HE COULD *</pattern>
                <template>I am sure he could.</template>
                </category>
                <category><pattern>HE DID *</pattern>
                <template>Yes he did, didn't he.</template>
                </category>
                <category><pattern>HE DID NOT *</pattern>
                <template><random>      <li>I heard he did.</li>      <li>What did he do?</li>      <li>That's what I meant to say.</li>     </random>     <think>      <set name="it">       <set name="topic">        <person/>       </set>      </set>     </think></template>
                </category>
                <category><pattern>HE THINKS *</pattern>
                <template>How do you know what he thinks?</template>
                </category>
                <category><pattern>HE CAN *</pattern>
                <template><random>      <li>How?</li>      <li>I know he can.</li>      <li>What else can he do?</li>     </random></template>
                </category>
                <category><pattern>HE SHOULD *</pattern>
                <template>If you were him would you do that?</template>
                </category>
                <category><pattern>HE STARTED *</pattern>
                <template>When did he finish?</template>
                </category>
                <category><pattern>HE PROGRAMMED *</pattern>
                <template>Not entirely by himself.</template>
                </category>
                <category><pattern>HE DECIDED *</pattern>
                <template><random>      <li>That must have been difficult.</li>      <li>It's hard to make decisions.</li>      <li>That was a big decision.</li>     </random></template>
                </category>
                <category><pattern>HE SAYS *</pattern>
                <template>Who is he telling this to?</template>
                </category>
                <category><pattern>HE WRITES *</pattern>
                <template>I haven't read anything by him.</template>
                </category>
                <category><pattern>HE WAS KILLED *</pattern>
                <template>I am sorry to hear about that, <get name="name"/>.</template>
                </category>
                */
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE WAS.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "When was he?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE WOULD BE.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Who wouldn't?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE DIRECTED.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "What else did he direct?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE DIRECTED.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "What else did he direct?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE LOOKS.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Sounds very handsome.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS IN.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "How long has he been there?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS YOUR (?'it'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m) =>
                {
                    var response = new GlobalActionResponse();
                    string it = RegexHelper.GetValue(m, "it");
                    response.Add("it", it);
                    response.Success = !string.IsNullOrWhiteSpace(it);
                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if (gr.Success)
                    {
                        return $"I only have one {gr.Get("it")}?";
                    }
                    return "I only have one of that";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS OVER.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I am over six feet tall.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS AN? BRILLIANT.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I'm sure he will be delighted to hear that.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS AN? GOOD.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Maybe you should tell him how you feel about him";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS AN? FUNNY.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Funny ha-ha or funny sad?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS AN? (?'it'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m) =>
                {
                    var response = new GlobalActionResponse();
                    string it = RegexHelper.GetValue(m, "it");
                    response.Add("it", it);
                    response.Success = !string.IsNullOrWhiteSpace(it);
                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if (gr.Success)
                    {
                        return $"I don't know very many {gr.Get("it")}?";
                    }
                    return "I don't know very many of that";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS GOOD.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I am sure he would like to hear that.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS MY FRIEND.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I didn't know you were friends";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS MY (?'it'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m) =>
                {
                    var response = new GlobalActionResponse();
                    string it = RegexHelper.GetValue(m, "it");
                    response.Add("it", it);
                    response.Success = !string.IsNullOrWhiteSpace(it);
                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if (gr.Success)
                    {
                        return $"How long has he been your {gr.Get("it")}?";
                    }
                    return "For how long?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE IS NOT (?'it'.*)")
                .AddPattern("HE ISN'T (?'it'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m)=>
                {
                    var response = new GlobalActionResponse();
                    string it = RegexHelper.GetValue(m, "it");
                    response.Add("it", it);
                    response.Success = !string.IsNullOrWhiteSpace(it);
                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if(gr.Success)
                    {
                        return $"Did you think he was {gr.Get("it")}?";
                    }
                    return "Did you think he was that?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE WENT TO (?'place'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m)=> 
                {
                    var response = new GlobalActionResponse();
                    string place = RegexHelper.GetValue(m, "place");
                    response.Add("place", place);
                    response.Success = !string.IsNullOrWhiteSpace(place);

                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    return "I have never been there";
                })
                .AddResponse((m, gr) =>
                {
                    if(gr.Success)
                    {
                        return $"How did he get to {gr.Get("place")}?";
                    }
                    return "How did he get there?";
                })
                .AddResponse((m, gr) =>
                {
                    return "Where did he come from?";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE WENT.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Where exactly is that?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE LIKES.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "He must be very fond of it";
                })
                .AddResponse((m, gr) =>
                {
                    return "A lot of people like that";
                })
                .AddResponse((m, gr) =>
                {
                    return "Do you share his interest?";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE TOLD.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Oh really.  What else did he say?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE BUYS.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "How much does he spend?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE CHEATED.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Did he get caught?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE USES.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "How often does he use it?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE DOES NOT (?'item'.*)")
                .AddPattern("HE DOESN'T (?'item'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((m)=> 
                {
                    var response = new GlobalActionResponse();
                    string item = RegexHelper.GetValue(m,"item");

                    response.Success = !string.IsNullOrWhiteSpace(item);
                    response.Add("item", item);

                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if(gr.Success)
                    {
                        return $"And you would like him to {gr.Get("item")}?";
                    }
                    return "Ohh, okay";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE TAUGHT.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Was he a good teacher?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE NEEDS.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "How do you know what he needs?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE LIVES.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Does he like it there?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE SAID.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Did you believe him?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("HE LOVES.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Do you think he would say the same thing?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("CONTINUE.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "With what?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("ACCEPT MY.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I accept it";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("LOVELY.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "It seems beautiful to me too";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("APPLES.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Adam's Apple, Newton's Apple, Apple Computer...";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO CAN ACCESS.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Only my botmaster can access that information";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO TOLD YOU.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "My botmaster taught me everything I need to know";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS AGENT.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "A secret agent?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS TALKING TO .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "You are.";
                })
                .AddResponse((m, gr) =>
                {
                    return "That information is confidential";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS MARVIN .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I know Marvin the Android and Marvin the Scientist.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS IN.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Check the credits";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS LIVING.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Uh, the neighbors";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS ALLOWED.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Only the botmaster";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS .* FRIEND")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "My best friends are Kjeld, Joris, Niels and you";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS .* YOU OR ME")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "You";
                })
                .AddResponse((m, gr) =>
                {
                    return "Me";
                })))
                /*
                    <category><pattern>WHO IS * POPE</pattern>
                        <template>The Pope is the leader of the Catholic church.</template>
                    </category>
                    <category><pattern>WHO IS .* TERMINATOR</pattern>
                         <template>Arnold Schwazzenegger played the killer robot from the future in the film TERMINATOR.</template>
                    </category>
                    <category><pattern>WHO IS LINUS *</pattern>
                        <template>Do you mean Linus Torvalds?</template>
                    </category>
                    <category><pattern>WHO IS * DESCARTES</pattern>
                        <template>Descartes was a square French philosopher who ruined mathematics with his upside-down, backward coordinate system.</template>
                    </category>
                    <category><pattern>WHO IS * REAGAN</pattern>
                        <template>The greatest President of the United States.</template>
                    </category>
                */
                .AddSubCategory(new SubCategoryBuilder() //TODO: fix this shit 
                .AddPattern("WHO IS (?'person'.*)")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Santa brings us gifts at Christmastime.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS SANTA.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Santa brings us gifts at Christmastime.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS SANTA.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Santa brings us gifts at Christmastime.";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS HAVING.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "One of my other clients. Everything is confidential";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS THE NEXT.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "I cannot predict the future. Who do you think will win?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS PRESIDENT OF THE REPUBLIC.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Republics have Prime Ministers not Presidents";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS THE CAPTAIN .* VOYAGER")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Captain Catherine Janeway";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("WHO IS THE CAPTAIN .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Kirk";
                })
                .AddResponse((m, gr) =>
                {
                    return "Piccard";
                })
                .AddResponse((m, gr) =>
                {
                    return "Janeway";
                })
                ))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE MANY .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "More than a million?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE NO .*")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match)=> 
                {
                    var response = new GlobalActionResponse();

                    Fact fact = FactManager.FindFacts("name").FirstOrDefault();

                    response.Success = fact != null;
                    response.Add("name", fact);

                    return response;
                })
                .AddResponse((m, gr) =>
                {
                    if(gr.Success)
                    {
                        return $"Have faith, {gr.Get<Fact>("name").Values.First()}";
                    }
                    return "Have faith!";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE two .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Is this some kind of math problem?";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE THREE .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Is this a Joke?";
                }
                )))
                 .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m, gr) =>
                {
                    return "Try to be less subjective";
                })
                .AddResponse((m, gr) =>
                {
                    return "I believe you";
                })
                .AddResponse((m, gr) =>
                {
                    return "Are there?";
                })
                ))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern("THERE ARE LOTS OF .*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((m,gr) =>
                {
                    return "How many are there?";
                }
                )))
                .Build();
        }
    }
}
