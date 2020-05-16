using drawApp.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
//using System.Windows.Shapes;

namespace drawApp
{
    /// <summary>
    /// Interaction logic for Filtration.xaml
    /// </summary>
    public partial class Filtration : Window
    {
        public Filtration()
        {
            InitializeComponent();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            Img.Source = bitmap;
        }

        private void filter1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 1");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);
            Filter edgeFilter = new Edge();
            Bitmap res = Compute(bmp, edgeFilter);
            Img.Source = BitmapToImageSource(res);

        }

        private void filter2_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 2");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new Laplcae();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        private void filter3_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 3");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new Horizontal();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private Bitmap Compute(Bitmap bmp, Filter filter)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);

            var offset = filter.Size / 2;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color[,] colorMap = new Color[filter.Size, filter.Size];
                    for(int filterY = 0; filterY < filter.Size; filterY++)
                    {
                        int p;
                        if (filterY + x - offset <= 0) p = 0;
                        else if (filterY + x - offset >= bmp.Width - 1) p = bmp.Width - 1;
                        else p = filterY + x - offset;
                        for (int filterX = 0; filterX < filter.Size; filterX++)
                        {
                            int l;
                            if (filterX + y - offset <= 0) l = 0;
                            else if (filterX + y - offset >= bmp.Height - 1) l = bmp.Height - 1;
                            else l = filterX + y - offset;

                            colorMap[filterY, filterX] = bmp.GetPixel(p, l);
                        }
                    }
                    result.SetPixel(x, y, colorMap * filter);
                }

            }
            return result;
        }

        private void filter4_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 4");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new MeanRemoval();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        private void filter5_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 5");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new Avg();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        private void filter6_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 6");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new Cirlce();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        private void filter7_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER 7");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Filter filter = new Piramid();
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }

        private void custom_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FILTER CUSTOM");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile(path);

            Double[,] customFilterData = new Double[,]
            {
                { Convert.ToDouble(a11.Text), Convert.ToDouble(a12.Text), Convert.ToDouble(a13.Text) },
                { Convert.ToDouble(a21.Text), Convert.ToDouble(a22.Text), Convert.ToDouble(a23.Text) },
                { Convert.ToDouble(a31.Text), Convert.ToDouble(a32.Text), Convert.ToDouble(a33.Text) }
            };

            Filter filter = new Custom(customFilterData);
            Bitmap res = Compute(bmp, filter);
            Img.Source = BitmapToImageSource(res);
        }
    }
}
