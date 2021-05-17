using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Helpers
{
    class MovePanel
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(MovePanel), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Canvas ca))
                throw new InvalidOperationException();
            if ((bool)e.NewValue)
            {
                ShellViewModel.DragService = new CursorAnimation();
                ca.PreviewMouseLeftButtonDown += Drop;
            }
            else
            {
                ca.PreviewMouseLeftButtonDown -= Drop;
            }
        }
        private static void Drop(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            Canvas.SetLeft(canvas, 100);
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
