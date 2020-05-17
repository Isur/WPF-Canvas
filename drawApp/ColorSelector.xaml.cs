using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace drawApp
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : Window
    {
        enum COLOR_TYPES { RGB, HEX, CMYK, INIT }

        HexColor hex;
        RGBColor rgb;
        CMYKColor cmyk;
        MainWindow caller;
        public ColorSelector(MainWindow caller, Color currentColor)
        {
            this.rgb = new RGBColor(currentColor.R, currentColor.G, currentColor.B);
            this.hex = this.rgb.GetHexColor();
            this.cmyk = this.hex.GetCMYKColor();
            InitializeComponent();
            this.caller = caller;
            this.setColors(COLOR_TYPES.INIT);
        }

        private void setColors(COLOR_TYPES color)
        {
            if(color == COLOR_TYPES.RGB)
            {
                this.rgb = new RGBColor((int)RGB_R_Slider.Value, (int)RGB_G_Slider.Value, (int)RGB_B_Slider.Value);
                this.cmyk = this.rgb.GetCMYKColor();
                this.hex = this.rgb.GetHexColor();
            }
            else if (color == COLOR_TYPES.HEX)
            {
                if (HEX_Text.Text.Length == 6)
                {
                    this.hex = new HexColor(HEX_Text.Text);
                    this.rgb = this.hex.GetRGBColor();
                    this.cmyk = this.hex.GetCMYKColor();
                }
                else return;
            }
            else if(color == COLOR_TYPES.CMYK)
            {
                this.cmyk = new CMYKColor((int)CMYK_C_Slider.Value, (int)CMYK_M_Slider.Value, (int)CMYK_Y_Slider.Value, (int)CMYK_K_Slider.Value);
                this.rgb = this.cmyk.GetRGBColor();
                this.hex = this.cmyk.GetHexColor();
            }
            HEX_Text.Text = this.hex.ToString();
            RGB_R_Slider.Value = this.rgb.R;
            RGB_G_Slider.Value = this.rgb.G;
            RGB_B_Slider.Value = this.rgb.B;
            RGB_R_Text.Text = this.rgb.R.ToString();
            RGB_G_Text.Text = this.rgb.G.ToString();
            RGB_B_Text.Text = this.rgb.B.ToString();
            CMYK_C_Text.Text = this.cmyk.C.ToString();
            CMYK_M_Text.Text = this.cmyk.M.ToString();
            CMYK_Y_Text.Text = this.cmyk.Y.ToString();
            CMYK_K_Text.Text = this.cmyk.K.ToString();
            CMYK_C_Slider.Value = this.cmyk.C;
            CMYK_M_Slider.Value = this.cmyk.M;
            CMYK_Y_Slider.Value = this.cmyk.Y;
            CMYK_K_Slider.Value = this.cmyk.K;

            colorPreview.Fill = new SolidColorBrush(Color.FromRgb((byte)this.rgb.R, (byte)this.rgb.G, (byte)this.rgb.B));
        }

        private void RGB_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            this.setColors(COLOR_TYPES.RGB);
        }
        
        private void CMYK_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            this.setColors(COLOR_TYPES.CMYK);
        }

        private void RGB_R_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RGB_R_Text.Text = ((int)e.NewValue).ToString();
        }

        private void RGB_G_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RGB_G_Text.Text = ((int)e.NewValue).ToString();
        }

        private void RGB_B_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RGB_B_Text.Text = ((int)e.NewValue).ToString();
        }

        private void CMYK_C_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CMYK_C_Text.Text = ((int)e.NewValue).ToString();
        }

        private void CMYK_M_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CMYK_M_Text.Text = ((int)e.NewValue).ToString();
        }

        private void CMYK_Y_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CMYK_Y_Text.Text = ((int)e.NewValue).ToString();
        }

        private void CMYK_K_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CMYK_K_Text.Text = ((int)e.NewValue).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.caller.setColor(this.rgb.GetColor());
            this.Close();
        }

        private void Save_Hex(object sender, RoutedEventArgs e)
        {
            this.setColors(COLOR_TYPES.HEX);
        }
    }
}
