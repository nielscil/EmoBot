using Alice.Models.Facts;
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

        public bool Evaluate(params string[] values)
        {
            FillInBlanks(values);
            return FactManager.EvaluateFact(FactName, Values);
        }

        private void FillInBlanks(params string[] values)
        {
            int valCount = 0;

            if (FactName == null && values.Length > 0)
            {
                values[0] = values[0].Trim();
                if(values[0].StartsWith("a "))
                {
                    values[0] = values[0].Remove(0, 2);
                }
                else if(values[0].StartsWith("an "))
                {
                    values[0] = values[0].Remove(0, 3);
                }

                FactName = values[0];
                valCount++;
            }

            for (int i = 0; i < Values.Length; i++)
            {
                if (valCount == values.Length)
                {
                    break;
                }

                if (Values[i] == null)
                {
                    Values[i] = values[valCount];
                    valCount++;
                }
            }
        }
    }
}
