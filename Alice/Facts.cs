using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice
{
    public delegate bool execute_condition(string f, string v);

    public class Facts
    {
        public List<Fact> facts { get; set; }

        public void setLists()
        {
            foreach(Fact f in facts)
            {
                f.condition.facts = facts;
            }
        }

        public bool is_happy(string value)
        {
            foreach(Fact f in facts)
            {
                if(f.fact == "happy" && f.value == value)
                {
                    return f.getStatus();
                }
            }
            return false;
        }
    }

    public class Fact
    {
        public string fact { get; set; }
        public string value { get; set; }
        public bool status { get; set; }
        public Condition condition { get; set; }

        public bool getStatus()
        {
            if(condition.fact != null && condition.value != null)
            {
                status = condition.execute();
            }

            return status;
        }
    }

    public class Condition
    {
        public List<Fact> facts;

        public string fact { get; set; }
        public string value { get; set; }

        execute_condition ex;

        public Condition()
        {
            ex = (f, v) =>
            {
                for(int i = 0; i < facts.Count; i++)
                {
                    if(facts[i].fact == f && facts[i].value == v)
                    {
                        return facts[i].getStatus();
                    }
                }
                return false;
            };
        }

        public bool execute()
        {
            return ex.Invoke(fact, value);
        }
    }
}
