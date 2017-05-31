using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Alice
{
    //public delegate string Execute(string template, Match match);

    //public class Categories
    //{
    //    public List<Category> categories;

    //    public Categories()
    //    {
    //        categories = new List<Category>();
    //        CreateCategories();
    //    }

    //    public void CreateCategories()
    //    {
    //        /*
    //        Define all categories here that the AI can recognise
            
    //        Example:

    //        |------------------------------------------------------|
    //        |                                                      |
    //        |  * Category:                                         |
    //        |      - gobal pattern: happy                          |
    //        |                                                      |
    //        |      * Sub-Category:                                 |
    //        |          - detailed pattern: is kjeld happy          |
    //        |          - detailed pattern: is Niels happy          |
    //        |                                                      |
    //        |------------------------------------------------------|
            
    //        Category cat = new Category(this);
    //        cat.addPattern("happy");
            
    //        SubCategory subcat = new SubCategory(cat);
    //        subcat.addPattern("is niels happy", "niels is ", (t, ic) =>
    //        {
    //            if (ic.facts.is_happy("niels"))
    //                Console.WriteLine(t + "happy");
    //            else
    //                Console.WriteLine(t + "not happy");
    //        });
    //        subcat.addPattern("is kjeld happy", "kjeld is ", (t, ic) =>
    //        {
    //            if (ic.facts.is_happy("kjeld"))
    //                Console.WriteLine(t + "happy");
    //            else
    //                Console.WriteLine(t + "not happy");
    //        });
    //        cat.addSubCategory(subcat);
    //        categories.Add(cat);
    //        */

    //    }

    //    public bool isMatch(string input, out string response)
    //    {
    //        response = null;
    //        foreach(Category c in categories)
    //        {
    //            if (c.isMatch(input,out response))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}

    //public class Category
    //{
    //    public List<Pattern> patterns;
    //    public List<SubCategory> subcategories;
        
    //    public Category()
    //    {
    //        patterns = new List<Pattern>();
    //        subcategories = new List<SubCategory>();
    //    }

    //    public void addPattern(string r)
    //    {
    //        Pattern p = new Pattern(r);
    //        patterns.Add(p);
    //    }
        
    //    public void addSubCategory(SubCategory cat)
    //    {
    //        subcategories.Add(cat);
    //    }

    //    public bool isMatch(string input, out string response)
    //    {
    //        response = null;
    //        foreach(Pattern p in patterns)
    //        {
    //            if (p.isMatch(input))
    //            {
    //                foreach(SubCategory sc in subcategories)
    //                {
    //                    if (sc.isMatch(input, out response))
    //                    { 
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //        return false;
    //    }
    //}

    //public class SubCategory : Category
    //{
    //    //public List<SubCatPattern> patterns;

    //    public SubCategory()
    //    {
    //        //patterns = new List<SubCatPattern>();
    //    }

    //    public void addPattern(string r, string t, Execute ex = null)
    //    {
    //        SubCatPattern p = new SubCatPattern(r, t, ex);
    //        patterns.Add(p);
    //    }

    //    public bool isMatch(string input, out string response)
    //    {
    //        response = null;
    //        foreach(SubCatPattern p in patterns)
    //        {
    //            if (p.Match(input, out response))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}

    //public class Pattern
    //{
    //    public string RegexPattern { get; }
        
    //    public Pattern(string pattern)
    //    {
    //        RegexPattern = pattern;
    //    }

    //    public bool isMatch(string input)
    //    {
    //        return Regex.IsMatch(input, RegexPattern);
    //    }
    //}

    //public class SubCatPattern : Pattern
    //{
    //    public string Template { get; private set; }
    //    private Execute _execute;

    //    public SubCatPattern(string pattern, string template, Execute execute) : base(pattern)
    //    {
    //        Template = template;
    //        _execute = execute;
    //    }

    //    public bool Match(string input, out string response)
    //    {
    //        response = null;

    //        Match match = Regex.Match(input, RegexPattern);

    //        if(match.Success)
    //        {
    //            response = Execute(match);
    //        }

    //        return match.Success;
    //    }

    //    private string Execute(Match match)
    //    {
    //        string response = Template;

    //        if(_execute != null)
    //        {
    //            response = _execute?.Invoke(Template, match);
    //        }

    //        return response;
    //    }
    //}


}
