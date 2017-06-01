using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public delegate bool Template(Match match, out string response);
}
