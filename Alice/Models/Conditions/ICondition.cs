using Alice.Models.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Conditions
{
    public interface ICondition
    {
        bool Evaluate(params string[] values);
    }
}
