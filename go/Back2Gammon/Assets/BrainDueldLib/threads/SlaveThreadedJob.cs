
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.web;
using System.IO;

//using Newtonsoft.Json;
using BrainDuelsLib.web.requests;


namespace BrainDuelsLib.threads
{
    public class SlaveThreadedJob : SocketThreadedJob
    {
        public SlaveThreadedJob(int gameServer, int interval)
            : base(SocketManager.GAME_SERVERS[gameServer], null)
        {
            this.callback = MessageCallback;
            Write("register_slave ");

            System.Threading.Thread thread = new System.Threading.Thread(delegate()
            { 
                while(true){
                    System.Threading.Thread.Sleep(interval);
                    Send();
                }
            });
            thread.Start();
        }

        public void MessageCallback(string s)
        {
            Message message = new Message(s);
            if(message.header.Equals("terminate")){
                string title = message.content[0];
                string result = message.content[1];
                string users = message.content[2];
                string settings = message.content[3];
                string text = "report_game users=" + users + ";title=" + title + ";settings=" + settings + ";result=" + result;
                BrainDuelsLib.web.socket_requests.DBSocketRequest.Send(text);
            }
        }

        private long lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        

        public void Send()
        {
            Write("time ");
        }
    }
}
