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

        public Fact(string name, ICondition condition, params string[] values)
        {
            Name = name;
            Condition = condition;
            Values = values;
        }

        public Fact(string name, params string[] values)
        {
            Name = name;
            Values = values;
        }

        public bool Evaluate(params string[] values)
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

        public bool CheckGivenValues(params string[] values)
        {
            if(Values.Length >= values.Length)
            {
                for(int i = 0; i < values.Length; i++)
                {
                    if(values[i] != null && Values[i] != values[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public string[] GetSpecificValues(params string[] values)
        {
            if(values.Length <= Values.Length)
            {
                List<string> strings = new List<string>();

                for(int i= 0; i< values.Length; i++)
                {
                    if(values[i] == null)
                    {
                        strings.Add(Values[i]);
                    }
                }

                return strings.ToArray();
            }
            return new string[0];
        }
    }
}
