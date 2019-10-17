using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BusyIndicator {
    public class Indicator : UserControl {

        private List<Path> paths = new List<Path>();

        private static SolidColorBrush DefaultStrokeBrush=new SolidColorBrush(Colors.DarkGreen);
        private static SolidColorBrush DefaultFillBrush=new SolidColorBrush(Colors.LawnGreen);

        /// <summary>
        /// 添加依赖属性，方便在xaml中改变对应属性
        /// </summary>
        public Brush StrokBrush {
            get { return (Brush)GetValue(StrokBrushProperty); }
            set { SetValue(StrokBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokBrushProperty =
            DependencyProperty.Register("StrokBrush", typeof(Brush), typeof(Indicator), new PropertyMetadata(DefaultStrokeBrush, StrokeChanged));

        private static void StrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Indicator owner = (Indicator)d;
            var brush = e.NewValue as Brush;
            if (brush != null) owner.SetStrokeBrush(brush);
        }



        public Brush FillBrush {
            get { return (Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FillBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register("FillBrush", typeof(Brush), typeof(Indicator), new PropertyMetadata(DefaultFillBrush, FillBrushChanged));

   


        private static void FillBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Indicator owner = (Indicator)d;
            var brush = e.NewValue as Brush;
            if (brush != null) {
                owner.SetFillBrush(brush);
            }
        }


        private void SetStrokeBrush(Brush brush) {
            foreach (var path in paths) {
                path.Stroke = brush;
            }
        }

        private void SetFillBrush(Brush brush) {
            foreach (var path in paths) {
                path.Fill = brush;
            }
        }


        public Indicator() {
            InitialComponent();
        }

        private void InitialComponent() {
            Grid grid = new Grid();
            this.Content = grid;

            for (int i = 0; i < 12; i++) {
                Path p = new Path();
                //Z表示闭合，M表示开始，L表示直线
                p.Data = Geometry.Parse("M 0,0 L -10,0 L -10,-60 L 0 ,-70 L 10 ,-60 L 10,0 Z");
                DefaultStrokeBrush = new SolidColorBrush(Colors.DarkGreen);
                p.Stroke = DefaultStrokeBrush;
                p.StrokeThickness = 1;
                DefaultFillBrush = new SolidColorBrush(Colors.LawnGreen);
                p.Fill = DefaultFillBrush;
                p.Opacity = 0.2;

                TransformGroup tg = new TransformGroup();
                p.RenderTransform = tg;
                //偏移
                TranslateTransform tt = new TranslateTransform();
                tt.Y = -50;
                //旋转
                RotateTransform rt = new RotateTransform();
                rt.Angle = i * 30;
                tg.Children.Add(tt);
                tg.Children.Add(rt);

                //添加动画
                DoubleAnimation da = new DoubleAnimation();
                da.From = 1.0;
                da.To = 0.2;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.BeginTime = TimeSpan.FromMilliseconds(i * 1000.0 / 12);  //交错感
                da.RepeatBehavior = RepeatBehavior.Forever;

                grid.Children.Add(p);
                p.BeginAnimation(Path.OpacityProperty, da);
                paths.Add(p);
            }
        }
    }
}
