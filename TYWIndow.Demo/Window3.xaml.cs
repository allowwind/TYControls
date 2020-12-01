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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TYWIndow.Demo
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            pager.CurrentIndexChanged += Pager_CurrentIndexChanged;
            pager.PageSize = 10;
            pager.TotalCount = 100;
            pager.CurrentIndex = 1;
        }

        private void Pager_CurrentIndexChanged(object sender, TYControls.CurrentIndexChangedEventArgs e)
        {
           
        }
    }
}
