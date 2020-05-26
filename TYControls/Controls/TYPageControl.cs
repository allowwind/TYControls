using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TYControls
{
    public class TYPageControl : Control
    {
        static TYPageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TYPageControl), new FrameworkPropertyMetadata(typeof(TYPageControl)));
        }

        public TYPageControl()
        {
            if (TotalIndex == 0)
                TotalIndex = 1;
            if (CurrentIndex == 0)
                CurrentIndex = 1;
        }

        #region Routed Event
        public static readonly RoutedEvent CurrentIndexChangedEvent = EventManager.RegisterRoutedEvent("CurrentIndexChanged", RoutingStrategy.Bubble, typeof(CurrentIndexChangedEventHandler), typeof(TYPageControl));
        public event CurrentIndexChangedEventHandler CurrentIndexChanged
        {
            add { AddHandler(CurrentIndexChangedEvent, value); }
            remove { RemoveHandler(CurrentIndexChangedEvent, value); }
        }
        void RaiseCurrentIndexChanged(int index)
        {
            var arg = new CurrentIndexChangedEventArgs(index, CurrentIndexChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property
        /// <summary>
        /// Current index.
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(TYPageControl), new PropertyMetadata(OnCurrentIndexChanged));


        /// <summary>
        /// Total index.
        /// </summary>
        public int TotalIndex
        {
            get { return (int)GetValue(TotalIndexProperty); }
            set { SetValue(TotalIndexProperty, value); }
        }

        public static readonly DependencyProperty TotalIndexProperty =
            DependencyProperty.Register("TotalIndex", typeof(int), typeof(TYPageControl), new PropertyMetadata(OnTotalIndexChanged));



        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }



        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(TYPageControl), new PropertyMetadata(20));


        // Using a DependencyProperty as the backing store for TotalCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(int), typeof(TYPageControl), new PropertyMetadata(onTotalCountChanged));

        private static void onTotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagination = d as TYPageControl;
            pagination.TotalIndex=(pagination.TotalCount + pagination.PageSize - 1) / pagination.PageSize;

        }



        /// <summary>
        /// Theme brush.
        /// </summary>
        public Brush HoverBrush
        {
            get { return (Brush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(TYPageControl));

        /// <summary>
        /// Pagination style.
        /// </summary>
        public PaginationStyle PaginationStyle
        {
            get { return (PaginationStyle)GetValue(PaginationStyleProperty); }
            set { SetValue(PaginationStyleProperty, value); }
        }

        public static readonly DependencyProperty PaginationStyleProperty =
            DependencyProperty.Register("PaginationStyle", typeof(PaginationStyle), typeof(TYPageControl), new PropertyMetadata(PaginationStyle.Standard));

        /// <summary>
        /// Spacing
        /// </summary>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing", typeof(double), typeof(TYPageControl));


        /// <summary>
        /// CornerRadius
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(TYPageControl));
        #endregion

        #region Internal Property
        internal ObservableCollection<PagerItem> PaginationItems
        {
            get { return (ObservableCollection<PagerItem>)GetValue(PaginationItemsProperty); }
            set { SetValue(PaginationItemsProperty, value); }
        }

        internal static readonly DependencyProperty PaginationItemsProperty =
            DependencyProperty.Register("PaginationItems", typeof(ObservableCollection<PagerItem>), typeof(TYPageControl));

        internal ICommand PreviousCommand
        {
            get { return (ICommand)GetValue(PreviousCommandProperty); }
            set { SetValue(PreviousCommandProperty, value); }
        }

        internal static readonly DependencyProperty PreviousCommandProperty =
            DependencyProperty.Register("PreviousCommand", typeof(ICommand), typeof(TYPageControl), new PropertyMetadata(new PreviousCommand()));




        internal ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        internal static readonly DependencyProperty NextCommandProperty =
            DependencyProperty.Register("NextCommand", typeof(ICommand), typeof(TYPageControl), new PropertyMetadata(new NextCommand()));



        internal ICommand IndexCommand
        {
            get { return (ICommand)GetValue(IndexCommandProperty); }
            set { SetValue(IndexCommandProperty, value); }
        }

        internal static readonly DependencyProperty IndexCommandProperty =
            DependencyProperty.Register("IndexCommand", typeof(ICommand), typeof(TYPageControl), new PropertyMetadata(new IndexCommand()));


        #endregion

        #region EventHandler
        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagination = d as TYPageControl;

            if (pagination.CurrentIndex > pagination.TotalIndex)
            {
                pagination.CurrentIndex = pagination.TotalIndex;
                return;
            }
            else if (pagination.CurrentIndex < 1)
            {
                pagination.CurrentIndex = 1;
                return;
            }

            pagination.UpdatePaginationItems();
            pagination.RaiseCurrentIndexChanged(pagination.CurrentIndex);
        }

        private static void OnTotalIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagination = d as TYPageControl;

            if (pagination.TotalIndex < 1)
            {
                pagination.TotalIndex = 1;
                return;
            }

            if (pagination.CurrentIndex > pagination.TotalIndex)
            {
                pagination.CurrentIndex = pagination.TotalIndex;
            }

            pagination.UpdatePaginationItems();
        }
        #endregion

        #region Function
        private void UpdatePaginationItems()
        {
            if (PaginationItems == null)
                PaginationItems = new ObservableCollection<PagerItem>();

            PaginationItems.Clear();

            if (TotalIndex <= 7)
            {
                for (var i = 1; i <= TotalIndex; i++)
                {
                    PaginationItems.Add(new PagerItem(i, CurrentIndex == i));
                }
            }
            else
            {
                PaginationItems.Add(new PagerItem(1, CurrentIndex == 1));
                PaginationItems.Add(new PagerItem(2, CurrentIndex == 2));


                if (CurrentIndex == 1 || CurrentIndex == 2 || CurrentIndex == 3 || CurrentIndex == 4)
                {
                    PaginationItems.Add(new PagerItem(3, CurrentIndex == 3));
                    PaginationItems.Add(new PagerItem(4, CurrentIndex == 4));
                    PaginationItems.Add(new PagerItem(5, CurrentIndex == 5));
                }

                PaginationItems.Add(new PagerItem(null));

                if (CurrentIndex >= TotalIndex - 3)
                {
                    PaginationItems.Add(new PagerItem(null));

                    for (var i = TotalIndex - 4; i <= TotalIndex; i++)
                    {
                        PaginationItems.Add(new PagerItem(i, CurrentIndex == i));
                    }
                    return;
                }
                if (CurrentIndex != 1 && CurrentIndex != 2 && CurrentIndex != 3 && CurrentIndex != 4)
                {
                    for (var i = CurrentIndex - 1; i <= (CurrentIndex + 1); i++)
                    {
                        PaginationItems.Add(new PagerItem(i, CurrentIndex == i));
                    }
                }
                PaginationItems.Add(new PagerItem(null));
                for (var i = TotalIndex - 1; i <= TotalIndex; i++)
                {
                    PaginationItems.Add(new PagerItem(i, CurrentIndex == i));
                }
            }
        }
        #endregion
    }

    internal class PagerItem
    {
        public PagerItem(int? value)
        {
            Value = value;
        }

        public PagerItem(int? value, bool isSelected)
        {
            Value = value;
            IsSelected = isSelected;
        }

        public int? Value { get; set; }

        public bool IsSelected { get; set; }
    }

    internal class PreviousCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var pagination = (parameter as TYPageControl);

            if (pagination.CurrentIndex - 1 < 0)
                return;

            pagination.CurrentIndex--;
        }
    }

    internal class NextCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var pagination = (parameter as TYPageControl);

            if (pagination.CurrentIndex + 1 > pagination.TotalIndex)
                return;

            pagination.CurrentIndex++;
        }
    }

    internal class IndexCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var objs = parameter as object[];

            var pagination = objs[0] as TYPageControl;
            var index = (int)objs[1];


            pagination.CurrentIndex = index;
        }
    }
    public class CurrentIndexChangedEventArgs : RoutedEventArgs
    {
        public CurrentIndexChangedEventArgs(int currentIndex, RoutedEvent routedEvent) : base(routedEvent)
        {
            CurrentIndex = currentIndex;
        }

        public int CurrentIndex { get; set; }
    }

    public delegate void CurrentIndexChangedEventHandler(object sender, CurrentIndexChangedEventArgs e);
    #region PaginationStyle
    public enum PaginationStyle
    {
        Standard,
        Classic,
        Simple,
    }
    #endregion
}
