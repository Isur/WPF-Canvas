using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace drawApp
{
    static class ColorConverter
    {
    }

    public class HexColor
    {
        // hex strings
        string R;
        string G;
        string B;

        public HexColor(string R, string G, string B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public HexColor(string hex)
        {
            this.R = hex.Substring(0, 2);
            this.G = hex.Substring(2, 2);
            this.B = hex.Substring(4, 2);
        }

        public Color GetColor()
        {
            Color c = new Color();
            c.R = Utils.HexToByte(this.R);
            c.G = Utils.HexToByte(this.G);
            c.B = Utils.HexToByte(this.B);
            return c;
        }

        public RGBColor GetRGBColor()
        {
            return new RGBColor(Utils.HexToInt(this.R), Utils.HexToInt(this.G), Utils.HexToInt(this.B));
        }

        public CMYKColor GetCMYKColor()
        {
            float r = Utils.HexToInt(this.R) / 255.0f;
            float g = Utils.HexToInt(this.G) / 255.0f;
            float b = Utils.HexToInt(this.B) / 255.0f;
            float k = 1 - Math.Max(r, Math.Max(g, b));
            float c = (1 - r - k) / (1 - k);
            float m = (1 - g - k) / (1 - k);
            float y = (1 - b - k) / (1 - k);
            int K = (int) Math.Round(k * 100);
            int C = (int) Math.Round(c * 100);
            int M = (int) Math.Round(m * 100);
            int Y = (int) Math.Round(y * 100);
            CMYKColor cmyk = new CMYKColor(C, M, Y, K);
            return cmyk;
        }

        public override string ToString()
        {
            return $"{this.R}{this.G}{this.B}";
        }
    }

    public class RGBColor
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public RGBColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color GetColor()
        {
            Color c = new Color();
            c.R = Utils.IntToByte(this.R);
            c.G = Utils.IntToByte(this.G);
            c.B = Utils.IntToByte(this.B);
            return c;
        }

        public HexColor GetHexColor()
        {
            return new HexColor(Utils.IntToHex(this.R), Utils.IntToHex(this.G), Utils.IntToHex(this.B));
        }

        public CMYKColor GetCMYKColor()
        {
            float r = this.R / 255.0f;
            float g = this.G / 255.0f;
            float b = this.B / 255.0f;
            float k = 1 - Math.Max(r, Math.Max(g, b));
            float c = (1 - r - k) / (1 - k);
            float m = (1 - g - k) / (1 - k);
            float y = (1 - b - k) / (1 - k);
            int K = (int)Math.Round(k * 100);
            int C = (int)Math.Round(c * 100);
            int M = (int)Math.Round(m * 100);
            int Y = (int)Math.Round(y * 100);
            CMYKColor cmyk = new CMYKColor(C, M, Y, K);
            return cmyk;
        }
    }

    public class CMYKColor
    {
        public int C { get; }
        public int M { get; }
        public int Y { get; }
        public int K { get; }

        public CMYKColor(int c, int m, int y, int k)
        {
            C = c < 0 ? 0 : c;
            M = m < 0 ? 0 : m;
            Y = y < 0 ? 0 : y;
            K = k < 0 ? 0 : k;
            Console.WriteLine($"{C}={c} | {M}={m} | {Y}={y} | {K}={k}");
        }

        public Color GetColor()
        {
            return this.GetRGBColor().GetColor();
        }

        public RGBColor GetRGBColor()
        {
            float r = 255 * (1 - this.C / 100f) * (1 - K / 100f);
            float g = 255 * (1 - this.M / 100f) * (1 - K / 100f);
            float b = 255 * (1 - this.Y / 100f) * (1 - K / 100f);
            return new RGBColor((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
        }

        public HexColor GetHexColor()
        {
            return this.GetRGBColor().GetHexColor();
        }
    }
}
