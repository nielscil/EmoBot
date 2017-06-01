using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Conditions
{
    public class Condition : ICondition
    {
        [JsonProperty("fact_name")]
        public string FactName { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }

        public Condition() { }

        public Condition(string name, params string[] values)
        {
            FactName = name;
            Values = values;
        }

        public bool Evaluate()
        {
            return FactManager.EvaluateFact(FactName, Values);
        }
    }
}
