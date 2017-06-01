using Alice.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class CategoryBuilder
    {
        private Category _category;

        public CategoryBuilder()
        {
            _category = new Category();
        }

        public CategoryBuilder AddPattern(string pattern)
        {
            _category.AddPattern(pattern);
            return this;
        }

        public CategoryBuilder AddSubCategory(SubCategoryBuilder builder)
        {
            _category.AddSubCategory(builder.Build());
            return this;
        }

        public Category Build()
        {
            return _category;
        }
    }
}
