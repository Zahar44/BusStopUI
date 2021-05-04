using Simulation.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Animations
{
    class DefaultPickService : IStationPickService
    {
        private readonly Canvas canvas;
        private readonly UserControl control;

        public DefaultPickService(Canvas _canvas, UserControl _control)
        {
            canvas = _canvas;
            control = _control;
        }

        public void Detach()
        {
            control.BorderThickness = new Thickness();
            Panel.SetZIndex(control, StationCore.defaultZCoord - 1);

            control.MouseMove -= DragMove;
            control.MouseLeftButtonUp -= PlaceView;
        }

        public void Pick()
        {
            control.BorderThickness = new Thickness(1, 1, 1, 1);
            Panel.SetZIndex(control, StationCore.defaultZCoord + 1);

            control.MouseMove += DragMove;
            control.MouseLeftButtonUp += PlaceView;
        }

        private void DragMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var view = sender as UserControl;
                var cursorPos = Mouse.GetPosition(canvas);
                var colisionHelper = new ColisionHelper(view, cursorPos, canvas, StationCore.Views);

                if (colisionHelper.CanMove() == false)
                    return;

                Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
                Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);
            }
        }

        private void PlaceView(object sender, MouseEventArgs e)
        {
            var view = sender as UserControl;
            var cursorPos = Mouse.GetPosition(canvas);
            var colisionHelper = new ColisionHelper(view, cursorPos, canvas, StationCore.Views);

            Point response = colisionHelper.CalculateOffset();
            cursorPos.Offset(response.X, response.Y);

            Panel.SetZIndex(view, StationCore.defaultZCoord);
            Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
            Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);
        }
    }
}
