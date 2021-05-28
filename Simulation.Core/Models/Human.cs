using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    public class Human : ISimulationEntityModel
    {
        static private int idCnt = 0;
        private int waitingTime;
        public int Id { get; set; }

        public int WaitingTime => waitingTime;

        public int StationA { get; private set; }

        public int StationB { get; private set; }

        public SimulationTimer CreatedAt { get; private set; }
        
        public SimulationTimer DestroyedAt { get; set; }

        public SimulationTimer SitAt { get; set; }

        public Human(int stationA)
        {
            Id = ++idCnt;
            StationA = stationA;
            waitingTime = 0;
            CreatedAt = ShellViewModel.FullTime;

            SetStationB();
        }

        public  void Tick()
        {
            waitingTime++;
        }

        
        private void SetStationB()
        {
            var station = new Random().Next(0, Station.Stations.Count);
            StationB = Station.Stations[station].Id;

            if (StationA == StationB)
                SetStationB();
        }

        public override string ToString() => $"{Id}";


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
