using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TYControls
{
    public class TYMarqueeControl : Control
    {
        private const string ElementRoot = "PART_ROOT_Canvas";

        private const string ElementContent = "PART_Content";

        /// <summary>
        /// 影子
        /// </summary>
        private const string ElementContentShadow = "PART_ContentShadow";

        private FrameworkElement _elementContent;
        private FrameworkElement _elementContentShadow;
        private Canvas _elementRoot;

        #region animation property by lsbao
        enum EnumLeads
        {
            LEAD_A, LEAD_B,
        }

        DoubleAnimation dubAnim = new DoubleAnimation();
        DoubleAnimation dubAnim2 = new DoubleAnimation();
        System.Windows.Threading.DispatcherTimer NewsTimer = new System.Windows.Threading.DispatcherTimer();

        EnumLeads EnumMyLead = EnumLeads.LEAD_A;
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _elementContent = GetTemplateChild(ElementContent) as FrameworkElement;
            _elementContentShadow = GetTemplateChild(ElementContentShadow) as FrameworkElement;
            _elementRoot = GetTemplateChild(ElementRoot) as Canvas;
        }



        public object ContentA
        {
            get { return (object)GetValue(ContentAProperty); }
            set { SetValue(ContentAProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentA.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAProperty =
            DependencyProperty.Register("ContentA", typeof(object), typeof(TYMarqueeControl), new PropertyMetadata(null));




        public object ContentB
        {
            get { return (object)GetValue(ContentBProperty); }
            set { SetValue(ContentBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentB.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentBProperty =
            DependencyProperty.Register("ContentB", typeof(object), typeof(TYMarqueeControl), new PropertyMetadata(null));


        public TYMarqueeControl()
        {
            this.Loaded += TYMarqueeControl_Loaded;
            MouseEnter += TYMarqueeControl_MouseEnter;
            MouseLeave += TYMarqueeControl_MouseLeave;
        }

        int animationDuration=20;
        private void TYMarqueeControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double maxWidthA = _elementRoot.ActualWidth + _elementContent.ActualWidth;
            double maxWidthB = _elementRoot.ActualWidth + _elementContent.ActualWidth;
            if (isAnimationA)
            {




                Point _elementContentLocation = _elementContent.TranslatePoint(new Point(0, 0), _elementRoot);

                dubAnim.From = _elementContentLocation.X;
                dubAnim.BeginTime = TimeSpan.FromSeconds(0);
                dubAnim.Duration = getDurationFromLoaction(_elementContent);
                //TimeSpan timeAnimation = new TimeSpan(0, 0, 20);
                //计算剩余的距离


                _elementContent.BeginAnimation(Canvas.LeftProperty, dubAnim);

            }
            if (isAnimationB)
            {
                Point _elementContentLocation = _elementContentShadow.TranslatePoint(new Point(0, 0), _elementRoot);             
                dubAnim2.Duration = getDurationFromLoaction(_elementContentShadow);
                dubAnim2.From = _elementContentLocation.X;
                dubAnim2.BeginTime = TimeSpan.FromSeconds(0);
                _elementContentShadow.BeginAnimation(Canvas.LeftProperty, dubAnim2);

            }

            isPause = false;
        }

        private void TYMarqueeControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //暂停动画
            isPause = true;
            if (isAnimationA)
            {
                dubAnim.BeginTime = null;
                _elementContent.BeginAnimation(Canvas.LeftProperty, dubAnim);

            }
            if (isAnimationB)
            {
                dubAnim2.BeginTime = null;
                _elementContentShadow.BeginAnimation(Canvas.LeftProperty, dubAnim2);

            }

        }

        private bool isAnimationA = false;
        private bool isAnimationB = false;
        private bool isPause = false;


        private void TYMarqueeControl_Loaded(object sender, RoutedEventArgs e)
        {
           // TimeSpan timeAnimation = new TimeSpan(0, 0, 20);
            dubAnim.From = _elementRoot.ActualWidth;
            dubAnim.To = -_elementContent.ActualWidth;
            dubAnim.Duration = TimeSpan.FromSeconds(animationDuration);

            dubAnim.Completed += DubAnim_Completed;
            Timeline.SetDesiredFrameRate(dubAnim, 320);
            _elementContent.BeginAnimation(Canvas.LeftProperty, dubAnim);
            isAnimationA = true;

            _elementContentShadow.Visibility = Visibility.Hidden;
            _elementContentShadow.SetValue(Canvas.LeftProperty, _elementRoot.ActualWidth);

            dubAnim2.From = _elementRoot.ActualWidth;
            dubAnim2.To = -_elementContentShadow.ActualWidth;
            //dubAnim2.Duration = new Duration(timeAnimation);
            dubAnim2.Duration = getDurationFromLoaction(_elementContentShadow);
            Timeline.SetDesiredFrameRate(dubAnim2, 320);
            dubAnim2.Completed += DubAnim2_Completed;
            NewsTimer.Tick += NewsTimer_Tick;
            NewsTimer.Interval = new TimeSpan(0, 0, 1);
            NewsTimer.Start();
        }

        TimeSpan getDurationFromLoaction(FrameworkElement framework)
        {
            double maxwidth = framework.ActualWidth + _elementRoot.ActualWidth;
            Point _elementContentLocation = framework.TranslatePoint(new Point(0, 0), _elementRoot);
            double durationTmp = (_elementContentLocation.X + framework.ActualWidth) / maxwidth * animationDuration;
            return TimeSpan.FromSeconds(durationTmp);

        }
        private void NewsTimer_Tick(object sender, EventArgs e)
        {
            if (isPause == true)
                return;
            if (EnumMyLead == EnumLeads.LEAD_A)
            {
                Point _elementContentLocation = _elementContent.TranslatePoint(new Point(0, 0), _elementRoot);
                Double loc = _elementContentLocation.X + _elementContent.ActualWidth;
                if (loc < _elementRoot.ActualWidth)
                {
                    dubAnim2.From = _elementRoot.ActualWidth;
                    dubAnim2.Duration = TimeSpan.FromSeconds(animationDuration);
                    _elementContentShadow.BeginAnimation(Canvas.LeftProperty, dubAnim2);
                    isAnimationB = true;
                    if (_elementContentShadow.Visibility != Visibility.Visible)
                    {
                        _elementContentShadow.Visibility = Visibility.Visible;
                    }
                    NewsTimer.Stop();
                }
            }
            else
            {
                Point _elementContentShadowLocation = _elementContentShadow.TranslatePoint(new Point(0, 0), _elementRoot);
                Double loc = _elementContentShadowLocation.X + _elementContentShadow.ActualWidth;
                if (loc < _elementRoot.ActualWidth)
                {
                    dubAnim.From = _elementRoot.ActualWidth;
                    dubAnim.Duration = TimeSpan.FromSeconds(animationDuration);
                    _elementContent.BeginAnimation(Canvas.LeftProperty, dubAnim);
                    isAnimationA = true;
                    NewsTimer.Stop();
                }
            }

        }

        private void DubAnim2_Completed(object sender, EventArgs e)
        {
            EnumMyLead = EnumLeads.LEAD_A;
            NewsTimer.Start();
        }

        private void DubAnim_Completed(object sender, EventArgs e)
        {
            EnumMyLead = EnumLeads.LEAD_B;
            NewsTimer.Start();
        }

    }
}
