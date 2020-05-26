using System;
using System.Collections.Generic;
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
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Busy = "系统加载中...";
             IsBusy = true;
             var ret= await Task.Run(()=>{

                 Thread.Sleep(2000);
                 for (int i = 0; i < 1000; i++)
                 {
                     listData.Add(i + 1);
                 }
                 return true;
             });
            IsBusy = false;
            //分页 数据
            pager.TotalCount = listData.Count;
            if (pager.CurrentIndex == 1)
            {
                UpdateData(1);
            }
            else
            {
                pager.CurrentIndex = 1;
            }
            IsBusy = true;
            Busy = "加载完成请稍后...";
            await Task.Delay(2000);
            IsBusy = false;


        }

        private void Pager_CurrentIndexChanged(object sender, TYControls.CurrentIndexChangedEventArgs e)
        {
            UpdateData(e.CurrentIndex);
        }
        List<int> listData=new   List<int>();
        private void UpdateData(int nowIndex)
        {
            //vm.ListData.Clear();
            //IEnumerable<int> lisTmp = listData.Skip((nowIndex - 1) * pager.PageSize).Take(pager.PageSize);
            //foreach (var item in lisTmp)
            //{

            //    vm.ListData.Add(item);
            //}
        }
    }
}
