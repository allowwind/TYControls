﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TYControls
{
    public interface ISpinable
    {
        bool IsSpin { get; set; }
    }

    public static class SpinExtend
    {
        private const string storyBoardName = "LSbSpinStoryBoard";

        public static void BeginSpin<T>(this T control, double seconds) where T : FrameworkElement, ISpinable
        {
            var transform = control.RenderTransform;
            control.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
            TransformGroup transformGroup;

            if (transform is TransformGroup)
            {
                if (!(((TransformGroup)transform).Children.FirstOrDefault() is RotateTransform))
                {
                    transformGroup = (TransformGroup)transform.Clone();
                    transformGroup.Children.Insert(0, new RotateTransform(0.0));
                    control.SetCurrentValue(UIElement.RenderTransformProperty, transformGroup);
                }
            }
            else
            {
                transformGroup = new TransformGroup();

                if (transform is RotateTransform)
                {
                    transformGroup.Children.Add(transform);
                }
                else
                {
                    transformGroup.Children.Add(new RotateTransform(0.0));

                    if (transform != null && transform != Transform.Identity)
                    {
                        transformGroup.Children.Add(transform);
                    }
                }

                control.SetCurrentValue(UIElement.RenderTransformProperty, transformGroup);
            }

            if (!(control.Resources[storyBoardName] is Storyboard storyboard))
            {
                storyboard = new Storyboard();
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    AutoReverse = false,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                Storyboard.SetTarget(animation, control);
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath("(0).(1)[0].(2)", UIElement.RenderTransformProperty,
                        TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

                storyboard.Children.Add(animation);
                control.Resources.Add(storyBoardName, storyboard);
            }
            storyboard.Children[0].Duration = TimeSpan.FromSeconds(seconds);
            storyboard.Begin();
        }

        public static void StopSpin<T>(this T control) where T : FrameworkElement, ISpinable
        {
            (control.Resources[storyBoardName] as Storyboard)?.Stop();
        }
    }
}