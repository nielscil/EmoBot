using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Conditions
{
    public class NotCondition : ICondition
    {
        [JsonProperty("condition")]
        public ICondition Condition { get; set; }

        public NotCondition() { }

        public NotCondition(ICondition condition)
        {
            Condition = condition;
        }

        public bool Evaluate()
        {
            return !Condition.Evaluate();
        }
    }
}
