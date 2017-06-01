using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class SubCategoryBuilder
    {
        private SubCategory _category;

        public SubCategoryBuilder()
        {
            _category = new SubCategory();
        }

        public SubCategoryBuilder AddPattern(string pattern)
        {
            _category.AddPattern(pattern);
            return this;
        }

        public SubCategoryBuilder AddTemplate(Template template)
        {
            _category.AddTemplate(template);
            return this;
        }

        public SubCategory Build()
        {
            return _category;
        }
    }
}
