using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Simulation.Wpf.Helpers
{
    class PlaceNewModel
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(PlaceNewModel), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Canvas ca))
                throw new InvalidOperationException();
            if ((bool)e.NewValue)
            {
                //ca.Drop += Drop;
                ca.MouseLeftButtonDown += Drop;
            }
            else
            {
            }
        }

        private static void Drop(object sender, RoutedEventArgs e)
        {
            var dataContext = sender as FrameworkElement;
            Mouse.OverrideCursor = Cursors.Arrow;

            if (!(dataContext.DataContext is IAttachedToCursor at))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(IAttachedToCursor)}");
            }
            at.OnDrop(DragNewModel.SimulationEntity);

            if (!(dataContext is Canvas))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(Canvas)}");
            }
            TryPositionOnCanvas(dataContext);
        }

        private static void TryPositionOnCanvas(FrameworkElement element)
        {
            try
            {
                var ca = element as Canvas;
                var img = new System.Windows.Controls.Image();
                var drImg = System.Drawing.Image.FromFile(DragNewModel.SimulationEntity.PickPath.AbsolutePath);
                int w = 50, h = 50;

                img.Source = MyResize.Image(drImg, w, h);
                ca.Children.Add(img);

                var position = Mouse.GetPosition(ca);
                Canvas.SetLeft(img, position.X);
                Canvas.SetTop(img, position.Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }
    }
}
