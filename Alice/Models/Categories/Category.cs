using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public class Category : CategoryBase
    {       
        private List<SubCategory> _subCategories = new List<SubCategory>();

        public void AddSubCategory(SubCategory category)
        {
            _subCategories.Add(category);
        }

        public override void Accept(IResponseFinder finder)
        {
            if(_patterns.Count == 0 || IsMatchingPatterns(finder.Input))
            {
                foreach(var subCategory in _subCategories)
                {
                    if(!finder.Found)
                    {
                        subCategory.Accept(finder);
                    }
                }
            }
        } 
    }
}
