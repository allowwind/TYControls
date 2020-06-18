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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TYWIndow.Demo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        enum EnumLeads
        {
            LEAD_A, LEAD_B,
        }

        DoubleAnimation dubAnim = new DoubleAnimation();
        DoubleAnimation dubAnim2 = new DoubleAnimation();
        System.Windows.Threading.DispatcherTimer NewsTimer = new System.Windows.Threading.DispatcherTimer();
        EnumLeads leadText = EnumLeads.LEAD_A;

        public Window1()
        {
            InitializeComponent();
            this.Loaded += Window1_Loaded;
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            dubAnim.From = rootBox.ActualWidth;
            dubAnim.To = -ctrlA.ActualWidth;
            dubAnim.SpeedRatio = 0.05;
            dubAnim.Completed += DubAnim_Completed;
            Timeline.SetDesiredFrameRate(dubAnim, 320);
            ctrlA.BeginAnimation(Canvas.LeftProperty, dubAnim);

            dubAnim2.From = rootBox.ActualWidth;
            dubAnim2.To = -ctrlB.ActualWidth;
            dubAnim2.SpeedRatio = 0.05;
            Timeline.SetDesiredFrameRate(dubAnim2, 320);
            dubAnim2.Completed += DubAnim2_Completed;
            NewsTimer.Tick += NewsTimer_Tick;
            NewsTimer.Interval = new TimeSpan(0, 0, 1);
            NewsTimer.Start();
        }

        private void NewsTimer_Tick(object sender, EventArgs e)
        {
            Point ctrlALocation = ctrlA.TranslatePoint(new Point(0, 0), rootBox);
            Point ctrlBLocation = ctrlB.TranslatePoint(new Point(0, 0), rootBox);

            if (leadText == EnumLeads.LEAD_A)
            {
                Double loc = ctrlALocation.X + ctrlA.ActualWidth;
                if (loc < rootBox.ActualWidth)

                //Double loc = ctrlALocation.X;
                // if (loc < 0)
                {
                    ctrlB.BeginAnimation(Canvas.LeftProperty, dubAnim2);
                    NewsTimer.Stop();
                }


            }
            else
            {

                Double loc = ctrlBLocation.X + ctrlB.ActualWidth;
                if (loc < rootBox.ActualWidth)

                {
                    ctrlA.BeginAnimation(Canvas.LeftProperty, dubAnim);
                    NewsTimer.Stop();
                }
            }

        }

        private void DubAnim2_Completed(object sender, EventArgs e)
        {
            leadText = EnumLeads.LEAD_A;
            NewsTimer.Start();
        }

        private void DubAnim_Completed(object sender, EventArgs e)
        {
            leadText = EnumLeads.LEAD_B;
            NewsTimer.Start();
        }
    }
}
