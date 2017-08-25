
using System.Collections.Generic; using System;

using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using BrainDuelsLib.web;
using BrainDuelsLib.web.requests;

namespace BrainDuelsLib.threads
{
    public class SocketThreadedJob : BrainDuelsLib.threads.ThreadedJob
    {

        public class Message{
            public string header;
            public string[] content;
            public static char MAIN_DELIMITER = ' ';
            public static char SIDE_DELIMITER = ';';

            public Message(string line){
                if (line[0] == '!')
                {
                    ErrorResponse er = new ErrorResponse(line);
                    Exception exception = er.ThrowException();
                    throw exception;
                }

                string[] main = line.Split(MAIN_DELIMITER);
                if(main.Length != 2){
                    throw new ArgumentException("Illegal message: "+line);
                }
                header = main[0];
                content = main[1].Split(SIDE_DELIMITER);
            }
        }


        protected StreamReader reader;
        protected StreamWriter writer;
        private NetworkStream network;
        protected Action<string> callback;
        private TcpClient socket;
        public SocketThreadedJob(SocketManager.HostAndPort hostAndPort, Action<string> callback)
        {
            socket = new TcpClient();
            socket.Connect(hostAndPort.host, hostAndPort.port);
            network = socket.GetStream();
            StreamWriter streamWriter = new System.IO.StreamWriter(network);
            StreamReader streamReader = new System.IO.StreamReader(network);
            this.reader = streamReader;
            this.writer = streamWriter;
            this.callback = callback;
        }

        public void Write(string s)
        {
            writer.WriteLine(s+"\r");
            writer.Flush();
        }


        protected override void ThreadFunction()
        {
            try
            {
                while (true)
                {
                    if (!IsDone)
                    {
                        string s = reader.ReadLine();
                        callback(s);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("error");
            }
        }

        protected override void OnFinished()
        {
            socket.Close();
            network.Close();
            reader.Close();
            writer.Close();
        }
    }
}
