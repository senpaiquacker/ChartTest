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
        public GraphicPage()
        {
            InitializeComponent();
            Width = 400;
            Height = 400;
            Decoration.Content = new DecorationPage();
            DotsVisualised.Content = new DotsPage(100, 100, Height * 0.92, Width * 0.92);
        }
    }
}
