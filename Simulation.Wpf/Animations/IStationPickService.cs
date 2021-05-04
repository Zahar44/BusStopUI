using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Simulation.Wpf.Animations
{
    interface IStationPickService
    {
        void Pick();

        void Detach();
    }
}
