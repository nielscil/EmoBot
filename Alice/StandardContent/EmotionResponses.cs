using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Classes;
using Alice.Models.Facts;
using EmotionLib.Models;
using Humanizer;
using Alice.Models.InputResponses;

namespace Alice.StandardContent
{
    internal class EmotionResponses : IInputResponseCollection
    {
        private Random random = new Random();
        private string GetTimeOfDay()
        {
            if (DateTime.Now.TimeOfDay.Hours <= 6)
            {
                return "Good night ";
            }
            else if (DateTime.Now.TimeOfDay.Hours > 6 && DateTime.Now.TimeOfDay.Hours < 12)
            {
                return "Good morning ";
            }
            else if (DateTime.Now.TimeOfDay.Hours > 12 && DateTime.Now.TimeOfDay.Hours <= 18)
            {
                return "Good afternoon ";
            }
            else
            {
                return "Good evening ";
            }
        }

        public IEnumerable<InputResponse> GetInputResponses()
        {
            yield return new InputResponseBuilder()
                .AddPattern(@".*hello.*")
                .AddPattern(@".*hi.*")
                .AddPattern(@".*hey.*")
                .AddPattern(@".*hallo.*")
                .AddPattern(@".*wazzaaap.*")
                .AddPattern(@".*hé!.*")
                .AddPattern(@".*hoi.*")
                .AddPattern(@".*plaklona.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Well hi there! What is your name?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"You look happy today! What is your name?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Uhmm, what is your name?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Hi there, I'm Alice. What is your name?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Well hello there! What is your name?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"Don't be so sad. What is your name?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Hi there. What is your name?";
                })).Build();
            
            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $".*what is your name?")
                //Kan dit allemaal veel simpeler door .AddPreviousResponse(0, $".*name?") misschien?
                .AddPattern(@"it's (?'name'.*)")
                .AddPattern(@"i'm (?'name'.*)")
                .AddPattern(@"i am (?'name'.*)")
                .AddPattern(@"my name is (?'name'.*)")
                .AddPattern(@"(?'name'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "name");
                    //check facts
                    response.Add("newUser", true);
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    else
                    {
                        return $"{GetTimeOfDay()}, {i.GlobalActionResponse.Get<string>("name").Transform(To.TitleCase)}. How are you feeling today?";
                    }
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    else
                    {
                        return $"{GetTimeOfDay()}, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}. Good to see you again! You look very happy, why is that?";
                    }
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    return $"{i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}. You look kind of angry. What's wrong?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    return $"Relax {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, it's just me";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    return $"Whoah {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, relax, it's me.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    return $"Hi {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, what's with the long face";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    if (!i.GlobalActionResponse.Success || i.GlobalActionResponse.Get<bool>("newUser"))
                    {
                        return $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?";
                    }
                    return $"What's the problem, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}?";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $"{GetTimeOfDay()}, I don't think we've met before. Is that correct?")
                .AddPattern(@"yes*")
                .AddPattern(@"correct*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    var prevAction = match.PreviousResponseData[0].GlobalActionResponse;
                    if(prevAction.Success && prevAction.Get<bool>("newUser"))
                    {
                        //voeg toe
                    }
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Okay, I don’t know anything about you yet. Can you name something you like?";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $"Okay, I don’t know anything about you yet. Can you name something you like?")
                .AddPattern(@"i like (?'happy'.*)")
                .AddPattern(@"yes, (?'name'.*)")
                .AddPattern(@"(?'happy'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string happy = RegexHelper.GetValue(match, "happy");
                    FactManager.AddFact(new Fact("happy", happy));
                    response.Add("happy", happy);
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"You do seem quite happy about it! Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Relax, I won't bite. Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"You don't have to be sad when you talk about things you like. Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Can you give me a specific example why you like {i.GlobalActionResponse.Get<string>("happy")}?";
                })).Build();

            // weather API
            yield return new InputResponseBuilder()
                .AddPattern(@".*weather")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"In which city?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"In which city?";
                })).Build();

            yield return new InputResponseBuilder()
               .AddPreviousResponse(0, $"In which city?")
               .AddPattern(@"In (?'city'.*)")
               .AddPattern(@".* (?'city'.*) for example")
               .AddPattern(@"(?'city'.*)")
               .AddTemplate(new EmotionTemplateBuilder()
               .SetGlobalTemplateAction((match) =>
               {
                   var response = new GlobalActionResponse();
                   string city = RegexHelper.GetValue(match, "city");
                   response.Add("city", city);
                   response.Add("temperature", WeatherAPI.GetTempature(city));

                   return response;
               })
               .AddResponse(Emotion.Neutral, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Happy, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Anger, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Fear, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Suprise, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Sad, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })
               .AddResponse(Emotion.Disgust, (i) =>
               {
                   return $"The weather in " + i.GlobalActionResponse.Get<string>("city") + " is " + i.GlobalActionResponse.Get<float>("temperature") + ". Do you like that kind of temperature?";
               })).Build();

            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $".*do you like.* ?")
                .AddPattern(@"yes*")
                .AddPattern(@"yeah")
                .AddPattern(@"correct")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Really? I can't see it";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"That's nice to hear!";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"What makes you so happy then?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"You don’t seem very happy about it";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"You are?";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPattern(@"How do you know.*")
                .AddPattern(@"yeah")
                .AddPattern(@"correct")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"I know a lot of things";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"Well, you happy individual, I know a lot of things";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Relax, but I know a lot of things";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Relax, but I know a lot of things";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Don’t be suprised, I can see you.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    // Get some happy facts or something?
                    return $"I know a lot of things";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"You are?";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $".*i can see you.*")
                .AddPattern($"you do?")
                .AddPattern($"really?")
                .AddPattern($".*what?")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Yes, but don’t worry, I wont capture images of you.";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPattern(@".*I'm feeling happy.*")
                .AddPattern(@".*I'm happy.*")
                .AddPattern(@".*I am happy.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    // Get happy facts?

                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Really? I can't see it";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"That's nice to hear! What makes you happy?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"What makes you so happy then?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    // Get some happy facts or something?
                    return $"You don't seem so happy";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"You are?";
                })).Build();
            
            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $"That's nice to hear! What makes you happy?")
                .AddPreviousResponse(0, $"What makes you so happy then?")
                .AddPattern(@".* because i am (?'happy'.*)")
                .AddPattern(@".* because i'm (?'happy'.*)")
                .AddPattern(@".* because (?'happy'.*)")
                .AddPattern(@"(?'happy'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "happy");
                    Fact happy = FactManager.FindFacts("happy").FirstOrDefault();
                    FactManager.AddFact(new Fact("happy", name));
                    response.Add("happy", happy);
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Thanks, I'll note that";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"That's so nice to hear! I'll note that.";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Are you sure? You don't seem happy about it, but I'll note that";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"You don't seem happy about that, but I'll note that";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Whoah, nice to hear! I'll note that";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    // Get some happy facts or something?
                    return $"You don't seem so happy, but I'll note that.";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"I can't really see it, but I'll note that";
                })).Build();

            // check for previous question is 'why u sad'
            yield return new InputResponseBuilder()
                .AddPattern(@"Because (?'sad'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "sad");
                    Fact happy = FactManager.FindFacts("sad").FirstOrDefault();
                    FactManager.AddFact(new Fact("sad", name));
                    response.Add("sad", happy);
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"You seem pretty happy about it actually. Is it really making you sad?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Does it upsets you a lot?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"And that's something you're sad about or fear?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"And why are you suprised by that?";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })).Build();

            // check for previous response is 'what can cheer you up?'
            yield return new InputResponseBuilder()
                .AddPreviousResponse(0, $"What can cheer you up?")
                .AddPattern(@"(?'happy'.*)")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "happy");
                    Fact happy = FactManager.FindFacts("happy").FirstOrDefault();
                    FactManager.AddFact(new Fact("happy", name));
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"You seem happier already!";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })).Build();

            // Show something about happy stuff
            yield return new InputResponseBuilder()
                .AddPattern(@".*cheer me up.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    //TODO: get happy facts??
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"We can talk about {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"You seem happy already! But if you want, we can talk about {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    if (i.GlobalActionResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {i.GlobalActionResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })).Build();

            // Get index number of happy fact list item
            yield return new InputResponseBuilder()
                .AddPattern(@".*joke.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "happy");
                    Fact happy = FactManager.FindFacts("happy").FirstOrDefault();
                    FactManager.AddFact(new Fact("happy", name));
                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    return $"You seem happier already!";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    return $"Thanks, I'll remember that.";
                })).Build();
        }
    }
}