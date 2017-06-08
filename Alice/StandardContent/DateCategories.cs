using Alice.Classes;
using Alice.Models.Categories;
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
    internal class DateCategories : ICategoryCollection
    {
        private CultureInfo Info
        {
            get
            {
                return CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
            }
        }

        private readonly string[] parsableDates = new string[] { "MMMM" , "dd-MM-yyyy", "dd-MM", "d MMMM", "dd MMMM", "dd MMMM yyyy", "MMMM yyyy" }; //TODO: add more formats?

        public IEnumerable<Category> GetCategories()
        {
            yield return new CategoryBuilder()
                .AddPattern(".*date.*")
                .AddPattern(".*time.*")
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*date and time.*")
                .AddPattern(".*time and date.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It's: {DateTime.Now.ToOrdinalWords()} {DateTime.Now.ToString("HH:mm")}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
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
                .AddResponse((match, gr) =>
                {
                    if (!gr.Success)
                    {
                        return $"I can't calculate the date in {gr.Get<string>("days")} days";
                    }
                    string signular = gr.Get<bool>("singular") ? "day" : "days";

                    return $"In {gr.Get<string>("days")} {signular} it's {gr.Get<DateTime>("date").ToString("dd-MM-yyyy")}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*date tomorrow*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It's tomorrow {DateTime.Now.AddDays(1).ToOrdinalWords()}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*date yesterday*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It was yesterday {DateTime.Now.AddDays(-1).ToOrdinalWords()}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*date day after tomorrow*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It's the day after tomorrow {DateTime.Now.AddDays(2).ToOrdinalWords()}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*date.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It's today {DateTime.Now.ToOrdinalWords()}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*time.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"It's: {DateTime.Now.ToString("HH:mm:ss")}";
                })))
                .Build();
            yield return new CategoryBuilder()
                .AddPattern(".*day.*")
                .AddPattern(".*tomorrow.*")
                .AddPattern(".*yesterday.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(@".*days until (?'date'.*)")
                .AddTemplate(new TemplateBuilder()
                .SetGlobalTemplateAction((match) =>
                {
                    var response = new GlobalActionResponse();
                    DateTime date;

                    if(DateTime.TryParseExact(RegexHelper.GetValue(match,"date"), parsableDates, Info, DateTimeStyles.None, out date))
                    {
                        response.Add("diff", date - DateTime.Now);
                    }
                    else // maybe its a fact?
                    {
                        Fact fact = FactManager.FindFacts(RegexHelper.GetValue(match, "date")).FirstOrDefault();

                        if(fact != null && DateTime.TryParseExact(fact.Values.First(), parsableDates, Info, DateTimeStyles.None, out date))
                        {
                            response.Add("diff", date - DateTime.Now);
                        }
                    }
                    return response;
                })
                .AddResponse((match,gr) =>
                {
                    if(gr.Empty)
                    {
                        return $"I can't calculate the days until {RegexHelper.GetValue(match, "date")}";
                    }
                    return $"There are {gr.Get<TimeSpan>("diff").Days} days till {RegexHelper.GetValue(match, "date")}";
                }
                )))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(".*day[^s].*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"Today it is {DateTime.Now.ToString("dddd")}";
                }
                )))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*tomorrow.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"Tomorrow it is {DateTime.Now.Date.AddDays(1).ToString("dddd")}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*yesterday.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"Yesterday it was {DateTime.Now.Date.AddDays(-1).ToString("dddd")}";
                })))
                .Build();
            yield return new CategoryBuilder()
                .AddPattern(".*year.*")
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(".*next year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match,gr) =>
                {
                    return $"Next year it is {DateTime.Now.Year + 1}";
                })))
                .AddSubCategory(new SubCategoryBuilder()
                .AddPattern(".*last year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, gr) =>
                {
                    return $"Last year it was {DateTime.Now.Year - 1}";
                })))
                .AddSubCategory(
                new SubCategoryBuilder()
                .AddPattern(".*year.*")
                .AddTemplate(new TemplateBuilder()
                .AddResponse((match, globalResponse) =>
                {
                    return $"This is {DateTime.Now.ToString("yyyy")}";
                })))
                .Build();
        }
    }
}
