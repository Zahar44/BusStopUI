using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Simulation.Wpf.Animations
{
    class RoadPickService : IStationPickService
    {
        private readonly Canvas canvas;
        private readonly UserControl control1;
        private readonly UserControl control2;

        public RoadPickService()
        {

        }

        public void Detach()
        {
            throw new NotImplementedException();
        }

        public void Pick()
        {
            throw new NotImplementedException();
        }
    }
}
