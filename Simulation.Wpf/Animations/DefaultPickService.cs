using Simulation.Core.ViewModels;
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
        private readonly ISimulationService simulationService;
        private readonly bool canMove;
        private ISimulationEntityCore core;
        private UserControl control;
        private bool picked = false;

        public DefaultPickService(ISimulationService _simulationService, bool _canMove = true)
        {
            simulationService = _simulationService;
            canMove = _canMove;
        }

        public void Detach()
        {
            if (control == null)
                return;

            control.BorderThickness = new Thickness();
            
            if(canMove)
            {
                DetachWithMove();
            }

            picked = false;
        }

        public void Pick(UserControl _control)
        {
            control = _control;
            control.BorderThickness = new Thickness(1, 1, 1, 1);

            if(canMove)
            {
                PickWithMove();
            }
            picked = true;
        }

        private void PickWithMove()
        {
            Panel.SetZIndex(control, StationCore.defaultZCoord + 1);

            control.MouseMove += DragMove;
            control.MouseLeftButtonUp += PlaceView;
        }

        private void DetachWithMove()
        {
            Panel.SetZIndex(control, StationCore.defaultZCoord - 1);

            control.MouseMove -= DragMove;
            control.MouseLeftButtonUp -= PlaceView;
        }

        public void Lock(ISimulationEntityCore _core)
        {
            core = _core;
            core.View.MouseDoubleClick -= SelectElement;
        }

        public void Unlock(ISimulationEntityCore _core)
        {
            core = _core;
            core.View.MouseDoubleClick += SelectElement;
        }

        private void SelectElement(object sender, MouseButtonEventArgs e)
        {
            simulationService.DetachModel();

            if (!picked)
            {
                if(core is IPickable)
                    simulationService.AttachModel(core as IPickable);
            }
        }

        private void DragMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var view = sender as UserControl;
                var cursorPos = Mouse.GetPosition(core.Canvas);
                var colisionHelper = new ColisionHelper(view, cursorPos, core.Canvas, StationCore.Views);

                if (colisionHelper.CanMove() == false)
                    return;

                Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
                Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);

                // Make in proper!!!
                if(core is StationCore)
                {
                    (core as StationCore).Redraw();
                }
            }
        }

        private void PlaceView(object sender, MouseEventArgs e)
        {
            var view = sender as UserControl;
            var cursorPos = Mouse.GetPosition(core.Canvas);
            var colisionHelper = new ColisionHelper(view, cursorPos, core.Canvas, StationCore.Views);

            Point response = colisionHelper.CalculateOffset();
            cursorPos.Offset(response.X, response.Y);

            Panel.SetZIndex(view, StationCore.defaultZCoord);
            Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
            Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);
        }

        public IStationPickService Copy()
        {
            return (IStationPickService)MemberwiseClone();
        }
    }
}
