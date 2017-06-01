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
    public static class FactManager
    {
        private static List<Fact> Facts { get; set; } = new List<Fact>();
        private static string _path;

        public static void AddFact(Fact fact)
        {
            Fact foundFact = FindFact(fact.Name, fact.Values);

            if(foundFact != null)
            {
                foundFact.Condition = fact.Condition;
            }
            else
            {
                Facts.Add(fact);
            }
        }

        public static void RemoveFacts(string name)
        {
            for(int i = 0; i < Facts.Count;)
            {
                if(Facts[i].Name == name)
                {
                    Facts.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public static List<Fact> FindFacts(string name)
        {
            List<Fact> facts = new List<Fact>();

            foreach(var fact in Facts)
            {
                if(fact.Name == name)
                {
                    facts.Add(fact);
                }
            }

            return facts;
        }

        internal static void LoadFacts(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Facts = LoadFactFile(path).Facts;
                    _path = path;
                }
                catch(Exception ex)
                {
                    throw new Exception("Could not parse file",ex);
                }

            }
            else
            {
                throw new Exception("Could not found facts file or directory");
            }

        }

        internal static bool EvaluateFact(string name,params string[] values)
        {
            Fact fact = FindFact(name, values);

            if(fact != null)
            {
                return fact.Evaluate();
            }

            return false;
        }

        internal static void SaveFacts()
        {
            if(!string.IsNullOrWhiteSpace(_path))
            {
                using (var stream = new FileStream(_path, FileMode.Create))
                using (var streamwriter = new StreamWriter(stream))
                {
                    streamwriter.Write(Serialize());
                }
            }
        }

        private static string Serialize()
        {
            SerializedFacts facts = new SerializedFacts(Facts);
            return JsonConvert.SerializeObject(facts,Formatting.Indented,new JsonSerializerSettings() {
                TypeNameHandling = TypeNameHandling.Auto
            });
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
                    SerializedFacts facts = JsonConvert.DeserializeObject<SerializedFacts>(streamReader.ReadToEnd(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    if(facts == null)
                    {
                        facts = new SerializedFacts();
                    }

                    return facts;
                }
            }

            throw new Exception("File is not a json file");
        }
    }    
}
