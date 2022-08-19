using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Test2.Models;
using Test2.Utility;

namespace Test2.Graphic
{
    public partial class DotsPage : Page
    {
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


        private ChartQueue<Point> Points;

        private int xpointer = 0;
        private int linepointer = 0;


        private double realheight;
        private double realwidth;

        public GeneratorTimer Timer { get; private set; }

        public DotsPage()
        {
            Points = new ChartQueue<Point>();
            InitializeComponent();
        }
        public DotsPage(int HorBufferSize, int VertBufferSize, double height, double width)
        {
            Points = new ChartQueue<Point>((uint)HorBufferSize);
            Points.AssignActionOnOverflow(() =>
            { 
                DestroyLine(linepointer - Points.Count);
            });

            VerticalBufferSize = VertBufferSize;
            HorizontalBufferSize = HorBufferSize;

            realheight = height;
            realwidth = width;

            InitializeComponent();

            Timer = new GeneratorTimer(20, () =>
            {
                var r = new System.Random();
                AddPointToQueue(xpointer, r.Next(0, VerticalBufferSize + 1));
                linepointer++;
                if (xpointer < HorizontalBufferSize)
                    xpointer++;
            });
        }
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

        private void BuildLine(Point point1, Point point2)
        {
            var line = LineTemplate;
            if (lastLine == null)
            {
                line.X1 = realwidth / (HorizontalBufferSize - 1) * point1.X;
                line.Y1 = realheight - realheight / VerticalBufferSize * point1.Y;
            }
            else
            {
                line.X1 = lastLine.X2;
                line.Y1 = lastLine.Y2;
            }
            line.X2 = realwidth / (HorizontalBufferSize - 1) * point2.X;
            line.Y2 = realheight - realheight / VerticalBufferSize * point2.Y;
            lastLine = line;
            chart_area.Children.Add(line);
        }
        
        private void DestroyLine(int id)
        {
            Line found = null;
            foreach (var c in chart_area.Children)
            {
                if (((Line)c).Name == "line" + id.ToString())
                    found = (Line)c;
                MoveLeft((Line)c);
            }
            chart_area.Children.Remove(found);
        }
        private void MoveLeft(Line line)
        {
            line.X1 -= realwidth / (HorizontalBufferSize - 1);
            line.X2 -= realwidth / (HorizontalBufferSize - 1);
        }
    }
}
