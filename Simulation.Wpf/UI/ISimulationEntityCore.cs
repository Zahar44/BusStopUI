using MvvmCross.ViewModels;
using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Simulation.Wpf.Helpers
{
    internal class ViewDataContext
    {
        public UserControl View;
        public MvxViewModel Data;

        public ViewDataContext(UserControl _view, MvxViewModel _pickable)
        {
            View = _view;
            Data = _pickable;
        }
    }

    public interface ISimulationEntityCore
    {
        UserControl View { get; }

        ISimulationEntityModel SimulationModel { get; }
        
        Uri PickPath { get; }
        
        Size Size { get; }
    }
}
