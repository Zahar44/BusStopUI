using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Simulation.Core.Models
{
    public interface ISimulationEntityModel : IDisposable
    {
        int Id { get; set; }
    }
}
