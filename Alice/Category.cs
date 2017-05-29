using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Alice
{
    public delegate void Execute(string template, InputController IController);

    public class Categories
    {
        public List<Category> categories;
        public InputController IController;

        public Categories(InputController IControl)
        {
            IController = IControl;
            categories = new List<Category>();
            createCategories();
        }

        public void createCategories()
        {
            /*
            Define all categories here that the AI can recognise
            
            Example:

            |------------------------------------------------------|
            |                                                      |
            |  * Category:                                         |
            |      - gobal pattern: happy                          |
            |                                                      |
            |      * Sub-Category:                                 |
            |          - detailed pattern: is kjeld happy          |
            |          - detailed pattern: is Niels happy          |
            |                                                      |
            |------------------------------------------------------|
            */
            Category cat = new Category(this);
            cat.addPattern("happy");
            
            SubCategory subcat = new SubCategory(cat);
            subcat.addPattern("is niels happy", "niels is ", (t, ic) =>
            {
                if (ic.facts.is_happy("niels"))
                    Console.WriteLine(t + "happy");
                else
                    Console.WriteLine(t + "not happy");
            });
            subcat.addPattern("is kjeld happy", "kjeld is ", (t, ic) =>
            {
                if (ic.facts.is_happy("kjeld"))
                    Console.WriteLine(t + "happy");
                else
                    Console.WriteLine(t + "not happy");
            });
            cat.addSubCategory(subcat);
            categories.Add(cat);
        }

        public bool isMatch(string input)
        {
            foreach(Category c in categories)
            {
                if (c.isMatch(input))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Category
    {
        public List<Pattern> patterns;
        public List<SubCategory> subcategories;
        public Categories parent;
        
        public Category(Categories p)
        {
            parent = p;
            patterns = new List<Pattern>();
            subcategories = new List<SubCategory>();
        }

        public void addPattern(string r)
        {
            Pattern p = new Pattern(r);
            patterns.Add(p);
        }
        
        public void addSubCategory(SubCategory cat)
        {
            subcategories.Add(cat);
        }

        public bool isMatch(string input)
        {
            foreach(Pattern p in patterns)
            {
                if (p.isMatch(input))
                {
                    foreach(SubCategory sc in subcategories)
                    {
                        if (sc.isMatch(input))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    public class SubCategory
    {
        public List<SubCatPattern> patterns;
        public Category parent;

        public SubCategory(Category p)
        {
            parent = p;
            patterns = new List<SubCatPattern>();
        }

        public void addPattern(string r, string t, Execute ex)
        {
            SubCatPattern p = new SubCatPattern(this, r, t, ex);
            patterns.Add(p);
        }

        public bool isMatch(string input)
        {
            foreach(SubCatPattern p in patterns)
            {
                if (p.Match(input))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Pattern
    {
        public string regex;
        Regex RX;
        
        public Pattern(string r)
        {
            regex = r;
        }

        public bool isMatch(string input)
        {
            if (RX == null)
                RX = new Regex(regex);

            if (RX.IsMatch(input))
            {
                return true;
            }
            else
                return false;
        }
    }

    public class SubCatPattern
    {
        public string regex;
        public string template;
        Regex RX;
        Execute execute;
        SubCategory parent;

        public SubCatPattern(SubCategory p ,string r, string t, Execute ex)
        {
            parent = p;
            regex = r;
            template = t;
            execute = ex;
        }

        public bool Match(string input)
        {
            if (RX == null)
                RX = new Regex(regex);

            if (RX.IsMatch(input))
            {
                Execute();
                return true;
            }
            else
                return false;
        }

        void Execute()
        {
            execute.Invoke(template, parent.parent.parent.IController);
        }
    }


}
