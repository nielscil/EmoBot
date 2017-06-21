using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Models.Facts;
using Newtonsoft.Json;

namespace Alice.Models.Conditions
{
    public class DynamicForeachCondition : ICondition
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("values",NullValueHandling =NullValueHandling.Include)]
        public string[] Values { get; private set; }

        [JsonProperty("condition")]
        public ICondition Condition { get; private set; }

        public DynamicForeachCondition(string name,ICondition condition,params string[] values)
        {
            Name = name;
            Values = values;
            Condition = condition;
        }

        public DynamicForeachCondition() { }

        public bool Evaluate(params string[] values)
        {
            List<Fact> facts = FactManager.FindFactsWithGivenValues(Name, false, Values);

            if (facts.Count == 0)
            {
                return false;
            }

            foreach(var fact in facts)
            {
                string[] val = fact.GetSpecificValues(Values);
                if(!Condition.Evaluate(val))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
