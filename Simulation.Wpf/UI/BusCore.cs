using MvvmCross.Platforms.Wpf.Views;
using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Simulation.Wpf.UI
{
    class BusCore : ISimulationEntityCore, IUpdatable
    {
        private readonly RouteCore route;
        private readonly UserControl view;
        private readonly Bus bus;
        private int delay;

        public IHoldBusesUI Previous { get; set; }
        public IHoldBusesUI Current { get; set; }

        public bool IsForward => bus.IsForward;

        public UserControl View => view;

        public Canvas Canvas => throw new NotImplementedException();

        public ISimulationEntityModel SimulationModel => bus;

        public Uri PickPath => throw new NotImplementedException();

        public Size Size => throw new NotImplementedException();

        public BusCore(RouteCore _route, int _delay)
        {
            delay = _delay;
            route = _route;
            Current = null;
            view = new BusView();
            bus = new Bus(delay);

            //(route.SimulationModel as Route).AddBus(bus);
            //Current.AddBus(this);
        }

        public void Redraw()
        {
            Current?.UpdatePos(this, delay);
        }

        public void Update()
        {
            delay--;
            Redraw();
            if (delay < 0)
            {
                Move();
                delay = Current.Delay;
            }
        }

        private void Move()
        {
            //Current.RemoveBus(this);
            Previous = Current;
            Previous?.RemoveBus(this);
            Current = route.Next(this);
            Current.AddBus(this);
        }
    }
}
