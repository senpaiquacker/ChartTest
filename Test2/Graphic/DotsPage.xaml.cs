using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System;
using Test2.Models;
using Point = Test2.Models.Point;
using System.Collections.Generic;

namespace Test2.Graphic
{
    /// <summary>
    /// Interaction logic for DotsPage.xaml
    /// </summary>
    public partial class DotsPage : Page
    {
        private ChartQueue<Point> Points;
        public int VerticalBufferSize { get; set; }
        private int horizontalBufferSize;
        public Brush lineBrush = new SolidColorBrush(Colors.Red);
        public int HorizontalBufferSize 
        { 
            get => horizontalBufferSize;
            set
            {
                horizontalBufferSize = value;
                Points.Buffer = (uint) value;
            }
        }

        private int xpointer = 0;
        private int realheight;
        private int realwidth;
        private int linepointer = 0;

        private Line lastLine = null;

        public DispatcherTimer Timer { get; private set; }
        public DotsPage()
        {
            Points = new ChartQueue<Point>();
            InitializeComponent();
        }
        public DotsPage(int HorBufferSize, int VertBufferSize, double height, double width)
        {
            VerticalBufferSize = VertBufferSize;
            Points = new ChartQueue<Point>((uint)HorBufferSize);
            HorizontalBufferSize = HorBufferSize;
            realheight = (int)height;
            realwidth = (int)width;
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(timerTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Start();
        }
        private void timerTick(object sender, EventArgs e)
        {
            var r = new Random();
            AddPointToQueue(xpointer, r.Next(0, VerticalBufferSize));
            linepointer++;
            if (xpointer < HorizontalBufferSize - 1)
                xpointer++;
        }
        public void AddPointToQueue(double x, double y)
        {
            var newpoint = new Point(x, y);
            var point = Points.PeekEnd();
            if (point != null)
            {
                if (Points.Count == HorizontalBufferSize)
                    DestroyLine(linepointer - Points.Count);
                BuildLine(point, newpoint);
                Points.Enqueue(newpoint);
            }
            else Points.Enqueue(newpoint);
        }

        private void BuildLine(Point point1, Point point2)
        {
            var line = new Line();
            if (lastLine == null)
            {
                line.X1 = realwidth / (HorizontalBufferSize - 1) * point1.X;
                line.Y1 = realheight / VerticalBufferSize * (VerticalBufferSize - point1.Y);
            }
            else
            {
                line.X1 = lastLine.X2;
                line.Y1 = lastLine.Y2;
            }
            line.X2 = realwidth / (HorizontalBufferSize - 1) * point2.X;
            line.Y2 = realheight / VerticalBufferSize * (VerticalBufferSize - point2.Y);

            line.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            line.Stroke = lineBrush;
            line.StrokeThickness = 1f;
            line.Visibility = System.Windows.Visibility.Visible;
            line.Name = "line" + (linepointer - 1).ToString();

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
