using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models
{
    public interface IResponseFinder
    {
        string Input { get; }
        string Response { get; set; }
        bool Found { get; }
    }
}
