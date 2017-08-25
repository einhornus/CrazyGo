
using System.Collections.Generic; using System;

using System.Text;
using BrainDuelsLib.web;


namespace BrainDuelsLib.model.entities
{
    public class Challenge
    {
        public int id { get; set; }
        public int first { get; set; }
        public int second { get; set; }
        public string game { get; set; }
        public string settings { get; set; }

        public override string ToString()
        {
            return "Challenge from "+first+" to "+second+" on game '"+game+"' winth settings "+settings;
        }
    }
}
