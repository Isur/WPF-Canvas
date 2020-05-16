using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawApp
{
    abstract public class Filter
    {
        abstract public double[,] Data { get; }
        public int Size => (int)Math.Sqrt(Data.Length);
        public double this[int i, int j] => Data[i, j];
        abstract public int Norm { get; }
        abstract public int Bias { get; }

        public static Color operator *(Color[,] map, Filter filter)
        {
            double red = 0.0;
            double green = 0.0;
            double blue = 0.0;

            for(int y = 0; y < filter.Size; y++)
            {
                for (int x = 0; x < filter.Size; x++)
                {
                    red += map[y, x].R * filter[x, y];
                    green += map[y, x].G * filter[x, y];
                    blue += map[y, x].B * filter[x, y];
                }
            }
            int r = NormalizeColorValue(red, filter);
            int g = NormalizeColorValue(green, filter);
            int b = NormalizeColorValue(blue, filter);

            return Color.FromArgb(r, g, b);
        }

        protected static int NormalizeColorValue(double value, Filter filter)
        {
            return Math.Min(Math.Max((int)(value / filter.Norm + filter.Bias), 0), 255);
        }
    }
}
