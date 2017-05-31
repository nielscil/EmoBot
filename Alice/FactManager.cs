using Alice.Classes;
using Alice.Models;
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
            if(File.Exists(path))
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
            else if(Directory.Exists(path))
            {
                Facts = LoadDirectory(path).Facts;
            }
            else
            {
                throw new Exception("Could not found facts file or directory");
            }

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

        private static SerializedFacts LoadDirectory(string directory)
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            List<SerializedFacts> facts = new List<SerializedFacts>();

            if (info != null)
            {
                foreach(var file in info.GetFiles())
                {
                    if(file.Extension.ToLower() == ".json")
                    {
                        facts.Add(LoadFactFile(file));
                    }
                }
            }

            return SerializedFactsHelper.Merge(facts);
        }
    }

    //TODO: seperate classes into seperate files
    public class Fact
    {
        public string fact { get; set; }
        public string value { get; set; }
        public bool status { get; set; }
        public Condition condition { get; set; }

        public bool getStatus()
        {
            if(condition.Fact != null && condition.Value != null)
            {
                status = condition.execute();
            }

            return status;
        }
    }

    public class Condition
    {
        [JsonProperty("fact")]
        public string Fact { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        public Condition()
        {
        }

        public bool execute()
        {
            foreach(Fact fact in FactManager.Facts)
            {
                if (fact.fact == Fact && fact.value == Value)
                {
                    return fact.getStatus();
                }
            }
            return false;
        }
    }
}
