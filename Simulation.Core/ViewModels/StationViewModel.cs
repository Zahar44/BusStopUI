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
        private int _id;
        private string _name;
        private bool _picked;
        private static MvxObservableCollection<Station> stations = new MvxObservableCollection<Station>();

        public string Name => _name;

        public bool Picked { get => _picked; set => _picked = value; }

        public StationViewModel()
        {
            _id = ++idCnt;
            _name = _id.ToString();
            _picked = false;
        }

        public void OnPick(IPickable pickable)
        {
            pickable.OnPick();
            Picked = true;
        }

        public void OnDetach(IPickable pickable)
        {
            pickable.OnDetach();
            Picked = false;
        }

        internal static void Attach(Station station)
        {
            stations.Add(station);
        }
    }
}
