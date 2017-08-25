
using System.Collections.Generic; using System;

using System.Text;
using BrainDuelsLib.delegates;

namespace BrainDuelsLib.utils.json
{
    public abstract class JsonAvatar<T>
    {
        public int result_code;

        public abstract void CopyData(T host);

        //[\s][a-z]([A-z]*)\((.*)\)
		public static void HandleNull(Object value, BrainDuelsLib.delegates.Action action){
            if(value.Equals("null")){
                return;
            }
            else{
                action();
            }
        }
    }
}
