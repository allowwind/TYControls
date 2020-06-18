using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace TYWIndow.Demo
{
    /// <summary>
    /// DragWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DragWindow
    {
        public DragWindow()
        {
            InitializeComponent();
            this.Loaded += DragWindow_Loaded;
        }
        Border border = new Border();

        private void DragWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
        Point point = new Point();
        public void InitStartPoint(Point prar_point)
        {
            point = prar_point;
        }
        double borderX;
        double borderY;
        public void InitControlMargin(double x, double y)
        {
            this.rootCanvas.Children.Clear();
            borderX = x;
            borderY = y;
            Point pNow = Mouse.GetPosition(this);
            Debug.Print($"InitControlMargin border at {pNow.X}--{pNow.Y}");
            this.rootCanvas.Children.Add(border);
            border.SetValue(Canvas.TopProperty, borderY + pNow.Y - point.Y);//设定位置Y
            border.SetValue(Canvas.LeftProperty, borderX + pNow.X - point.X); //设定位置X      

        }
        public void InitStartPoint(double x, double y)
        {
            point.X = x;
            point.Y = y;
            Debug.Print($"InitStartPoint border at {point.X}--{point.Y}");
        }

        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pNow = Mouse.GetPosition(this);
            Debug.Print($"RootCanvas_MouseMove border at {pNow.X}--{pNow.Y}");
            border.SetValue(Canvas.TopProperty, borderY + pNow.Y - point.Y);//设定位置Y
            border.SetValue(Canvas.LeftProperty, borderX + pNow.X - point.X); //设定位置X      
        }

        public Action OnMouseRealese;
        private void RootCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseRealese?.Invoke();
            this.Close();

        }

        internal void initHeightWidth(double actualWidth, double actualHeight)
        {
            border.Width = actualWidth;
            border.Height = actualHeight;
            border.Background = Brushes.Red;
        }
    }
}
