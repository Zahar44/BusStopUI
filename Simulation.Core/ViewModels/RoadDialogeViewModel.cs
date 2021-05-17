using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class RoadDialogeViewModel : MvxViewModel
    {
        private int _length;
        public int Length
        {
            get => _length;
            set { SetProperty(ref _length, value); }
        }
    }
}
