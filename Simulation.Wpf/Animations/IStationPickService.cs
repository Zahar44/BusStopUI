using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Simulation.Wpf.Animations
{
    interface IStationPickService
    {
        void Pick(UserControl control);

        void Detach();

        void Lock(ISimulationEntityCore core);

        void Unlock(ISimulationEntityCore core);

        IStationPickService Copy();
    }
}
