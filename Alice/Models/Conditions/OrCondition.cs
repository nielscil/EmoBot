using Alice.Models.Facts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Conditions
{
    public class OrCondition : ICondition
    {
        [JsonProperty("conditions")]
        public List<ICondition> Conditions { get; set; } = new List<ICondition>();

        public OrCondition() { }
        public OrCondition(params ICondition[] conditions)
        {
            Conditions = conditions.ToList();
        }

        public bool Evaluate(params string[] values)
        {
            foreach (var item in Conditions)
            {
                if (item.Evaluate(values))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
