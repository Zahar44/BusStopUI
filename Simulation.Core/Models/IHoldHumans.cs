using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    interface IHoldHumans
    {
        void AddHuman(Human human);

        void RemoveHuman(Human human); 
    }
}
