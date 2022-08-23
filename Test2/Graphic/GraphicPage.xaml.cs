using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test2.Graphic
{
    /// <summary>
    /// Interaction logic for GraphicPage.xaml
    /// </summary>
    public partial class GraphicPage : Page
    {
        private Brush[] BrushCollection = new Brush[]
        {
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.Black),
            new SolidColorBrush(Colors.DeepPink)
        };
        private int brushpointer = 0;
        private uint vertrange = 100;
        public uint VerticalValueRange
        {
            get
            {
                return vertrange;
            }
            set
            {
                vertrange = value;
                foreach (Frame f in body.Children)
                    if (f.Name.StartsWith("Trend"))
                        (f.Content as DotsPage).VerticalBufferSize = (int)horrange;
            }
        }

        private uint horrange = 200;
        public uint HorizontalValueRange
        {
            get
            {
                return horrange;
            }
            set
            {
                horrange = value;
                foreach(Frame f in body.Children)
                    if (f.Name.StartsWith("Trend"))
                        (f.Content as DotsPage).HorizontalBufferSize = (int)horrange;
                
            }

        }
        private int trendNum = 0;
        public int NumberOfTrends
        {
            get => trendNum;
            set
            {
                while(value > trendNum)
                {
                    AddTrend(trendNum, BrushCollection[brushpointer]);
                    brushpointer = (brushpointer + 1) % BrushCollection.Length;
                    trendNum++;
                }
                while(value < trendNum)
                {
                    trendNum--;
                    RemoveTrend(trendNum);
                }
            }
        }
        public GraphicPage()
        {
            InitializeComponent();
            Width = 400;
            Height = 400;
            Decoration.Content = new DecorationPage();
        }
        public GraphicPage(int trends):this()
        {
            NumberOfTrends = trends;
        }
        private void AddTrend(int id, Brush color)
        {
            var frame = new Frame();
            frame.SetValue(Panel.ZIndexProperty, id);
            frame.SetValue(Grid.ColumnProperty, 1);
            frame.SetValue(Grid.ColumnSpanProperty, 2);
            frame.SetValue(Grid.RowProperty, 2);
            frame.SetValue(Grid.RowSpanProperty, 2);
            frame.Name = "Trend" + id;
            frame.Background = new SolidColorBrush(Colors.Transparent);
            if (id == 0)
                frame.Content = new DotsPage((int)horrange, (int)vertrange, Height * 0.94, Width * 0.94, color);
            else
            {
                var pointer = ((body.Children[1] as Frame).Content as DotsPage).Pointer;
                frame.Content = new DotsPage((int)horrange, (int)vertrange, Height * 0.94, Width * 0.94, color, pointer);
            }
            body.Children.Add(frame);
        }
        private void RemoveTrend(int id)
        {
            Frame frame = null;
            foreach(Frame f in body.Children)
            {
                if (f.Name.StartsWith("Trend") && f.Name.EndsWith(id.ToString()))
                {
                    frame = f;
                    break;
                }
            }
            if (frame == null)
                throw new ArgumentException();
            body.Children.Remove(frame);
        }
    }
}
