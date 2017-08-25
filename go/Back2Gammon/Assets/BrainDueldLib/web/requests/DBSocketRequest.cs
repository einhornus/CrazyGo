
using System.Collections.Generic; using System;

using System.Text;


//using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using BrainDuelsLib.web.requests;
using BrainDuelsLib.web;
using BrainDuelsLib.view;

using System.IO;
using System.Net.Sockets;

using BrainDuelsLib.robot;

namespace BrainDuelsLib.web.socket_requests
{
    public class DBSocketRequest
    {
        public static string host = SocketManager.DB_SERVER.host;
        public static int port = SocketManager.DB_SERVER.port;

        public static TcpClient socket;
        public static NetworkStream network;

        public static void Close()
        {
            if(network != null){
                network.Close();
                socket.Close();
                socket = null;
                network = null;
            }
        }

        public static Response Send(string str)
        {
            try
            {
                if(socket == null){
                    socket = new TcpClient();
                    socket.Connect(host, port);
                    network = socket.GetStream();
                }

                StreamWriter streamWriter = new System.IO.StreamWriter(network);
                StreamReader streamReader = new System.IO.StreamReader(network);
                streamWriter.WriteLine(str+"\r");
                streamWriter.Flush();
                string r = streamReader.ReadLine();
                r = r.Replace('@', '\n');
                if(r[0] == '!'){
                    return new ErrorResponse(r);
                }
                else{
                    return new GoodResponse(r);
                }
            }
            catch (Exception e)
            {
                return new ErrorResponse("UNEXPECTED_ERROR", e.StackTrace);
            }
        }
    }
}
