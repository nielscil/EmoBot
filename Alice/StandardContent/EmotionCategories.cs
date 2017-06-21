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

namespace Alice.StandardContent
{
    internal class EmotionCategories : ICategoryCollection
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
                        return $"{GetTimeOfDay()}, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
                    }
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    if (globalResponse.IsEmpty())
                    {
                        return "Whoah, did I do something wrong?";
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
                    return $"What's the problem, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}?";
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
                        return $"{GetTimeOfDay()}, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}";
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
                    return $"What's the problem, {globalResponse.Get<Fact>("name").Values.FirstOrDefault().Transform(To.TitleCase)}?";
                })
                )).Build();

            yield return new CategoryBuilder().AddPattern(@".*I'm feeling happy.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@".*I'm feeling happy.*")
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    // Get happy facts?

                    return response;
                })
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Really? I can't see it";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"That's nice to hear! What makes you happy?";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"You aren't looking really happy";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"What makes you so happy then?";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    // Get some happy facts or something?
                    return $"You don't seem so happy";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"You are?";
                })
                )).Build();

            // check for previous question is 'why u happy'
            yield return new CategoryBuilder().AddPattern(@".*Because.*")
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Thanks, I'll note that";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"That's so nice to hear! Do you have more examples?";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"Are you sure?";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"You don't seem happy about that.";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"Whoah, nice to hear!";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    // Get some happy facts or something?
                    return $"You don't seem so happy";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"You are?";
                })
                )).Build();

            // check for previous question is 'why u sad'
            yield return new CategoryBuilder().AddPattern(@".*Because.*")
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"You seem pretty happy about it actually. Is it really making you sad?";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"Does it upsets you a lot?";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"And that's something you're sad about or fear?";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"And why are you suprised by that?";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"I'm sorry to hear that. What can cheer you up?";
                })
                )).Build();

            // check for previous response is 'what can cheer you up?'
            yield return new CategoryBuilder().AddPattern(@"")
                .AddSubCategory(new SubCategoryBuilder()
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
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"You seem happier already!";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                )).Build();
            
            // Show something about happy stuff
            yield return new CategoryBuilder().AddPattern(@".*cheer me up.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    return response;
                })
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"We can talk about {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}?";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"You seem happy already! But if you want, we can talk about {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}?";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    if (globalResponse.Get<Fact>("happy").Values.Length == 0)
                    {
                        return $"I don't know what makes you happy yet. Can you give me an example?";
                    }
                    return $"I remember you like {globalResponse.Get<Fact>("happy").Values[random.Next(0, FactManager.FindFacts("happy").Capacity - 1)].Transform(To.TitleCase)}, correct?";
                })
                )).Build();

            // check for previous response is 'talk about happy fact?'
            // Get index number of happy fact list item
            // nog niet af want ideeën zijn op
            yield return new CategoryBuilder().AddPattern(@".*yes.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "happy");
                    Fact happy = FactManager.FindFacts("happy").FirstOrDefault();
                    FactManager.AddFact(new Fact("happy", name));
                    return response;
                })
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"You seem happier already!";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                )).Build();

            // Get index number of happy fact list item
            yield return new CategoryBuilder().AddPattern(@".*joke.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddTemplate(new EmotionTemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string name = RegexHelper.GetValue(match, "happy");
                    Fact happy = FactManager.FindFacts("happy").FirstOrDefault();
                    FactManager.AddFact(new Fact("happy", name));
                    return response;
                })
                .AddResponse(EmotionEnum.Neutral, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Happy, (match, globalResponse) =>
                {
                    return $"You seem happier already!";
                })
                .AddResponse(EmotionEnum.Anger, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Fear, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Suprise, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Sad, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
                })
                .AddResponse(EmotionEnum.Disgust, (match, globalResponse) =>
                {
                    return $"Thanks, I'll remember that.";
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

