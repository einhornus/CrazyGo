using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Go
{
    public class Settings
    {
        public int size = 19;
        public static int HIDDEN_MOVES = 7;
        public static int RANDOMIZE = 0;
        public bool handi = true;
        public string time = "b10#15#3";


        public static string Save(Settings settings)
        {
            string res = "";
            res += "hm_count-" + HIDDEN_MOVES + "|";
            res += "board_size-" + settings.size + "|";
            res += "randomize-" + RANDOMIZE + "|";
            res += "handi-" + (settings.handi ?"1":"0") + "|";
            res += "time-" + settings.time;
            return res;
        }

        public bool GetHandi()
        {
            return handi;
        }

        public int getOvertime()
        {
            string[] parts = time.Substring(1).Split('#');
            return int.Parse(parts[1]);
        }

        public int getMainTime()
        {
            string[] parts = time.Substring(1).Split('#');
            return int.Parse(parts[0]);
        }

        public int getPeriods()
        {
            string[] parts = time.Substring(1).Split('#');
            return int.Parse(parts[2]);
        }

        public String GetTimeControl()
        {
            String _minutes = "";
            String _overtime = "";
            int minutes = getMainTime() / 60;
            if(minutes < 10){
                _minutes = "0" + minutes;
            }
            else
            {
                _minutes = minutes + "";
            }

            if(getOvertime() < 10){
                _overtime = "0" + getOvertime();
            }
            else
            {
                _overtime = "" + getOvertime();
            }
            return _minutes + "+" + _overtime + "(" + getPeriods() + ")";
        }

        public static Settings Load(string s)
        {
            string[] pairs = s.Split('|');
            Settings res = new Settings();
            for (int i = 0; i<pairs.Length; i++)
            {
                string[] p = pairs[i].Split('-');
                string fir = p[0];
                string sec = p[1];

                if (fir.Equals("handi"))
                {
                    res.handi = int.Parse(sec)==1;
                }

                if (fir.Equals("board_size"))
                {
                    res.size = int.Parse(sec);
                }

                if (fir.Equals("time"))
                {
                    res.time = sec;
                }
            }
            return res;
        }
    }
}
