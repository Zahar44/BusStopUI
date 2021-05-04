using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Simulation.Wpf.UI
{
    class RoadCore : ISimulationEntityCore, IPickable
    {
        private UserControl _view;
        private ISimulationEntityModel _simulationModel;
        private Point second;
        private Point first;
        private Canvas canvas;

        public UserControl View => _view;
        public ISimulationEntityModel SimulationModel => _simulationModel;
        public Uri PickPath => throw new NotImplementedException();
        public Size Size => throw new NotImplementedException();

        public RoadCore(Canvas _canvas)
        {
            canvas = _canvas;
        }
        
        public void Create()
        {
            throw new NotImplementedException();
        }

        public void OnPick()
        {
            throw new NotImplementedException();
        }

        public void OnDetach()
        {
            throw new NotImplementedException();
        }
    }
}
