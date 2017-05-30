using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Alice
{
    public sealed class InputController
    {
        public Facts facts;
        Categories categories;
        private static InputController _instance;


        public static InputController Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("InputController not initialized");
                }
                else
                {
                    return _instance;
                }
            }
        }

        public static void Init(string directory, string factsFile)
        {
            if (_instance != null)
            {
                throw new Exception("Already running");
            }
            _instance = new InputController(directory, factsFile);
        }

        private InputController(string directory, string factsFile)
        {
            using (var stream = new FileStream(directory + "\\" + factsFile, FileMode.Open))
            using (var streamReader = new StreamReader(stream))
            {
                facts = JsonConvert.DeserializeObject<Facts>(streamReader.ReadToEnd());
                facts.setLists();
            }

            categories = new Categories(this);
        }

        public async Task<string> GetResponse(string input)
        {
            return await Task.Run<string>(() =>
            {
                string output = null;
                if(!categories.isMatch(input, out output))
                {
                    output = "\"Huh??!\" - Kjeld";
                }
                return output;
            });
        }
    }
}
