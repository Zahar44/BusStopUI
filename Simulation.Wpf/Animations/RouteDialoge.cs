using MvvmCross.Core.ViewModels;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Animations;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Helpers
{
    class RouteDialoge
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(RouteDialoge), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

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
            Mouse.OverrideCursor = Cursors.Arrow;

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }
            var vm = new RouteDialogeViewModel();
            var content = new RouteDialogeView
            {
                DataContext = vm,
            };

            Dialoge.ShowDialog(content, "Route builder");

            simulationService.SetDragService(new CursorAnimation());
            StationCore.SetStationPickService(new DefaultPickService(simulationService));
            StationCore.UnlockAll();
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
