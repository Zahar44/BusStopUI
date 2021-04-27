using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Simulation.Core.ViewModels;

namespace Simulation.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            RegisterAppStart<ShellViewModel>();
        }
    }
}
