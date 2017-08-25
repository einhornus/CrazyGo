using System;
using System.Collections.Generic;
using System.Text;
using BrainDuelsLib.web;

namespace BrainDuelsLib.threads
{
    public class ChatSocketThreadedJob : SocketThreadedJob
    {
        private BrainDuelsLib.delegates.Action<int, string> messageReceived = delegate { };

        private Server.TokenAndId tai;

        private void MessageCallback(string s)
        {
            Message message = new Message(s);
            if (message.header.Equals("chat_message"))
            {
                int sender = int.Parse(message.content[0]);
                string content = message.content[1];
                string decoded = DecodeMessage(content);
                messageReceived(sender, decoded);
            }
        }

        public static string EncodeMessage(string message)
        {
            string res = "";
            for (int i = 0; i<message.Length; i++)
            {
                char c = message[i];
                for (int j = 0; j<16; j++)
                {
                    int value = (c >> j) % 2;
                    res += value;
                }
            }
            return res;
        }

        public void SendMessage(int receiver, string message)
        {
            string endodedMessage = EncodeMessage(message);
            int ceil = 0;
            if(endodedMessage.Length % SocketManager.CHUNK_SIZE == 0){
                ceil = endodedMessage.Length / SocketManager.CHUNK_SIZE;
            }
            else
            {
                ceil = endodedMessage.Length / SocketManager.CHUNK_SIZE + 1;
            }
            for (int i = 0; i < ceil; i++ )
            {
                int min = i * SocketManager.CHUNK_SIZE;
                int max = Math.Min(endodedMessage.Length - 1, (i+1) * SocketManager.CHUNK_SIZE);
                string chunk = endodedMessage.Substring(min, max - min + 1);
                string toWrite = "chat message=" + chunk + ";receiver=" + receiver + ";id=" + tai.id;
                Write(toWrite);
            }
        }

        public static string DecodeMessage(string binaryString)
        {
            string res = "";
            for (int i = 0; i<binaryString.Length/16; i++)
            {
                char ch = (char)0;
                for (int j = 0; j<16; j++)
                {
                    int index = i * 16 + j;
                    char cur = binaryString[index];
                    if (cur == '1')
                    {
                        int add = 1 << j;
                        ch = (char)((int)ch + add);
                    }
                }
                res += ch;
            }
            return res;
        }

        public void SetMessageReceivedCallback(BrainDuelsLib.delegates.Action<int, string> callback)
        {
            this.messageReceived = callback;
        }

        public ChatSocketThreadedJob(Server.TokenAndId tai):base(SocketManager.CHAT_SERVER, null)
        {
            this.tai = tai;
            this.callback = MessageCallback;
            this.Write("authorize id=" + tai.id + ";token=" + tai.token);
        }


        public void Stop()
        {
            this.IsDone = true;
            this.Update();
        }
    }
}
