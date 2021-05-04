using GalaSoft.MvvmLight.Command;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace Simulation.Core.ViewModels
{
    public class ShellViewModel : MvxViewModel, ISimulationService
    {
        private MvxObservableCollection<Station> Stations;
        private IPickable picked;

        public static IAnimationService DragService { get; set; }

        //public string StationCount => Stations.Count.ToString();

        public ShellViewModel()
        {
            Stations = new MvxObservableCollection<Station>();
        }

        public void SetDragService(IAnimationService ds)
        {
            DragService = ds;
        }

        public void SetDefaultDragAction(IAnimationService dragDefault)
        {
            DragService = dragDefault;
        }

        public void CreateModel(IPickable pickable)
        {
            pickable.Create();
        }

        public void DetachModel()
        {
            picked?.OnDetach();
            picked = null;
        }

        public void AttachModel(IPickable pickable)
        {
            picked = pickable;
            picked.OnPick();
        }
    }
}
