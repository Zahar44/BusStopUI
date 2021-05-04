using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Helpers
{
    class CursorAnimation : IAnimationService
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(CursorAnimation), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
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

            StationCore.UnlockAll();

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }
            
            simulationService.SetDragService(new CursorAnimation());
        }
        public void OnDrop(object sender)
        {
            //if (DragStation.SimulationEntityCore == null)
              //  return;
            //(DragStation.SimulationEntityCore as StationCore).OnDetach();
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
