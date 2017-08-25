
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.view
{
    public class Option
    {
        public string name;
        public Action<Object> action;

        public Option(string name, Action<Object> action)
        {
            this.name = name;
            this.action = action;
        }
    }
}
