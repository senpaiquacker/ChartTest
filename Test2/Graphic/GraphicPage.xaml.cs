using System;
using System.Collections.Generic;
using System.Text;
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
                (DotsVisualised.Content as DotsPage).VerticalBufferSize = (int)vertrange;
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
                (DotsVisualised.Content as DotsPage).HorizontalBufferSize = (int)horrange;
            }

        }
        public GraphicPage()
        {
            InitializeComponent();
            Width = 400;
            Height = 400;
            Decoration.Content = new DecorationPage();
            DotsVisualised.Content = new DotsPage((int)horrange, (int)vertrange, Height * 0.94, Width * 0.94);
        }
    }
}
