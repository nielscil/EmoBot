using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Alice
{
    public class InputController
    {
        public Facts facts;
        Categories categories;

        public InputController(string directory, string factsFile)
        {
            using(var stream = new FileStream(directory + "\\" + factsFile ,FileMode.Open))
            using (var streamReader = new StreamReader(stream))
            {
                facts = JsonConvert.DeserializeObject<Facts>(streamReader.ReadToEnd());
                facts.setLists();
            }

            categories = new Categories(this);
        }

        public void getResponse(string input)
        {
            categories.isMatch(input);
        }
    }
}
