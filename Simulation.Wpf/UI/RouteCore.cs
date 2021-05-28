using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Simulation.Wpf.UI
{
    class RouteCore : ISimulationEntityCore, IUpdatable, IPickable
    {
        private static List<RouteCore> routes = new List<RouteCore>();
        private readonly Route route;
        private readonly Canvas canvas;
        private List<BusCore> buses = new List<BusCore>();
        private List<IHoldBusesUI> ways = new List<IHoldBusesUI>();

        public UserControl View => throw new NotImplementedException();

        public Canvas Canvas => canvas;

        public ISimulationEntityModel SimulationModel => route;

        public Uri PickPath => throw new NotImplementedException();

        public Size Size => throw new NotImplementedException();


        public RouteCore(List<Station> _ways, int busCnt, int delay)
        {
            SetWays(_ways);
            canvas = (ways.First() as StationCore).Canvas; // -_-
            
            SetBuses(busCnt, delay);
            route = MakeRoute();
            AttachBuses();

            routes.Add(this);
        }

        public static RouteCore FindBy(string routeString)
        {
            return routes.Find(x => x.route.ToString() == routeString);
        }

        public static void DisposeBy(IHoldBusesUI place)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                DisposeBy(place, routes[i]);
            }
        }

        private static void DisposeBy(IHoldBusesUI place, RouteCore route)
        {
            foreach (var way in route.ways)
            {
                if(way == place)
                {
                    route.Dispose();
                }
            }
        }

        public void Dispose()
        {
            routes.Remove(this);
            SimulationModel.Dispose();

            foreach (var bus in buses)
            {
                bus.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public void Update()
        {
            foreach (var bus in buses)
            {
                bus.Update();
            }
        }

        public IHoldBusesUI Next(BusCore bus)
        {
            if(bus.Current is null)
                return ways.First();

            if (bus.IsForward)
            {
                return NextForward(bus);
            }
            else
            {
                return NextBackwards(bus);
            }
        }

        private IHoldBusesUI NextForward(BusCore bus)
        {
            for (int i = 0; i < ways.Count; i++)
            {
                if (BusAt(bus.Current, ways[i]))
                {
                    return i < ways.Count - 1 ? ways[i + 1] : ways.First();
                }
            }
            throw new Exception();
        }

        private IHoldBusesUI NextBackwards(BusCore bus)
        {
            for (int i = ways.Count - 1; i >= 0; i--)
            {
                if (BusAt(bus.Current, ways[i]))
                {
                    return i > 1 ? ways[i - 1] : ways.First();
                }
            }
            throw new Exception();
        }

        private bool BusAt(IHoldBusesUI busUI, IHoldBusesUI other) => busUI == other;

        private void SetWays(List<Station> _ways)
        {
            ways.Add(StationCore.GetModelBy(_ways.First()));
            for (int i = 1; i < _ways.Count; i++)
            {
                var road = Road.Find(_ways[i - 1], _ways[i]);
                var roadUI = RoadCore.FindBy(road);
                ways.Add(roadUI);
                ways.Add(StationCore.GetModelBy(_ways[i]));
            }
            //ways.Add(StationCore.GetModelBy(_ways.Last()));
        }

        private void SetBuses(int size, int delay)
        {
            for (int i = 0; i < size; i++)
            {
                buses.Add(new BusCore(this, delay * i, Canvas));
            }
        }

        private Route MakeRoute()
        {
            IList<Bus> _buses = new List<Bus>();
            foreach (var bus in buses)
            {
                _buses.Add(bus.SimulationModel as Bus);
            }
            List<IHoldBuses> places = new List<IHoldBuses>();

            foreach (var way in ways)
            {
                places.Add(way.Model);
            }

            return new Route(places, _buses, this);
        }

        private void AttachBuses()
        {
            foreach (var bus in buses)
            {
                route.AddBus(bus.SimulationModel  as Bus);
            }
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void OnPick()
        {
            foreach (var bus in buses)
            {
                bus.OnPick();
            }
        }

        public void OnDetach()
        {
            foreach (var bus in buses)
            {
                bus.OnDetach();
            }
        }

        public void Destroy()
        {
            Dispose();
        }
    }
}
