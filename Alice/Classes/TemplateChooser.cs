using Alice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public static class TemplateChooser
    {
        private static Random _random = new Random();

        public static Template ChooseTemplate(List<Template> templates)
        {
            return templates[GetIndex(templates)];
        }

        private static int GetIndex(List<Template> templates)
        {
            int index = 0;
            if (templates.Count > 1)
            {
                index = _random.Next(0, templates.Count);
            }
            return index;
        }

    }
}
