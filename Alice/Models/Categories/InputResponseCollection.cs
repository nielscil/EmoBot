using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public interface InputResponseCollection
    {
        IEnumerable<InputResponse> GetInputResponses();
    }
}
