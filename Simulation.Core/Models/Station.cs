using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Simulation.Core.Models
{
    public class Station : ISimulationEntityModel
    {
        static private int idCnt = 0;
        public int Id { get; set; }

        public Station()
        {
            Id = ++idCnt;
            StationViewModel.Attach(this);
        }

    }
}
