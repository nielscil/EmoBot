using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Classes
{
    public class GlobalActionResponse
    {
        public Dictionary<string, object> Values { get; private set; } = new Dictionary<string, object>();

        public void Add(string name, object obj)
        {
            if(obj != null)
            {
                Values.Add(name, obj);
            }
        }

        public bool IsEmpty()
        {
            return Values.Count == 0;
        }

        public object Get(string name)
        {
            object obj;

            if(!Values.TryGetValue(name, out obj))
            {
                obj = null;
            }

            return obj;
        }

        public T Get<T>(string name)
        {
            object obj = Get(name);
            
            if(obj == null)
            {
                return default(T);
            }

            return obj is T ? (T)obj : default(T);
        }
    }
}
