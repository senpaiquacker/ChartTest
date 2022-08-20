using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Test2.Models;
using Test2.Utility;

namespace Test2.Graphic
{
    public partial class DotsPage : Page
    {
        public GeneratorTimer Timer { get; private set; }
        private ChartQueue<Point> Points;
        private List<double> XDistances;
        #region Line
        public Brush lineBrush { get => new SolidColorBrush(Colors.Red); }
        public Line LineTemplate 
        { 
            get
            {
                var line = new Line();
                line.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                line.Stroke = lineBrush;
                line.StrokeThickness = 1f;
                line.Visibility = System.Windows.Visibility.Visible;
                line.Name = "line" + (linepointer - 1).ToString();
                return line;
            }
        }
        private Line lastLine = null;
        #endregion
        #region Buffer
        public int VerticalBufferSize { get; set; }
        private int horizontalBufferSize;
        public int HorizontalBufferSize 
        { 
            get => horizontalBufferSize;
            set
            {
                horizontalBufferSize = value;
                Points.Buffer = (uint) value;
                if(xpointer > horizontalBufferSize)
                    xpointer = horizontalBufferSize;
            }
        }
        #endregion
        #region Pointers
        private int xpointer = 0;
        private int linepointer = 0;
        #endregion
        #region Size
        private double realheight;
        private double realwidth;
        #endregion
        #region Constructors
        public DotsPage()
        {
            Points = new ChartQueue<Point>();
            InitializeComponent();
        }
        public DotsPage(int HorBufferSize, int VertBufferSize, double height, double width)
        {
            horizontalBufferSize = HorBufferSize;
            VerticalBufferSize = VertBufferSize;

            Points = new ChartQueue<Point>((uint)HorizontalBufferSize);
            Points.AssignActionOnOverflow(() =>
            { 
                DestroyLine(linepointer - Points.Count);
            });

            XDistances = new List<double>(HorizontalBufferSize);

            realheight = height;
            realwidth = width;

            InitializeComponent();

            Timer = new GeneratorTimer(1000, () =>
            {
                var r = new System.Random();
                AddPointToQueue(xpointer, r.Next(0, VerticalBufferSize + 1));
                linepointer++;
                if (xpointer < HorizontalBufferSize)
                    xpointer++;
            });
        }
        #endregion
        public void AddPointToQueue(double x, double y)
        {
            var newpoint = new Point(x, y);
            var point = Points.PeekEnd();
            if (point != null)
            {
                BuildLine(point, newpoint);
                Points.Enqueue(newpoint);
                
            }
            else Points.Enqueue(newpoint);
        }
        #region LineMethods
        private void BuildLine(Point point1, Point point2)
        {
            Line line;
            line = LineTemplate;
            XDistances.Add(realwidth / (HorizontalBufferSize - 1));
            if (lastLine == null)
            {
                line.X1 = XDistances[XDistances.Count - 1] * point1.X;
                line.Y1 = realheight - realheight / VerticalBufferSize * point1.Y;
            }
            else
            {
                line.X1 = lastLine.X2;
                line.Y1 = lastLine.Y2;
            }
            line.X2 = XDistances[XDistances.Count - 1] * point2.X;
            line.Y2 = realheight - realheight / VerticalBufferSize * point2.Y;
            chart_area.Children.Add(line);
        }
        private void DestroyLine(int id)
        {
            Line found = null;
            int k = 0;
            foreach (var p in Points)
            {
                p.X--;
                if (k == 0)
                {
                    k++;
                    continue;
                }
                var c = chart_area.Children[k - 1] as Line;
                if (((Line)c).Name == "line" + id.ToString())
                    found = (Line)c;
                MoveLeft((Line)c, XDistances[k - 1]);
                k++;
            }
            XDistances.RemoveAt(0);
            chart_area.Children.Remove(found);
        }
        private void MoveLeft(Line line, double distance)
        {
            line.X1 -= distance;
            line.X2 -= distance;
        }
        #endregion
    }
}
