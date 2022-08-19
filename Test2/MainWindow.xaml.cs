using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Test2.Graphic;
namespace Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var window = new GraphicPage();

            var frame = new Frame();
            frame.Content = window;
            frame.Width = Width;
            frame.Height = Height;
            frame.HorizontalAlignment = HorizontalAlignment.Center;
            frame.VerticalAlignment = VerticalAlignment.Center;
            frame.SetValue(Grid.RowProperty, 1);
            frame.SetValue(Grid.ColumnProperty, 1);

            var panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.SetValue(Grid.RowProperty, 0);
            panel.SetValue(Grid.ColumnProperty, 1);

            var vtext = new TextBlock();
            var htext = new TextBlock();
            vtext.Foreground = new SolidColorBrush(Colors.White);
            htext.Foreground = new SolidColorBrush(Colors.White);
            vtext.HorizontalAlignment = HorizontalAlignment.Center;
            htext.HorizontalAlignment = HorizontalAlignment.Center;

            var verticalslider = CreateIntSlider(1, 500, 200);
            verticalslider.ValueChanged += (object sender, RoutedPropertyChangedEventArgs<double> e) =>
            {
                var val = (uint)verticalslider.Value;
                vtext.Text = "VerticalBuffer:" + val.ToString();
                window.VerticalValueRange = val;
            };
            var horizontalslider = CreateIntSlider(2, 500, 100);
            horizontalslider.ValueChanged += (object sender, RoutedPropertyChangedEventArgs<double> e) =>
            {
                var val = (uint)horizontalslider.Value;
                htext.Text = "HorizontalBuffer:" + val.ToString();
                window.HorizontalValueRange = val;
            };

            panel.Children.Add(vtext);
            panel.Children.Add(verticalslider);
            panel.Children.Add(htext);
            panel.Children.Add(horizontalslider);

            InitializeComponent();
            body.Children.Add(frame);
            body.Children.Add(panel);
        }
        private Slider CreateIntSlider(int minimum, int maximum, int defaultValue)
        {
            var slider = new Slider();
            slider.Minimum = minimum;
            slider.Maximum = maximum;
            slider.TickFrequency = 1;
            slider.Value = defaultValue;
            slider.HorizontalAlignment = HorizontalAlignment.Stretch;
            slider.VerticalAlignment = VerticalAlignment.Top;
            return slider;
        }
    }
}
