
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.threads
{
    public class Game
    {
        public int id;
        public List<int> users;
        public string title;
        public string settings;
        public int max;
        public string status;
        public int server;

        public Game Copy()
        {
            Game res = new Game();
            res.id = id;
            res.users = new List<int>();
            if (users != null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    res.users.Add(users[i]);
                }
            }
            res.title = title;
            res.settings = settings;
            res.max = max;
            res.status = status;
            res.server = server;
            return res;
        }
    }
}
