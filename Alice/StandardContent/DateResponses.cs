using Alice.Classes;
using Alice.Models.InputResponses;
using Alice.Models.Facts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace Alice.StandardContent
{
    internal class DateResponses : IInputResponseCollection
    {
        private CultureInfo Info
        {
            get
            {
                return CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
            }
        }

        private readonly string[] parsableDates = new string[] { "MMMM" , "dd-MM-yyyy", "dd-MM", "d MMMM", "dd MMMM", "dd MMMM yyyy", "MMMM yyyy" }; //TODO: add more formats?

        public IEnumerable<InputResponse> GetInputResponses()
        {
            yield return new InputResponseBuilder()
                .AddPattern(".*date and time.*")
                .AddPattern(".*time and date.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It's: {DateTime.Now.ToOrdinalWords()} {DateTime.Now.ToString("HH:mm")}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(@".*date in (?'days'[a-z0-9]*) day(s)?.*")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    string days = RegexHelper.GetValue(match, "days");
                    int result = -1;

                    if (response.Success = (!string.IsNullOrWhiteSpace(days) && int.TryParse(days, NumberStyles.Any, Info, out result)))
                    {
                        response.Add("date", DateTime.Now.AddDays(result));
                        response.Add("singular", result == 1);
                    }
                    response.Add("days", days);

                    return response;
                })
                .AddResponse((i) =>
                {
                    if (!i.GlobalActionResponse.Success)
                    {
                        return $"I can't calculate the date in {i.GlobalActionResponse.Get<string>("days")} days";
                    }
                    string signular = i.GlobalActionResponse.Get<bool>("singular") ? "day" : "days";

                    return $"In {i.GlobalActionResponse.Get<string>("days")} {signular} it's {i.GlobalActionResponse.Get<DateTime>("date").ToString("dd-MM-yyyy")}";
                })).Build();
            yield return new InputResponseBuilder()
                 .AddPattern(".*date tomorrow*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It's tomorrow {DateTime.Now.AddDays(1).ToOrdinalWords()}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*date yesterday*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It was yesterday {DateTime.Now.AddDays(-1).ToOrdinalWords()}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*date day after tomorrow*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It's the day after tomorrow {DateTime.Now.AddDays(2).ToOrdinalWords()}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*date.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It's today {DateTime.Now.ToOrdinalWords()}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*time.*")
                .AddPattern(".*late it is")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"It's: {DateTime.Now.ToString("HH:mm")}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(@".*days until (?'date'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    DateTime dateTime;
                    string date = RegexHelper.GetValue(match, "date");
                    if (DateTime.TryParseExact(date, parsableDates, Info, DateTimeStyles.None, out dateTime))
                    {
                        response.Add("diff", dateTime - DateTime.Now);
                    }
                    else // maybe its a fact?
                    {
                        Fact fact = FactManager.FindFacts(date).FirstOrDefault();

                        if (fact != null && DateTime.TryParseExact(fact.Values.First(), parsableDates, Info, DateTimeStyles.None, out dateTime))
                        {
                            response.Add("diff", dateTime - DateTime.Now);
                        }
                    }
                    response.Add("date", date);
                    return response;
                })
                .AddResponse((i) =>
                {
                    if (i.GlobalActionResponse.Empty)
                    {
                        return $"I can't calculate the days until {i.GlobalActionResponse.Get("date")}";
                    }
                    return $"There are {i.GlobalActionResponse.Get<TimeSpan>("diff").Days} days till {i.GlobalActionResponse.Get("date")}";
                }
                )).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*day[^s].*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"Today it is {DateTime.Now.ToString("dddd")}";
                }
                )).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*tomorrow.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"Tomorrow it is {DateTime.Now.Date.AddDays(1).ToString("dddd")}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*yesterday.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"Yesterday it was {DateTime.Now.Date.AddDays(-1).ToString("dddd")}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*next year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"Next year it is {DateTime.Now.Year + 1}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*last year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"Last year it was {DateTime.Now.Year - 1}";
                })).Build();
            yield return new InputResponseBuilder()
                .AddPattern(".*year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((i) =>
                {
                    return $"This is {DateTime.Now.ToString("yyyy")}";
                })).Build();
        }
    }
}
