using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simulation.Wpf.Helpers
{
    public class StationAnimation : IAnimationService
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(StationAnimation), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        private static StationCore StationCore;

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
            Mouse.OverrideCursor = Cursors.Cross;

            StationCore.LockAll();

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }
            
            simulationService.SetDragService(new StationAnimation());
            simulationService.DetachModel();
        }
        public void OnDrop(object sender)
        {
            var element = sender as FrameworkElement;
            Mouse.OverrideCursor = Cursors.Arrow;

            if (!(element.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {element} not implement {typeof(ISimulationService)}");
            }

            StationCore = new StationCore(sender as Canvas, simulationService);
            simulationService.CreateModel(StationCore);
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
