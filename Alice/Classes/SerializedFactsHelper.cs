using Alice.Models;
using Alice.Models.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    internal static class SerializedFactsHelper
    {
        public static SerializedFacts Merge(List<SerializedFacts> mergeData)
        {
            SerializedFacts facts = new SerializedFacts();
            foreach(var serializedFacts in mergeData)
            {
                facts.Facts.AddRange(serializedFacts.Facts);
            }

            return facts;
        }
    }
}
