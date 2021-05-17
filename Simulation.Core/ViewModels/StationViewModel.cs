using MvvmCross.ViewModels;
using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class StationViewModel : MvxViewModel
    {
        private static int idCnt = 0;
        private readonly Station station;
        private int humanCount;
        private int _id;
        private string _name;
        private bool _picked;

        public string Name => station.Id.ToString();

        public int HumansCount
        {
            get => humanCount;
            set { SetProperty(ref humanCount, value); }
        }

        public bool Picked { get => _picked; set => _picked = value; }

        public StationViewModel(ISimulationEntityModel _station)
        {
            if (!(_station is Station))
                throw new InvalidCastException();

            _id = ++idCnt;
            _name = _id.ToString();
            _picked = false;
            station = (Station)_station;
            station.ViewModel = this;
            HumansCount = station.HumanCount;
        }

        internal void UpdateData()
        {
            HumansCount = station.HumanCount;
        }
    }
}
