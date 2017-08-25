
using System.Collections.Generic; using System;

using System.Text;

using System.Collections;
using BrainDuelsLib.web;

namespace BrainDuelsLib.utils.pic
{
    public class Clusterizer
    {
        public List<double[]> X = new List<double[]>();

        public void AddPoint(int a, int r, int g, int b)
        {
            double[] x = new double[] { a, r, g, b};
            X.Add(x);
        }

        public KeyValuePair<List<int>, int[,]> Clusterize(int clustersCount, int width, int height)
        {
            Random random = new Random();
            double maxDist = Double.MinValue;

            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < 1000; i++ )
            {           
                int p1 = random.Next()%X.Count;
                int p2 = random.Next()%X.Count;
                double distance = dist(X[p1], X[p2]);
                if(distance > maxDist){
                    index1 = p1;
                    index2 = p2;
                    maxDist = distance;
                }
            }

            List<double[]> centers = new List<double[]>();
            centers.Add(X[index1]);
            centers.Add(X[index2]);
            while(centers.Count < clustersCount){
                int toAdd = 0;
                double max = Double.MinValue;
                for(int q = 0; q<1000; q++){
                    int i = random.Next() % X.Count;
                    double min = Double.MaxValue;
                    for(int j = 0; j<centers.Count; j++){
                        double d = dist(centers[j], X[i]);
                        if(d < min){
                            min = d;
                        }
                    }
                    if(min > max){
                        max = min;
                        toAdd = i;
                    }
                }
                centers.Add(X[toAdd]);
            }

            int iterations = SocketManager.ImageCompression.iterationsCount;
            int[] clusterMarks = new int[X.Count];
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < X.Count; j++)
                {
                    double min = Double.MaxValue;
                    int cluster = -1;
                    for (int k = 0; k < centers.Count; k++)
                    {
                        double distance = dist(centers[k], X[j]);
                        if (distance < min)
                        {
                            min = distance;
                            cluster = k;
                        }
                    }
                    clusterMarks[j] = cluster;
                }


                for (int k = 0; k < centers.Count; k++)
                {
                    for (int q = 0; q < 4; q++)
                    {
                        double summ = 0;
                        double count = 0;
                        for (int j = 0; j < X.Count; j++)
                        {
                            if (clusterMarks[j] == k)
                            {
                                summ += 1.0 * X[j][q];
                                count += 1;
                            }
                        }
                        if (count != 0)
                        {
                            double average = summ / count;
                            centers[k][q] = average;
                        }
                    }
                }
            }

            List<int> res = new List<int>();

            for(int i = 0; i<clustersCount; i++){
                int a = (int)centers[i][0];
                int r = (int)centers[i][1];
                int g = (int)centers[i][2];
                int b = (int)centers[i][3];
                int color = (a << 24) + (r << 16) + (g << 8) + b;
                res.Add(color);
            }

            int[,] matrix = new int[height, width];

            for (int i = 0; i < height; i++ )
            {
                for (int j = 0; j < width; j++)
                {
                    int index = i*width+j;
                    int mark = clusterMarks[index];
                    matrix[i, j] = mark;
                }
            }
            return new KeyValuePair<List<int>,int[,]>(res, matrix);
        }

        private void dfs(int[,] graph, int[,] colors, int i, int j, int color)
        {
            int[] di = new int[] { 0, 1, -1, 0 };
            int[] dj = new int[] { 1, 0, 0, -1 };

            Stack<int[]> stack = new Stack<int[]>();
            stack.Push(new int[] { i, j });

            while (stack.Count > 0)
            {
                int[] pop = stack.Pop();
                int y = pop[0];
                int x = pop[1];

                if (colors[y, x] == 0)
                {
                    colors[y, x] = color;
                    for (int k = 0; k < di.Length; k++)
                    {
                        int newI = y + di[k];
                        int newJ = x + dj[k];
                        if (newI >= 0 && newI < graph.GetLength(0))
                        {
                            if (newJ >= 0 && newJ < graph.GetLength(1))
                            {
                                if (graph[newI, newJ] == graph[y, x])
                                {
                                    stack.Push(new int[]{newI, newJ});
                                }
                            }
                        }
                    }
                }
            }
        }

        private double dist(double[] x, double[] y)
        {
            double res = 0;
            for (int i = 0; i < x.Length; i++)
            {
                res += Math.Abs(x[i] - y[i]);
            }
            return res;
        }
    }
}
