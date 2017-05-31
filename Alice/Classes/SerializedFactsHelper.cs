using Alice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public static class SerializedFactsHelper
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
