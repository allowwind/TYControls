using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using TYControls;

namespace TYWIndow.Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
           
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dragWindow.IsActive)
                dragWindow.Close();
        }
 

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Busy = "系统加载中...";

            IsBusy = true;
            //var ret= await Task.Run(()=>{

            //    Thread.Sleep(2000);
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        listData.Add(i + 1);
            //    }
            //    return true;
            //});
            IsBusy = false;
            //分页 数据



        }

        bool CheckMouseOutSide()
        { 
            var point = Mouse.GetPosition(this);
            if (point.X < 0 || point.Y < 0|| point.X>this.ActualWidth|| point.Y> this.ActualHeight)
            {
                return true;
            }
            return false;

        }
        private void Pager_CurrentIndexChanged(object sender, TYControls.CurrentIndexChangedEventArgs e)
        {
            UpdateData(e.CurrentIndex);
        }
        List<int> listData = new List<int>();
        private void UpdateData(int nowIndex)
        {
            //vm.ListData.Clear();
            //IEnumerable<int> lisTmp = listData.Skip((nowIndex - 1) * pager.PageSize).Take(pager.PageSize);
            //foreach (var item in lisTmp)
            //{

            //    vm.ListData.Add(item);
            //}
        }
        public static Task<bool> TouchHold(FrameworkElement element,int millisecondsTick, double duration)
        {
            DispatcherTimer timer = new DispatcherTimer();
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            timer.Interval = TimeSpan.FromMilliseconds(millisecondsTick);

            MouseButtonEventHandler touchUpHandler = delegate
            {
                timer.Stop();
                if (task.Task.Status == TaskStatus.Running)
                {
                    task.SetResult(false);
                }
            };
            
            element.PreviewMouseUp += touchUpHandler;
            int ticks = 0;
            timer.Tick += delegate
            {
                ticks += millisecondsTick;
                if (millisecondsTick > duration)
                {
                    element.PreviewMouseUp -= touchUpHandler;
                    timer.Stop();
                    task.SetResult(true);
                }
            };

            timer.Start();
            return task.Task;
        }
        DragWindow dragWindow = new DragWindow();
        private async void btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.Print($"PreviewMouseDown --start{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            if (dragWindow.IsActive)
                dragWindow.Close();
            if (await TouchHold(sender as Border, 200,2))
            {
                Debug.Print($"PreviewMouseDown --end{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                Border border = sender as Border;
                dragWindow = new DragWindow();
                dragWindow.OnMouseRealese += () =>
                {

                    if (CheckMouseOutSide())
                    {
                        var window = Application.Current.Windows.OfType<DragWindow>().Where(w =>w.IsActive);

                        //MessageBox.Show("oh ! outside ");
                    }
                };
                Point relativePoint = border.PointToScreen(new Point(0, 0));//控件相对左上角的位置
                Debug.Print($"Border_MouseLeftButtonDown border at {relativePoint.X}--{relativePoint.Y}");
                var point_AtBorder = Mouse.GetPosition(border);
                dragWindow.initHeightWidth(border.ActualWidth, border.ActualHeight);
                dragWindow.InitStartPoint(relativePoint.X + point_AtBorder.X, relativePoint.Y + point_AtBorder.Y);
                dragWindow.Show();
                dragWindow.InitControlMargin(relativePoint.X, relativePoint.Y);

            }
        }


        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
