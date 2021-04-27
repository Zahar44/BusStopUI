using Simulation.Core.Models;

namespace Simulation.Core.ViewModels
{
    public interface IAttachedToCursor
    {
        void OnAttached(ISimulationEntity se);

        void OnDrop(ISimulationEntity se);
    }
}
