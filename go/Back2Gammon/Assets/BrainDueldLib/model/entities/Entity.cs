
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.model.entities
{
    public class Entity
    {
        private int id;

        static Dictionary<String, int> lastIdMap = new Dictionary<string, int>();

        int GetId()
        {
            return id;
        }

        public Entity()
        {
            string typeName = this.GetType().Name;
            if (lastIdMap.ContainsKey(typeName))
            {
                int lastId = lastIdMap[typeName];
                int newId = lastId + 1;
                lastIdMap[typeName] = newId;
                id = newId;
            }
            else
            {
                id = 1;
                lastIdMap.Add(typeName, id);
            }
        }
    }
}
