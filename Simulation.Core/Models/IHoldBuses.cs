using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    public interface IHoldBuses
    {
        void AddBus(Bus bus);

        void RemoveBus(Bus bus);

        int Delay { get; }
    }
}
