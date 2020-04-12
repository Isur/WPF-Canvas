using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace drawApp
{
    enum MODE { DRAWING, LINE }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color color = new Color();
        Point currentPoint = new Point();
        Point firstPoint;
        Point secondPoint;
        Boolean clicked = false;
        Ellipse circle;
        MODE mode = MODE.DRAWING;
        List<UIElement> deleted = new List<UIElement>();
        public MainWindow()
        {
            InitializeComponent();
            this.color = Color.FromRgb(255, 0, 0);
            ColorSelected.Fill = new SolidColorBrush(this.color);

        }

        public void setColor(Color c)
        {
            this.color.R = c.R;
            this.color.G = c.G;
            this.color.B = c.B;
            ColorSelected.Fill = new SolidColorBrush(this.color);
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.mode == MODE.DRAWING)
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    currentPoint = e.GetPosition(this);
                }
            } else if (this.mode == MODE.LINE)
            {
                if (!clicked)
                {
                    firstClick(e.GetPosition(this));
                }
                else
                {
                    secondClick(e.GetPosition(this));
                    drawLine();
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mode == MODE.DRAWING)
            {
                 if (e.LeftButton == MouseButtonState.Pressed)
                {
                    drawPoint(e.GetPosition(this));
                }
            }
        }

        private void drawPoint(Point point)
        {
            Line line = new Line();

            line.Stroke = new SolidColorBrush(this.color);
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = point.X;
            line.Y2 = point.Y;

            currentPoint = point;

            paintSurface.Children.Add(line);
        }
        private void firstClick(Point point)
        {
            firstPoint = point;
            drawCircle(point);
            this.clicked = true;
        }

        private void secondClick(Point point)
        {
            secondPoint = point;
            removeCircle();
            this.clicked = false;
        }

        private void drawLine()
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(this.color);
            line.X1 = this.firstPoint.X;
            line.X2 = this.secondPoint.X;
            line.Y1 = this.firstPoint.Y;
            line.Y2 = this.secondPoint.Y;

            paintSurface.Children.Add(line);
        }

        private void drawCircle(Point point)
        {
            this.circle = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Black,
                StrokeThickness = 5,
                Name = "Circle"
            };
            circle.SetValue(Canvas.LeftProperty, point.X - 5);
            circle.SetValue(Canvas.TopProperty, point.Y - 5);
            paintSurface.Children.Add(circle);
        }

        private void removeCircle()
        {
            paintSurface.Children.Remove(this.circle);
        }

        private void clearCanvas()
        {
            paintSurface.Children.Clear();
        }

        private void back()
        {
            if (paintSurface.Children.Count <= 0) return;
            this.deleted.Add(paintSurface.Children[paintSurface.Children.Count - 1]);
            paintSurface.Children.RemoveAt(paintSurface.Children.Count - 1);
        }

        private void next()
        {
            if (this.deleted.Count <= 0) return;
            paintSurface.Children.Add(this.deleted[0]);
            this.deleted.RemoveAt(0);
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            clearCanvas();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            next();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int mode = ModeSelector.SelectedIndex;
            removeCircle();
            switch (mode)
            {
                case 0:
                    this.mode = MODE.DRAWING;
                    break;
                case 1:
                    this.mode = MODE.LINE;
                    break;
                default:
                    this.mode = MODE.DRAWING;
                    break;
            }
        }

         private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorSelector colorSelector = new ColorSelector(this);
            colorSelector.Show();
        }
    }
}
