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
        public static MvxObservableCollection<Station> Stations = new MvxObservableCollection<Station>();
        static private int idCnt = 0;
        private readonly int humanSpawnChance = 5;
        private readonly Random random;
        private MvxObservableCollection<Human> Humen;
        private MvxObservableCollection<Bus> Buses;
        private MvxObservableCollection<Road> Roads;
        private List<Data> datas;
        private SimulationTimer lastLogetTime;


        public StationViewModel ViewModel { get; set; }

        public List<Data> Datas => datas;

        public int Id { get; set; }

        public int Delay => 5;

        public int HumanCount => Humen.Count;

        public int HumanSpawnChanse => humanSpawnChance;

        public Station()
        {
            Id = ++idCnt;
            random = new Random();
            Humen = new MvxObservableCollection<Human>();
            Buses = new MvxObservableCollection<Bus>();
            Roads = new MvxObservableCollection<Road>();
            datas = new List<Data>();
            lastLogetTime = ShellViewModel.FullTime;

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

        public void DetachRoad(Road road)
        {
            Roads.Remove(road);
        }

        public MvxObservableCollection<Road> GetRoads()
        {
            return Roads;
        }

        public bool AddHuman(Human human)
        {
            Humen.Add(human);
            return true;
        }

        public void DropHuman(Human human)
        {
            Humen.Remove(human);
        }

        public void AddBus(Bus bus)
        {
            Buses.Add(bus);
            bus.RemoveRandomHumen(random.Next(0, 100));
        }

        public void RemoveBus(Bus bus)
        {
            Buses.Remove(bus);
        }

        public void Dispose()
        {
            Stations.Remove(this);
         
            GC.SuppressFinalize(this);
        }

        private void Simulate()
        {
            ViewModel?.UpdateData();
            SpawnHuman();
            DropHumans();

            foreach (var bus in Buses)
            {

                if(random.Next(0, 100) > 25 && Humen.Count > 0)
                {
                    var h = Humen[random.Next(0, HumanCount - 1)];
                    PickHumanByBus(h, bus);
                }
            }

            foreach (var human in Humen)
            {
                human.Tick();
            }

            LogData(ShellViewModel.FullTime.Hour);
        }

        private void LogData(int hour)
        {
            if (lastLogetTime.Hour == hour) return;
            Datas.Add(new Data());
            lastLogetTime = ShellViewModel.FullTime;
            var dataIndex = datas.Count - 1;

            datas[dataIndex].HumanCount = Humen.Count;
            int avr = 0;
            foreach (var human in Humen)
            {
                avr += human.WaitingTime;
            }

            avr = datas[dataIndex].HumanCount > 0 ? avr / Datas[dataIndex].HumanCount : avr;
            datas[dataIndex].AverageHumanWaitingTime = avr;
        }

        private void SpawnHuman()
        {
            if (humanSpawnChance > random.Next(0, 100))
            {
                var human = new Human(Id);
                Humen.Add(human);
                //Debug.WriteLine($"Human #{human} created at station #{this}");
            }
        }

        private void PickHumanByBus(Human human, Bus bus)
        {
            if(bus.AddHuman(human))
            {
                human.SitAt = ShellViewModel.FullTime;
                Humen.Remove(human);
            }
            //Debug.WriteLine($"Human #{human} picked by Bus #{bus}");
        }

        private void DropHumans()
        {
            foreach (var bus in Buses)
            {
                DropHumans(bus);
            }
        }

        private void DropHumans(Bus bus)
        {
            for (int i = 0; i < bus.Humen.Count; i++)
            {
                if (bus.Humen[i].StationB == Id)
                {
                    bus.Humen[i].DestroyedAt = ShellViewModel.FullTime;
                    new Loger().LogHuman(bus.Humen[i]);
                    bus.DropHuman(bus.Humen[i]);
                }
            }
        }


        public override string ToString() => $"{Id}";


        public class Data
        {
            public int HumanCount { get; set; }
            public int AverageHumanWaitingTime { get; set; }
        }
    }
}
