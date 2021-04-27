using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    public interface ISimulationEntity
    {
        Uri PickPath { get; }
    }
}
