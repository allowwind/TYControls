using System.Windows;
using System.Windows.Controls;

namespace TYControls
{
    public class TYBusy : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(TYBusy), new PropertyMetadata(false));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(TYBusy), new PropertyMetadata("加载中!请稍后..."));

        #endregion Dependency Properties

        #region AnimationTime double 动画时间

        public double AnimationTime
        {
            get { return (double)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty =
            DependencyProperty.Register("AnimationTime", typeof(double), typeof(TYBusy), new PropertyMetadata(0d));

        #endregion AnimationTime double 动画时间
    }

    internal static partial class VisualStates
    {
        public const string GroupBusyStates = "GroupBusyStates";
        public const string StateBusy = "Busy";
        public const string StateIdle = "Idle";
    }
}