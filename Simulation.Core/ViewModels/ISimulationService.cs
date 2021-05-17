using Simulation.Core.Models;

namespace Simulation.Core.ViewModels
{
    public interface ISimulationService
    {
        void SetDefaultDragAction(IAnimationService dragDefault);

        void SetDragService(IAnimationService se);

        void CreateModel(IPickable pickable);

        void AttachModel(IPickable pickable);

        void DetachModel();

        void Action();
    }
}
