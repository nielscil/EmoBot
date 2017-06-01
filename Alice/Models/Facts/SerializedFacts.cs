using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.Facts
{
    public class SerializedFacts
    {
        [JsonProperty("facts")]
        public List<Fact> Facts { get; set; }

        internal SerializedFacts()
        {
            Facts = new List<Fact>();
        }
    }
}
