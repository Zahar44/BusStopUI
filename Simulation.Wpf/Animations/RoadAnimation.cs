using Simulation.Core.ViewModels;
using Simulation.Wpf.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Helpers
{
    class RoadAnimation : IAnimationService
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(RoadAnimation), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        private static RoadCore RoadCore;

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
                fe.Click -= Click;
            }
        }

        private static void Click(object sender, RoutedEventArgs e)
        {
            var dataContext = sender as FrameworkElement;
            Mouse.OverrideCursor = Cursors.Pen;

            StationCore.LockAll();

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }

            simulationService.SetDragService(new RoadAnimation());
            simulationService.DetachModel();
        }
        public void OnDrop(object sender)
        {
            var dataContext = sender as FrameworkElement;
            //Mouse.OverrideCursor = Cursors.Arrow;

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }

            RoadCore ??= new RoadCore(sender as Canvas);


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
