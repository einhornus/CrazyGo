
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.web.requests
{
    class GoodResponse : Response
    {
        private string text;
        public GoodResponse(string text)
        {
            this.text = text;
        }

        public override string ToString()
        {
            return text;
        }

        public string GetText()
        {
            return text;
        }
    }
}
