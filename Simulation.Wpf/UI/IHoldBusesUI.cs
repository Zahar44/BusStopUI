using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Simulation.Wpf.UI
{
    interface IHoldBusesUI
    {
        int Delay { get; }

        UserControl View { get; }

        IHoldBuses Model { get; }

        void AddBus(BusCore bus);
        void RemoveBus(BusCore bus);
        void UpdatePos(BusCore bus, int delay);
    }
}
