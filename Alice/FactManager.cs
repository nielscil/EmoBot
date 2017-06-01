using Alice.Classes;
using Alice.Models;
using Alice.Models.Conditions;
using Alice.Models.Facts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice
{
    //TODO: think of way to save the facts!
    internal static class FactManager
    {   
        public static List<Fact> Facts { get; private set; }

        public static void LoadFacts(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Facts = LoadFactFile(path).Facts;
                }
                catch
                {
                    throw new Exception("Could not parse file");
                }

            }
            else
            {
                throw new Exception("Could not found facts file or directory");
            }

        }

        public static bool EvaluateFact(string name,params string[] values)
        {
            Fact fact = FindFact(name, values);

            if(fact != null)
            {
                return fact.Evaluate();
            }

            return false;
        }

        private static Fact FindFact(string name,params string[] values)
        {
            foreach(var fact in Facts)
            {
                if(fact.Name == name && fact.HasValues(values))
                {
                    return fact;
                }
            }
            return null;
        }

        private static SerializedFacts LoadFactFile(string file)
        {
            return LoadFactFile(new FileInfo(file));
        }

        private static SerializedFacts LoadFactFile(FileInfo info)
        {
            if (info != null && info.Extension.ToLower() == ".json")
            {
                using (var stream = info.OpenRead())
                using (var streamReader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<SerializedFacts>(streamReader.ReadToEnd());
                }
            }

            throw new Exception("File is not a json file");
        }
    }    
}
