using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Simulation.Core.Models
{
    public class Road : ISimulationEntityModel, IHoldBuses
    {
        private static int idCnt = 0;
        private List<Bus> buses = new List<Bus>();
        private Station first;
        private Station second;
        public int Id { get; set; }

        public int Length { get; set; }

        public int Delay => Length / 100;

        public Station First 
        {
            get => first;
            private set
            {
                first = value;
                first.AttachRoad(this);
            } 
        }
        public Station Second
        {
            get => second;
            private set
            {
                second = value;
                //second.AttachRoad(this);
            }
        }

        public Road(ISimulationEntityModel f, ISimulationEntityModel s, int _length)
        {
            if (!(f is ISimulationEntityModel) || !(s is ISimulationEntityModel))
                throw new InvalidCastException();

            Id = ++idCnt;
            Length = _length;
            First = (Station)f;
            Second = (Station)s;
            Debug.WriteLine($"Road with length: {Length} created between {f} and {s}");
        }

        public static Road Find(Station first, Station second)
        {
            foreach (var road1 in first.GetRoads())
            {
                foreach (var road2 in second.GetRoads())
                {
                    if (road1 == road2)
                        return road1;
                }
            }
            throw new Exception();
        }

        public void AddBus(Bus bus)
        {
            buses.Add(bus);
        }

        public void RemoveBus(Bus bus)
        {
            buses.Remove(bus);
        }

        public override string ToString() => $"{Id}";
    }
}
