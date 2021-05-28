using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class BusViewModel : MvxViewModel
    {
        private static int idCnt = 0;
        private readonly Bus bus;
        private int humanCount;
        private int _id;
        private string _name;
        private bool _picked;

        public string Name => bus.Id.ToString();

        public int HumansCount
        {
            get => humanCount;
            set { SetProperty(ref humanCount, value); }
        }

        public bool Picked { get => _picked; set => _picked = value; }

        public BusViewModel(Bus _bus)
        {
            bus = _bus;
            bus.Data = this;
        }

        internal void UpdateData()
        {
            HumansCount = bus.Humen.Count;
        }

    }
}
