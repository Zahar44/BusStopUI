using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    interface IHoldHumans
    {
        bool AddHuman(Human human);

        void DropHuman(Human human); 
    }
}
