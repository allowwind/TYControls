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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TYControls
{

    public class TYWindow : Window
    {
        static TYWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TYWindow), new FrameworkPropertyMetadata(typeof(TYWindow)));
        }

        private const string MinimizeButtonName = "minimizeButton";
        private const string MaximizeRestoreButtonName = "maximizeRestoreButton";
        private const string CloseButtonName = "closeButton";




        private Button m_minimizeButton;
        private Button m_maximizeRestoreButton;
        private Button m_closeButton;




        /// <summary>
        /// Creates a new <see cref="TYWindow" />.
        /// </summary>
        public TYWindow() : base() { }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (m_minimizeButton != null)
                m_minimizeButton.Click -= MinimizeButtonClickHandler;

            m_minimizeButton = GetTemplateChild(MinimizeButtonName) as Button;

            if (m_minimizeButton != null)
                m_minimizeButton.Click += MinimizeButtonClickHandler;

            if (m_maximizeRestoreButton != null)
                m_maximizeRestoreButton.Click -= MaximizeRestoreButtonClickHandler;

            m_maximizeRestoreButton = GetTemplateChild(MaximizeRestoreButtonName) as Button;

            if (m_maximizeRestoreButton != null)
                m_maximizeRestoreButton.Click += MaximizeRestoreButtonClickHandler;

            if (m_closeButton != null)
                m_closeButton.Click -= CloseButtonClickHandler;

            m_closeButton = GetTemplateChild(CloseButtonName) as Button;

            if (m_closeButton != null)
                m_closeButton.Click += CloseButtonClickHandler;
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void MaximizeRestoreButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }



        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register("TitleBarBackground", typeof(Brush), typeof(TYWindow), new PropertyMetadata(default(Brush)));



        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(TYWindow), new PropertyMetadata(0d));



        public Brush TitleBarForeground
        {
            get { return (Brush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }


        public static readonly DependencyProperty TitleBarForegroundProperty =
           DependencyProperty.Register("TitleBarForeground", typeof(Brush), typeof(TYWindow), new PropertyMetadata(Brushes.Black));




        public object TitleLeftContent
        {
            get => GetValue(TitleLeftContentProperty);
            set => SetValue(TitleLeftContentProperty, value);
        }

        public static readonly DependencyProperty TitleLeftContentProperty =
           DependencyProperty.Register("TitleLeftContent", typeof(object), typeof(TYWindow), new PropertyMetadata(null));

        public object TitleRightContent
        {
            get => GetValue(TitleRightContentProperty);
            set => SetValue(TitleRightContentProperty, value);
        }

        public static readonly DependencyProperty TitleRightContentProperty =
            DependencyProperty.Register("TitleRightContent", typeof(object), typeof(TYWindow), new PropertyMetadata(null));




        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(TYWindow), new PropertyMetadata(false));




        public string Busy
        {
            get { return (string)GetValue(BusyProperty); }
            set { SetValue(BusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Busy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyProperty =
            DependencyProperty.Register("Busy", typeof(string), typeof(TYWindow), new PropertyMetadata(""));


    }
}
