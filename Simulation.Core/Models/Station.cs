using MvvmCross.ViewModels;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Simulation.Core.Models
{
    public class Station : ISimulationEntityModel, IHoldHumans, IHoldBuses
    {
        static private int idCnt = 0;
        //private readonly int humanSpawnRate = 25;
        private readonly int humanSpawnChance = 25;
        private readonly Random random;
        private MvxObservableCollection<Human> Humen = new MvxObservableCollection<Human>();
        private MvxObservableCollection<Bus> Buses = new MvxObservableCollection<Bus>();
        private MvxObservableCollection<Road> Roads = new MvxObservableCollection<Road>();
        public static MvxObservableCollection<Station> Stations = new MvxObservableCollection<Station>();

        public StationViewModel ViewModel { get; set; }

        public int Id { get; set; }

        public int Delay => 5;

        public int HumanCount => Humen.Count;

        public Station()
        {
            Id = ++idCnt;
            random = new Random();

            Stations.Add(this);
        }

        public static void SimulateAll()
        {
            for (int i = 0; i < Stations.Count; i++)
            {
                Stations[i].Simulate();
            }
        }

        public static Station Find(int id)
        {
            return Stations.FirstOrDefault(x => x.Id == id);
        }

        public void AttachRoad(Road road)
        {
            if (road == null)
                return;
            Roads.Add(road);
        }

        public MvxObservableCollection<Road> GetRoads()
        {
            return Roads;
        }

        public void AddHuman(Human human)
        {
            Humen.Add(human);
        }

        public void RemoveHuman(Human human)
        {
            Humen.Remove(human);
        }

        public void AddBus(Bus bus)
        {
            Buses.Add(bus);
        }

        public void RemoveBus(Bus bus)
        {
            Buses.Remove(bus);
        }


        private void Simulate()
        {
            ViewModel?.UpdateData();

            if (humanSpawnChance < random.Next(0, 100))
            {
                SpawnHuman();
            }

            foreach (var bus in Buses)
            {
                if(random.Next(0, 100) > 25 && Humen.Count > 0)
                {
                    var h = Humen[random.Next(0, HumanCount - 1)];
                    PickHumanByBus(h, bus);
                }
            }
        }

        private void SpawnHuman()
        {
            var human = new Human();
            Humen.Add(human);
            //Debug.WriteLine($"Human #{human} created at station #{this}");
        }

        private void PickHumanByBus(Human human, Bus bus)
        {
            bus.AddHuman(human);
            Humen.Remove(human);
            //Debug.WriteLine($"Human #{human} picked by Bus #{bus}");
        }

        public override string ToString() => $"{Id}";
    }
}
