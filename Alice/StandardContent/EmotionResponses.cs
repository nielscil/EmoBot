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
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    Fact name = FactManager.FindFacts("name").FirstOrDefault();
                    response.Add("name", name);

                    return response;
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    //TODO: check if name is set
                    return $"Hello {i.GlobalActionResponse.Get("name")}, what can you do?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Do you look happy today! What is your name?";
                    }
                    else
                    {
                        return $"{GetTimeOfDay()}, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                    }
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Whoah, did I do something wrong?";
                    }
                    return $"{i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I feel oncomfortable with your anger. What's wrong?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Don't be scared, I won't bite. What is your name?";
                    }
                    return $"Relax {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, it's just me";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Whoah! I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Whoah {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, relax, it's me.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Hello, I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Hi {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I remember you more beautifull with a smile on your face";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "You look disgusted, do I have someting between my teeth?";
                    }
                    return $"What's the problem, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}?";
                })).Build();

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
                })
                .AddResponse(Emotion.Neutral, (i) =>
                {
                    return $"Hello {i.GlobalActionResponse.Get("name")}, what can you do?";
                })
                .AddResponse(Emotion.Happy, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Do you look happy today! What is your name?";
                    }
                    else
                    {
                        return $"{GetTimeOfDay()}, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                    }
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Whoah, did I say something wrong?";
                    }
                    return $"{i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I feel uncomfortable with your anger. What's wrong?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Don't be scared, I won't bite. What is your name?";
                    }
                    return $"Relax {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, it's just me";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Whoah! I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Whoah {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, relax, it's me.";
                })
                .AddResponse(Emotion.Sad, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "Hello, I believe this is the first time we meet. I think it's best to start with a smile";
                    }
                    return $"Hi {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}, I remember you more beautiful with a smile on your face";
                })
                .AddResponse(Emotion.Disgust, (i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return "You look disgusted, do I have someting between my teeth?";
                    }
                    return $"What's the problem, {i.GlobalActionResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}?";
                })).Build();

            yield return new InputResponseBuilder()
                .AddPattern(@".*I'm feeling happy.*")
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

            // check for previous question is 'why u happy'
            yield return new InputResponseBuilder()
                .AddPattern(@"Because (?'happy'.*)")
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
                    return $"That's so nice to hear! Do you have more examples?";
                })
                .AddResponse(Emotion.Anger, (i) =>
                {
                    return $"Are you sure?";
                })
                .AddResponse(Emotion.Fear, (i) =>
                {
                    return $"You don't seem happy about that.";
                })
                .AddResponse(Emotion.Suprise, (i) =>
                {
                    return $"Whoah, nice to hear!";
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

            // check for previous response is 'talk about happy fact?'
            // Get index number of happy fact list item
            // nog niet af want ideeën zijn op
            yield return new InputResponseBuilder()
                .AddPattern(@".*yes.*")
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

