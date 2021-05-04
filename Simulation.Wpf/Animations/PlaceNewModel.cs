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
            ShellViewModel.DragService.OnDrop(sender);

            var dataContext = sender as FrameworkElement;

            if (!(dataContext.DataContext is ISimulationService simulationService))
            {
                throw new Exception($"Element {dataContext} not implement {typeof(ISimulationService)}");
            }

            simulationService.SetDragService(new CursorAnimation());
            if(e.ClickCount == 2)
                simulationService.DetachModel();
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
