
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TYControls
{
    public class TYIcon : FrameworkElement, ISpinable
    {
        static TYIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TYIcon), new FrameworkPropertyMetadata(typeof(TYIcon)));
        }

        #region IsSpin bool 是否旋转动画

        public bool IsSpin
        {
            get => (bool)GetValue(IsSpinProperty);
            set => SetValue(IsSpinProperty, value);
        }

        public static readonly DependencyProperty IsSpinProperty =
            DependencyProperty.Register("IsSpin", typeof(bool), typeof(TYIcon), new PropertyMetadata(false, OnIsSpinChanged));

        private static void OnIsSpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TYIcon).SetSpinAnimation();
        }

        #endregion IsSpin bool 是否旋转动画

        #region Data Geometry LSBIcon内容

        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(TYIcon), new PropertyMetadata(Geometry.Empty, OnUpdateLayOut));

        #endregion Data Geometry LSBIcon内容

        #region Scale double 缩放

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(TYIcon), new PropertyMetadata(1d, OnUpdateLayOut));

        private static void OnUpdateLayOut(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TYIcon ico = d as TYIcon;
            if (ico.IsLoaded)
            {
                ico.InvalidateMeasure();
                ico.InvalidateArrange();
                ico.InvalidateVisual();
                ico.UpdateLayout();
            }
        }

        #endregion Scale double 缩放

        #region SpinTime double 旋转时间

        public double SpinTime
        {
            get => (double)GetValue(SpinTimeProperty);
            set => SetValue(SpinTimeProperty, value);
        }

        public static readonly DependencyProperty SpinTimeProperty =
            DependencyProperty.Register("SpinTime", typeof(double), typeof(TYIcon), new PropertyMetadata(1d, OnSpinTimeChanged));

        private static void OnSpinTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TYIcon ico = d as TYIcon;
            if (ico.IsSpin)
            {
                ico.StopSpin();
                ico.BeginSpin(ico.SpinTime);
            }
        }

        #endregion SpinTime double 旋转时间

        private void SetSpinAnimation()
        {
            if (IsSpin)
            {
                this.BeginSpin(SpinTime);
            }
            else
            {
                this.StopSpin();
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double minHeight = 10.0;
            double minwidth = 10.0;
            if (IsGeometryEmpty(Data))
            {
                return new Size(0d, 0d);
            }
            Size size = new Size
            {
                Width = double.IsPositiveInfinity(constraint.Width) ? minwidth : constraint.Width,
                Height = double.IsPositiveInfinity(constraint.Height) ? minHeight : constraint.Height
            };
            //double d = Math.Min(size.Width, size.Height);
            //size.Width = d;
            //size.Height = d;
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //double d = Math.Min(finalSize.Width, finalSize.Height);
            return finalSize;
        }

        /// <summary>
        /// DependencyProperty for <see cref="Foreground" /> property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(typeof(TYIcon));

        /// <summary>
        /// The Foreground property specifies the foreground brush of an element's text content.
        /// </summary>
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// DependencyProperty setter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetForeground(DependencyObject element, Brush value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static Brush GetForeground(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (Brush)element.GetValue(ForegroundProperty);
        }

        public static readonly DependencyProperty BackgroundProperty =
         TextElement.BackgroundProperty.AddOwner(
                 typeof(TYIcon),
                 new FrameworkPropertyMetadata(
                         null,
                         FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The Background property defines the brush used to fill the content area.
        /// </summary>
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            Matrix matrix = GetStretchMatrix(Data);
            Geometry rendered = GetRenderedGeometry(Data, matrix);
            if (Background != null)
            {
                dc.DrawRectangle(Background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            }
            if (rendered != Geometry.Empty)
            {
                dc.DrawGeometry(Foreground, null, rendered);
            }
        }

        private bool IsGeometryEmpty(Geometry geometry)
        {
            return geometry == null || geometry.IsEmpty() || geometry.Bounds.IsEmpty;
        }

        /// <summary>
        /// Get the rendered geometry.
        /// </summary>
        private Geometry GetRenderedGeometry(Geometry geometry, Matrix matrix)
        {
            Geometry rendered = geometry.CloneCurrentValue();

            if (ReferenceEquals(geometry, rendered))
            {
                rendered = rendered.Clone();
            }

            Transform transform = rendered.Transform;

            if (transform == null)
            {
                rendered.Transform = new MatrixTransform(matrix);
            }
            else
            {
                rendered.Transform = new MatrixTransform(transform.Value * matrix);
            }

            return rendered;
        }

        #region LSBIconScaleX double Comment

        public double LSBIconScaleX
        {
            get => (double)GetValue(LSBIconScaleXProperty);
            set => SetValue(LSBIconScaleXProperty, value);
        }

        public static readonly DependencyProperty LSBIconScaleXProperty =
            DependencyProperty.Register("LSBIconScaleX", typeof(double), typeof(TYIcon), new PropertyMetadata(10d, OnUpdateLayOut));

        #endregion LSBIconScaleX double Comment

        #region LSBIconScaleY double Comment

        public double LSBIconScaleY
        {
            get => (double)GetValue(LSBIconScaleYProperty);
            set => SetValue(LSBIconScaleYProperty, value);
        }

        public static readonly DependencyProperty LSBIconScaleYProperty =
            DependencyProperty.Register("LSBIconScaleY", typeof(double), typeof(TYIcon), new PropertyMetadata(10d, OnUpdateLayOut));

        #endregion LSBIconScaleY double Comment

        /// <summary>
        /// Get the stretch matrix of the geometry.
        /// </summary>
        private Matrix GetStretchMatrix(Geometry geometry)
        {
            Matrix matrix = Matrix.Identity;

            if (geometry != null)
            {
                Rect bounds = geometry.Bounds;
                if (!IsGeometryEmpty(geometry))
                {
                    double m = Math.Min(ActualWidth, ActualHeight);
                    double scaleX = ActualWidth / bounds.Width * Scale;
                    double scaleY = ActualHeight / bounds.Height * Scale;
                    double scaleTmp = Math.Min(scaleX, scaleY);
                    double diffRec = Math.Abs(ActualWidth - ActualHeight);
                    matrix.Translate(
                        (ActualWidth -
                        bounds.Width * scaleTmp) / 2 / scaleTmp,
                        (ActualHeight -
                        bounds.Height * scaleTmp) / 2 / scaleTmp);

                    matrix.Scale(scaleTmp, scaleTmp);
                }
            }

            return matrix;
        }
    }
}