using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Simulation.Core.Models
{
    public class Route : ISimulationEntityModel
    {
        private static int idCnt = 0;
        private static ObservableCollection<Route> routes = new ObservableCollection<Route>();
        private readonly IUpdatable ui;
        private Dictionary<IHoldBuses, IList<Bus>> PlaceBuses = new Dictionary<IHoldBuses, IList<Bus>>();

        public int Id { get; set; }

        public Route(IList<IHoldBuses> places, IList<Bus> buses, IUpdatable _ui)
        {
            Id = ++idCnt;
            routes.Add(this);

            ui = _ui;
            
            Activate(places, buses);
            Debug.WriteLine($"Route #{this} created");
        }

        public static ref ObservableCollection<Route> GetRoutes() => ref routes;

        public static void SimulateAll()
        {
            for (int i = 0; i < routes.Count; i++)
            {
                routes[i].Simulate();
            }
        }
        
        private void Simulate()
        {
            //var t = new Thread(new ThreadStart(ui.Update));
            //t.Start();

            ui.Update();
            foreach (var sb in PlaceBuses)
            {
                MoveBuses(sb.Key, sb.Value);
            }
        }
        
        public void AddBus(Bus bus)
        {
            var first = PlaceBuses.First().Key;
            first.AddBus(bus);
            Debug.WriteLine($"Bus #{bus} created at station #{first}");
        }


        private void MoveBuses(IHoldBuses current, IList<Bus> buses)
        {
            for (int i = 0; i < buses.Count; i++)
            {
                if (!buses[i].Init()) continue;

                buses[i].Tick();
                if (buses[i].Next)
                {
                    var next = NextPlace(current, buses[i].IsForward);
                    MoveBus(current, next, buses[i]);
                    //buses[i].SetDelay(next);
                }
            }
        }

        private void MoveBus(IHoldBuses from, IHoldBuses to, Bus bus)
        {
            from.RemoveBus(bus);
            PlaceBuses[from].Remove(bus);

            to.AddBus(bus);
            PlaceBuses[to].Add(bus);

            bus.SetDelay(NextPlace(to, bus.IsForward).Delay);
            SetIsForward(bus, to);

            Debug.WriteLine($"Bus #{bus} moved from station {from} to {to}");
        }

        private IHoldBuses NextPlace(IHoldBuses current, bool forward)
        {
            if(forward && current != PlaceBuses.Last().Key)
            {
                return _NextStationForword(current);
            }
            else
            {
                return _NextStationBackwards(current);
            }
        }

        private IHoldBuses _NextStationForword(IHoldBuses current)
        {
            bool next = false;
            foreach (var sb in PlaceBuses)
            {
                if (next)
                    return sb.Key;
                if (sb.Key == current)
                    next = true;
            }
            if (next)
                return PlaceBuses.First().Key;
            throw new IndexOutOfRangeException();
        }

        private IHoldBuses _NextStationBackwards(IHoldBuses current)
        {
            bool next = false;
            for (int i = PlaceBuses.Count - 1; i >= 0; i--)
            {
                if (next)
                    return PlaceBuses.ElementAt(i).Key;
                if (PlaceBuses.ElementAt(i).Key == current)
                    next = true;
            }
            if (next)

                return PlaceBuses.First().Key;
            throw new IndexOutOfRangeException();
        }

        private void Activate(IList<IHoldBuses> places, IList<Bus> buses)
        {
            foreach (var place in places)
            {
                PlaceBuses.Add(place, new List<Bus>());
            }

            var first = PlaceBuses.First();
            foreach (var bus in buses)
            {
                first.Value.Add(bus);
                var delay = NextPlace(first.Key, true).Delay;
                bus.SetDelay(delay);
            }
        }

        private void SetIsForward(Bus bus, IHoldBuses station)
        {
            if(station == PlaceBuses.First().Key)
            {
                bus.IsForward = true;
            }
            if(station == PlaceBuses.Last().Key)
            {
                bus.IsForward = false;
            }
        }

        private string BuildName()
        {
            StringBuilder res = new StringBuilder();
            res.Append(PlaceBuses.First().Key.ToString());
            
            for (int i = 1; i < PlaceBuses.Count - 1; i++)
            {
                if (i % 2 != 0) continue;
                res.Append(" -> ");
                res.Append(PlaceBuses.ElementAt(i).Key.ToString());
            }
            
            res.Append(" -> ");
            res.Append(PlaceBuses.Last().Key.ToString());
            return res.ToString();
        }

        public override string ToString() => $"{BuildName()}";
    }
}
