using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models.InputResponses
{
    public class GlobalActionResponse
    {

        private Dictionary<string, object> _values;
        private ReadOnlyDictionary<string, object> _readOnlyValues;
        public ReadOnlyDictionary<string, object> Values
        {
            get
            {
                return _readOnlyValues;
            }
        }

        public bool Empty
        {
            get
            {
                return Values.Count == 0;
            }
        }

        public bool Success { get; set; } = true;

        public GlobalActionResponse()
        {
            _values = new Dictionary<string, object>();
            _readOnlyValues = new ReadOnlyDictionary<string, object>(_values);
        }

        public void Add(string name, object obj)
        {
            if(obj != null)
            {
                _values.Add(name, obj);
            }
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
