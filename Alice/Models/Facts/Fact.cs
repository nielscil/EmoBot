using Alice.Models.Conditions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Facts
{
    public class Fact
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }

        [JsonProperty("condition")]
        public ICondition Condition { get; set; }

        public Fact() { }
        public Fact(string name, params string[] values)
        {
            Name = name;
            Values = values;
        }

        public Fact(string name, ICondition condition)
        {
            Name = name;
            Condition = condition;
        }

        public bool Evaluate()
        {
            bool evaluation = true;

            if (Condition != null)
            {
                evaluation = Condition.Evaluate();
            }

            return evaluation;
        }

        public bool HasValues(params string[] values)
        {
            if (Values.Length == values.Length)
            {
                return Values.SequenceEqual(values);
            }

            return false;
        }
    }
}
