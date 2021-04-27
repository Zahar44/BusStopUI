using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Helpers
{
    public class DragNewModel
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(DragNewModel), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        public static ISimulationEntity SimulationEntity { get; set; }

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Button fe))
                throw new InvalidOperationException();
            if ((bool)e.NewValue)
            {
                fe.Click += Click;
            }
            else
            {
            }
        }

        private static void Click(object sender, RoutedEventArgs e)
        {
            var dataContext = sender as FrameworkElement;
            Mouse.OverrideCursor = Cursors.Cross;
            if (!(dataContext.DataContext is IAttachedToCursor at))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(IAttachedToCursor)}");
            }
            else
            {
                SimulationEntity = new Station();
                at.OnAttached(SimulationEntity);
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
