
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.utils.pic
{
    public class LightImage
    {
        public int[,] matrix;
        public int width;
        public int height;

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public IImage FillImage(IImage created)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    created.SetPixel(j, i, matrix[i, j]); //-V3066
                }
            }
            return created;
        }

        public LightImage(IImage image)
        {
            width = image.GetWidth();
            height = image.GetHeight();

            matrix = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = image.GetPixel(j, i); //-V3066
                }
            }
        }

        public LightImage(int[,] matrix, int width, int height)
        {
            this.matrix = matrix;
            this.width = width;
            this.height = height;
        }


        public override String ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(width + "|");
            res.Append(height + "|");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    string s = matrix[i, j] + "";
                    res.Append(s + "|");
                }
            }
            return res.ToString();
        }

        public LightImage Cut(int top, int left, int bottom, int right)
        {
            int newWidth = right - top + 1;
            int newHeight = bottom - top + 1;

            int[,] newMatrix = new int[newHeight, newWidth];
            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    int oldX = j - left;
                    int oldY = i - top;
                    int color = matrix[oldY, oldX];
                    newMatrix[i, j] = color;
                }
            }
            return new LightImage(newMatrix, newWidth, newHeight);
        }

        public LightImage Resize(double k)
        {
            int newWidth = (int)(width * k);
            int newHeight = (int)(height * k);
            int[,] newMatrix = new int[newHeight, newWidth];
            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    double x = j / k;
                    double y = i / k;
                    int newX = (int)x;
                    int newY = (int)y;
                    newMatrix[i, j] = matrix[newY, newX];
                }
            }
            return new LightImage(newMatrix, newWidth, newHeight);
        }

        public LightImage CropToSize(int dx, int dy)
        {
            int needWidth = dx;
            int needHeight = dy;
            double currentK = (double)(width) / (double)(height);
            double needK = (double)(needWidth) / (double)needHeight;

            int[,] resMatrix = new int[needHeight, needWidth];

            bool rot = needK >= currentK;

            if (rot)
            {
                double k = (double)needHeight / (double)height;
                LightImage resized = this.Resize(k);

                int left = (needWidth - resized.width) / 2;
                int right = needWidth - left;
                for (int i = 0; i < needHeight; i++)
                {
                    for (int j = 0; j < needWidth; j++)
                    {
                        int pixel = 0;
                        if (j > left && j < right && j - left >= 0 && j - left < resized.width && i < resized.height)
                        {
                            pixel = resized.matrix[i, j - left];
                        }
                        resMatrix[i, j] = pixel;
                    }
                }
            }
            else
            {
                double k = (double)needWidth / (double)width;
                LightImage resized = this.Resize(k);

                int top = (needHeight - resized.height) / 2;
                int bottom = needHeight - top;
                for (int i = 0; i < needHeight; i++)
                {
                    for (int j = 0; j < needWidth; j++)
                    {
                        int pixel = 0;
                        if (i > top && i < bottom && i - top >= 0 && i - top < resized.height && j < resized.width)
                        {
                            pixel = resized.matrix[i - top, j];
                        }
                        resMatrix[i, j] = pixel;
                    }
                }
            }
            return new LightImage(resMatrix, needWidth, needHeight);
        }
    }
}
