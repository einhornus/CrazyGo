
using System.Collections.Generic; using System;

using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BrainDuelsLib.utils.pic
{
    public class ImageSerizlizer
    {

        public static LightImage Deserialize(string s)
        {
            if(s[0] == '1'){
                return DecompressColorClusterization(s);
            }

            if (s[0] == '0')
            {
                return DeserializeNoCompression(s);
            }

            throw new ArgumentException("Invalid picture file");
        }

        public static string Serialize(LightImage li)
        {
            if (BrainDuelsLib.web.SocketManager.ImageCompression.doCompress)
            {
                return CompressColorClasterization(li);
            }
            else
            {
                return SerializeWithoutCompression(li);
            }
        }

        private static byte unshiftByteToAvoidSpecialCharacters(byte b)
        {
            if (b == 110)
            {
                return (byte)('\n');
            }

            if (b == 111)
            {
                return (byte)('\r');
            }
            return b;
        }

        private static byte shiftByteToAvoidSpecialCharacters(byte b)
        {
            if((char)(b) == ';'){
                return (byte)(b + 1);
            }

            if ((char)(b) == '=')
            {
                return (byte)(b + 1);
            }

            if ((char)(b) == ' ')
            {
                return (byte)(b + 1);
            }

            if ((char)(b) == '\n')
            {
                return (byte)(110);
            }

            if ((char)(b) == '\r')
            {
                return (byte)(111);
            }

            return b;
        }

        public static string CompressColorClasterization(LightImage li)
        {
            Clusterizer clusterizer = new Clusterizer();
            for (int i = 0; i < li.height; i++)
            {
                for (int j = 0; j < li.width; j++)
                {
                    int pixel = li.matrix[i, j];
                    byte a = (byte)((pixel >> 24) % 256);
                    byte r = (byte)((pixel >> 16) % 256);
                    byte g = (byte)((pixel >> 8) % 256);
                    byte b = (byte)((pixel >> 0) % 256);
                    double x = (double)j / (double)(li.width);
                    double y = (double)i / (double)(li.height);
                    clusterizer.AddPoint(a, r, g, b);
                }
            }

            KeyValuePair<List<int>, int[,]> clus = clusterizer.Clusterize(BrainDuelsLib.web.SocketManager.ImageCompression.clustersCount, li.width, li.height);

            List<int> colors = clus.Key;
            int[,] marks = clus.Value;

            StringBuilder res = new StringBuilder();
            res.Append("1");
            string formattedWidth = String.Format("{0:D5}", li.width);
            string formattedHeight = String.Format("{0:D5}", li.height);
            res.Append(formattedWidth+formattedHeight);

            for (int i = 0; i < colors.Count; i++)
            {
                int color = colors[i];
                byte a = (byte)((color >> 24) % 256);
                byte r = (byte)((color >> 16) % 256);
                byte g = (byte)((color >> 8) % 256);
                byte b = (byte)((color >> 0) % 256);
                a = shiftByteToAvoidSpecialCharacters(a);
                r = shiftByteToAvoidSpecialCharacters(r);
                g = shiftByteToAvoidSpecialCharacters(g);
                b = shiftByteToAvoidSpecialCharacters(b);
                res.Append((char)a);
                res.Append((char)r);
                res.Append((char)g);
                res.Append((char)b);
            }

            for (int i = 0; i < li.height; i++)
            {
                for (int j = 0; j < li.width; j++)
                {
                    byte mark = (byte)marks[i, j];
                    mark = shiftByteToAvoidSpecialCharacters(mark);
                    res.Append((char)mark);
                }
            }
            return res.ToString();
        }

        public static LightImage DecompressColorClusterization(string s)
        {
            int width = int.Parse(s.Substring(1, 5));
            int height = int.Parse(s.Substring(6, 5));

            List<int> colors = new List<int>();
            int[,] marks = new int[height, width];

            for (int i = 0; i < BrainDuelsLib.web.SocketManager.ImageCompression.clustersCount; i++ )
            {
                byte a = (byte)s[10 + i * 4 + 0 + 1];
                byte r = (byte)s[10 + i * 4 + 1 + 1];
                byte g = (byte)s[10 + i * 4 + 2 + 1];
                byte b = (byte)s[10 + i * 4 + 3 + 1];
                a = unshiftByteToAvoidSpecialCharacters(a);
                r = unshiftByteToAvoidSpecialCharacters(r);
                g = unshiftByteToAvoidSpecialCharacters(g);
                b = unshiftByteToAvoidSpecialCharacters(b);
                int color = (a << 24) + (r << 16) + (g << 8) + b;
                colors.Add(color);
            }

            int[,] matrix = new int[height, width];
            for (int i = 10 + BrainDuelsLib.web.SocketManager.ImageCompression.clustersCount * 4 + 1; i < s.Length; i++)
            {
                int index = i - (10 + BrainDuelsLib.web.SocketManager.ImageCompression.clustersCount * 4 + 1);
                int y = index / width;
                int x = index % width;
                byte b = (byte)s[i];
                b = unshiftByteToAvoidSpecialCharacters(b);
                int color = colors[b];
                matrix[y, x] = color;
            }

            return new LightImage(matrix, width, height);
        }












        public static string SerializeWithoutCompression(LightImage li)
        {
            StringBuilder res = new StringBuilder();
            res.Append("0");
            string formattedWidth = String.Format("{0:D5}", li.width);
            string formattedHeight = String.Format("{0:D5}", li.height);
            res.Append(formattedWidth + formattedHeight);

            for (int i = 0; i < li.height; i++)
            {
                for (int j = 0; j < li.width; j++)
                {
                    int pixel = li.matrix[i, j];
                    byte a = (byte)((pixel >> 24) % 256);
                    byte r = (byte)((pixel >> 16) % 256);
                    byte g = (byte)((pixel >> 8) % 256);
                    byte b = (byte)((pixel >> 0) % 256);
                    a = shiftByteToAvoidSpecialCharacters(a);
                    r = shiftByteToAvoidSpecialCharacters(r);
                    g = shiftByteToAvoidSpecialCharacters(g);
                    b = shiftByteToAvoidSpecialCharacters(b);
                    res.Append((char)a);
                    res.Append((char)r);
                    res.Append((char)g);
                    res.Append((char)b);
                }
            }
            return res.ToString();
        }

        public static LightImage DeserializeNoCompression(string s)
        {
            int width = int.Parse(s.Substring(1, 5));
            int height = int.Parse(s.Substring(6, 5));

            int[,] matrix = new int[height, width];
            int index = 0;
            for (int i = 10 + 1; i < s.Length; i+=4)
            {
                byte a = (byte)(s[i + 0]);
                byte r = (byte)(s[i + 1]);
                byte g = (byte)(s[i + 2]);
                byte b = (byte)(s[i + 3]);
                int pixel = (a << 24) + (r << 16) + (g << 8) + b;
                int y = index / width;
                int x = index % width;
                matrix[y, x] = pixel;
                index++;
            }

            return new LightImage(matrix, width, height);
        }




    }
}
