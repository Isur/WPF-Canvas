using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace drawApp
{
    enum MODE { DRAWING, LINE, EDIT }
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
        Line editable;
        Ellipse circle;
        Ellipse clickedCircle;
        MODE mode = MODE.DRAWING;
        List<Line> lines = new List<Line> { };
        List<Ellipse> circles = new List<Ellipse> { };
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
                    drawLine(true);
                }
            } else if(this.mode == MODE.EDIT)
            {
                
            }
        }

        private Point dist(Point p1, Point p2, Point p3)
        {
            double distanceP1 = Math.Sqrt(Math.Pow((p1.X - p3.X), 2) + Math.Pow((p1.Y - p3.Y), 2));
            double distanceP2 = Math.Sqrt(Math.Pow((p2.X - p3.X), 2) + Math.Pow((p2.Y - p3.Y), 2));
            return distanceP1 > distanceP2 ? p2 : p1;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mode == MODE.DRAWING)
            {
                 if (e.LeftButton == MouseButtonState.Pressed)
                {
                    drawPoint(e.GetPosition(this));
                }
            } else if(this.mode == MODE.EDIT)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point p = e.GetPosition(this);
                    if (this.editable != null)
                    {
                        Point x = dist(new Point(this.editable.X1, this.editable.Y1), new Point(this.editable.X2, this.editable.Y2), p);
                        if (x.X == this.editable.X1 && this.editable.Y1 == x.Y)
                        {
                            this.editable.X1 = p.X;
                            this.editable.Y1 = p.Y;
                        }
                        else
                        {
                            this.editable.X2 = p.X;
                            this.editable.Y2 = p.Y;
                        }
                        this.clickedCircle.SetValue(Canvas.LeftProperty, p.X - 5);
                        this.clickedCircle.SetValue(Canvas.TopProperty, p.Y - 5);
                    }
                } else if (e.LeftButton == MouseButtonState.Released)
                {
                    this.editable = null;
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

        private void drawLine(Boolean save)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(this.color);
            line.X1 = this.firstPoint.X;
            line.X2 = this.secondPoint.X;
            line.Y1 = this.firstPoint.Y;
            line.Y2 = this.secondPoint.Y;
            line.MouseDown += new MouseButtonEventHandler(line_click);
            paintSurface.Children.Add(line);
            lines.Add(line);
        }

        private void line_click(object sender, MouseButtonEventArgs e)
        {
            if(this.mode == MODE.EDIT)
            {
                Point p1 = new Point( ((Line)sender).X1, ((Line) sender).Y1);
                Point p2 = new Point(((Line)sender).X2, ((Line)sender).Y2);
                drawCircle(p1);
                drawCircle(p2);
            }
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
            circle.MouseDown += new MouseButtonEventHandler(circleClick);
            paintSurface.Children.Add(circle);
            this.circles.Add(circle);
        }

        private void circleClick(object sender, MouseButtonEventArgs e)
        {
            if(this.mode == MODE.EDIT)
            {
                Point f = e.GetPosition(this);
                this.clickedCircle = (Ellipse)sender;
                Line editableLine = findLine(f);
                if (editableLine != null)
                {
                    this.editable = editableLine;
                }
            }
        }

        private Line findLine(Point p)
        {
            return this.lines.Find(line =>
            {
                return posInRange(p, new Point(line.X1, line.Y1)) || posInRange(p, new Point(line.X2, line.Y2));
            });
        }

        private Boolean posInRange(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) > 5) return false;
            if (Math.Abs(p1.Y - p2.Y) > 5) return false;
            return true;
        }

        private void removeAllCircles()
        {
            this.circles.ForEach(ci =>
            {
                paintSurface.Children.Remove(ci);
            });
        }

        private void removeCircle()
        {
            paintSurface.Children.Remove(this.circle);
            this.circles.Remove(this.circle);
        }

        private void clearCanvas()
        {
            paintSurface.Children.Clear();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            clearCanvas();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int mode = ModeSelector.SelectedIndex;
            removeCircle();
            this.removeAllCircles();
            this.circles.Clear();
            switch (mode)
            {
                case 0:
                    this.mode = MODE.DRAWING;
                    break;
                case 1:
                    this.mode = MODE.LINE;
                    break;
                case 2:
                    this.mode = MODE.EDIT;
                    break;
                default:
                    this.mode = MODE.DRAWING;
                    break;
            }
        }

         private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorSelector colorSelector = new ColorSelector(this, this.color);
            colorSelector.Show();
        }

        private void toPng()
        {
            Transform transform = paintSurface.LayoutTransform;
            paintSurface.LayoutTransform = null;
            Size size = new Size(paintSurface.Width, paintSurface.Height);
            paintSurface.Measure(size);
            paintSurface.Arrange(new Rect(size));
            RenderTargetBitmap renderBitmat = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmat.Render(paintSurface);

            using (FileStream outstrem = new FileStream("image.png", FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmat));
                encoder.Save(outstrem);
            }

            paintSurface.LayoutTransform = transform;
            Filtration filtr = new Filtration();
            filtr.Show();
        }

        private void FilterSelectButton_Click(object sender, RoutedEventArgs e)
        {
            toPng();
        }
    }
}
