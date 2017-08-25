
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.view.forms;

namespace BrainDuelsLib.web
{
    public class SocketManager
    {
        public class HostAndPort{
            public string host;
            public int port;

            public HostAndPort(string host, int port){
                this.host = host;
                this.port = port;
            }
        }

        public static bool test = true;

        public static String PRODUCTION = "178.62.112.136";
        public static String MY = "46.101.218.246";
        public static String LOCALHOST = "localhost";

        public static string GetSocketsHost()
        {
            return LOCALHOST;
        }


        public static HostAndPort DB_SERVER = new HostAndPort(GetSocketsHost(), 111);
        public static HostAndPort LOBBY_SERVER = new HostAndPort(GetSocketsHost(), 112);
        public static HostAndPort[] GAME_SERVERS = new HostAndPort[] { new HostAndPort(GetSocketsHost(), 113) };
        public static HostAndPort CHAT_SERVER = new HostAndPort(GetSocketsHost(), 114);
        public static HostAndPort[] PICTURE_SERVERS = new HostAndPort[] { new HostAndPort(GetSocketsHost(), 115) };
        public static int CHUNK_SIZE = 16000;

        public class ImageCompression
        {
            public static bool doCompress = true;
            public static int clustersCount = 32;//must be <= 32
            public static int iterationsCount = 5;
        }

        public static ResourcesProvider resourcesProvider = null; 

        public class Images
        {
            public class Dimension{
                public int width;
                public int height;

                public Dimension(int width, int height){
                    this.width = width;
                    this.height = height;
                }
            }

            public static Dimension avatarSize = new Dimension(150, 200);
            public static Dimension miniAvatarSize = new Dimension(50, 75);
        }
    }
}
