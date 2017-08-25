
using System.Collections.Generic; using System;

using System.Text;
using BrainDuelsLib.utils.json;
using System;
using UnityEngine;

namespace BrainDuelsLib.model.entities
{
    public class User : Entity
    {
        public String login { get; set; }
        public int id { get; set; }
        public int rank { get; set; }


        public User()
        {
        }

        public String GetRankString()
        {
            return GetRankString(rank);
        }

        public static String GetRankString(int val)
        {
            if (val > 0)
            {
                return val + "d";
            }
            else
            {
                int kyu = -val + 1;
                return kyu + "k";
            }
        }
    }
}
