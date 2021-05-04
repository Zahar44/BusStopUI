using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public interface IPickable
    {
        void Create();

        void OnPick();

        void OnDetach();
    }
}
