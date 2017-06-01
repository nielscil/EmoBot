using Alice.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public class SubCategory : CategoryBase
    {
        private List<Template> _templates = new List<Template>();

        public void AddTemplate(Template template)
        {
            _templates.Add(template);
        }

        public override void Accept(IResponseFinder finder)
        {
            Match match;
            if(IsMatchingPatterns(finder.Input, out match))
            {
                ExecuteTemplate(finder, match);
            }
        }

        private void ExecuteTemplate(IResponseFinder finder, Match match)
        {
            string response = TemplateChooser.ChooseTemplate(_templates).Invoke(match);

            if (string.IsNullOrWhiteSpace(response))
            {
                response = string.Empty;
            }

            finder.Response = response;
        }
    }
}
