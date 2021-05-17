using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using Simulation.Wpf.UI;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simulation.Wpf.Animations
{
    class RoadPickService : IStationPickService
    {
        private static UserControl first;
        private static UserControl second;
        private static bool firstSelected = false;
        private static bool secondSelected = false;

        private readonly ISimulationService simulationService;
        private ISimulationEntityCore core;

        public RoadPickService(ISimulationService _simulationService)
        {
            simulationService = _simulationService;
        }

        public void Detach()
        {
            if (first != null)
            {
                first.BorderThickness = new Thickness();
                Panel.SetZIndex(first, StationCore.defaultZCoord - 1);
                firstSelected = false;
                first = null;
            }

            if (second != null)
            {
                second.BorderThickness = new Thickness();
                Panel.SetZIndex(second, StationCore.defaultZCoord - 1);
                secondSelected = false;
                second = null;
            }
        }

        public void Pick(UserControl control)
        {
            first ??= control;
            if (first != null && !firstSelected)
            {
                first.BorderThickness = new Thickness(1, 1, 1, 1);
                Panel.SetZIndex(first, StationCore.defaultZCoord + 1);
                firstSelected = true;
                return;
            }            
            
            second ??= control;
            if (second != null && !secondSelected)
            {
                second.BorderThickness = new Thickness(1, 1, 1, 1);
                Panel.SetZIndex(second, StationCore.defaultZCoord + 1);
                secondSelected = true;
            }

            MakeRoad();
            Detach();
        }

        private void MakeRoad()
        {
            var data = new RoadDialogeViewModel();
            var view = new RoadDialogeView
            {
                DataContext = data,
                ViewModel = data
            };
            
            Mouse.OverrideCursor = Cursors.Arrow;
            Dialoge.ShowDialog(view, "Road builder");
            Mouse.OverrideCursor = Cursors.Pen;

            if (data.Length == 0)
            {
                return;
            }

            var road = new RoadCore(core.Canvas, first, second, simulationService, data.Length);

            try
            {
                (core as StationCore).AddRoadToEach(road);
                (core.SimulationModel as Station).AttachRoad(road.SimulationModel as Road);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Lock(ISimulationEntityCore _core)
        {
            core = _core;
            core.View.MouseLeftButtonDown -= SelectElement;
        }

        public void Unlock(ISimulationEntityCore _core)
        {
            core = _core;
            core.View.MouseLeftButtonDown += SelectElement;
        }

        private void SelectElement(object sender, MouseButtonEventArgs e)
        {
            if(core is IPickable)
                simulationService.AttachModel(core as IPickable);
        }

        public IStationPickService Copy()
        {
            return (IStationPickService)MemberwiseClone();
        }
    }
}
