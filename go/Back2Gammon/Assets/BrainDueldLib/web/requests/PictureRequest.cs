
using System.Collections.Generic; using System;

using System.Text;


using System.IO;
using System.Net.Sockets;

using BrainDuelsLib.threads;
using BrainDuelsLib.delegates;

namespace BrainDuelsLib.web.requests
{
    public class PictureRequest
    {
        public class GetPictureSocketThreadedJob : SocketThreadedJob
        {
			public BrainDuelsLib.delegates.Action<string> resultCallback = null;
            public Dictionary<int, string> chunks = new Dictionary<int, string>();

            public GetPictureSocketThreadedJob(Server.TokenAndId tai, int serverIndex, int pictureId):base(SocketManager.PICTURE_SERVERS[serverIndex], null)
            {
                this.callback = MessageCallback;
                string str = "get id="+tai.id+";token="+tai.token+";picture_id="+pictureId;
                Write(str);
            }

            public void MessageCallback(string str)
            {
                Message message = new Message(str);
                if (str[0] == '!')
                {
                    throw new BrainDuelsLib.web.exceptions.WebException();
                }

                if(message.header.Equals("chunk"))
                {
                    int index = int.Parse(message.content[0]);
                    string chunk = message.content[1];
                    chunks.Add(index, chunk);
                }

                if (message.header.Equals("begin"))
                {
                    chunks = new Dictionary<int, string>();
                }

                if(message.header.Equals("end"))
                {
                    StringBuilder result = new StringBuilder();
                    int chunkCount = chunks.Count;
                    for (int i = 0; i < chunkCount; i++ )
                    {
                        string chunk = chunks[i];
                        result.Append(chunk);
                    }
                    string res = result.ToString();
                    IsDone = true;
                    Update();
                    resultCallback(res);
                }
            }
        }



        public class SetPictureSocketThreadedJob : SocketThreadedJob
        {
			public BrainDuelsLib.delegates.Action resultCallback = null;
            private Server.TokenAndId tai;
            private int serverIndex = 0;
            private bool isLarge;

            public SetPictureSocketThreadedJob(Server.TokenAndId tai, int serverIndex, string content, bool isLarge)
                : base(SocketManager.PICTURE_SERVERS[serverIndex], null)
            {
                this.isLarge = isLarge;
                this.serverIndex = serverIndex;
                this.callback = MessageCallback;
                this.tai = tai;
                StringBuilder chunk = new StringBuilder();
                int index = 0;
                for (int i = 0; i < content.Length; i++ )
                {
                    if (chunk.Length < SocketManager.CHUNK_SIZE)
                    {
                        chunk.Append(content[i]);
                    }

                    if (chunk.Length == SocketManager.CHUNK_SIZE || i == content.Length - 1)
                    {
                        string str = "chunk id="+tai.id+";token="+tai.token+";index="+index+";chunk="+chunk;
                        Write(str);
                        index++;
                        chunk = new StringBuilder();
                    }
                }
                Write("end id="+tai.id+";token="+tai.token+";server_index="+serverIndex);
            }

            public void MessageCallback(string str)
            {
                Message message = new Message(str);
                if (str[0] == '!')
                {
                    throw new BrainDuelsLib.web.exceptions.WebException();
                }

                if (message.header.Equals("index"))
                {
                    IsDone = true;
                    Update();
                    int index = int.Parse(message.content[0]);
                    string request = "";
                    if (isLarge)
                    {
                        request = "set_ava id=" + tai.id + ";token=" + tai.token + ";server_id=" + serverIndex + ";index=" + index + ";size=large";
                    }
                    else
                    {
                        request = "set_ava id=" + tai.id + ";token=" + tai.token + ";server_id=" + serverIndex + ";index=" + index+";size=small";
                    }
                    BrainDuelsLib.web.socket_requests.DBSocketRequest.Send(request);
                    Server.GetUser(tai, tai.id);
                    resultCallback();
                    IsDone = true;
                    Update();
                }
            }
        }


		public static void GetPicture(Server.TokenAndId tai, int serverIndex, int pictureId, BrainDuelsLib.delegates.Action<string> resultCallback)
        {
            GetPictureSocketThreadedJob job = new GetPictureSocketThreadedJob(tai, serverIndex, pictureId);
            job.resultCallback = delegate (string s)
            {
                job.IsDone = true;
                job.Update();
                resultCallback(s);
            };
            job.Start();
        }

		public static void SetPicture(Server.TokenAndId tai, string content, BrainDuelsLib.delegates.Action resultCallback, bool isLarge)
        {
            Random random = new System.Random();
            int serverIndex = random.Next()%SocketManager.PICTURE_SERVERS.Length;
            SetPictureSocketThreadedJob job = new SetPictureSocketThreadedJob(tai, serverIndex, content, isLarge); 
            job.resultCallback = resultCallback;
            job.Start();
        }
    }
}
