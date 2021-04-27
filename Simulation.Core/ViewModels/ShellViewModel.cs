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
    class ShellViewModel : MvxViewModel, IAttachedToCursor
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public IMvxCommand MouseDownCommand { get; set; }

        public ShellViewModel()
        {
            MouseDownCommand = new MvxCommand(MouseDown);
        }

        private void MouseDown()
        {
            
        }

        public void OnAttached(ISimulationEntity se)
        {

        }

        public void OnDrop(ISimulationEntity se)
        {
        }
    }
}
