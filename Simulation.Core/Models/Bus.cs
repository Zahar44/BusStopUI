using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class Bus : ISimulationEntityModel
    {
        public Uri PickPath => new Uri(@"D:\projects\BusStopUI\Simulation.Wpf\Source\Bus.png");

        public Size Size => new Size(50, 50);

        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
