using GalaSoft.MvvmLight.Command;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Simulation.Core.ViewModels
{
    public class ShellViewModel : MvxViewModel, ISimulationService
    {
        private static Thread thread;
        private static double tickTime = 250;
        private const int alterSpeed = 50;
        private const int maxSpeed = 500;
        private const int minSpeed = 100;
        private readonly IMvxNavigationService navigationService;
        private static SimulationTimer timer = new SimulationTimer();

        public IPickable Picked { get; set; }

        public static IAnimationService DragService { get; set; }

        public static SimulationTimer FullTime => timer;

        private object _pauseImage;

        public object PauseImage
        {
            get => _pauseImage;
            set { SetProperty(ref _pauseImage, value); }
        }

        private ObservableCollection<Route> _routes;

        public ObservableCollection<Route> Routes
        {
            get { return _routes; }
            set { SetProperty(ref _routes, value); }
        }


        private string _timerString;

        public string TimerString
        {
            get { return _timerString; }
            set { SetProperty(ref _timerString, value); }
        }


        public bool Paused { get; private set; } = true;

        //public string StationCount => Stations.Count.ToString();


        public ShellViewModel(IMvxNavigationService _navigationService)
        {
            navigationService = _navigationService;
            _timerString = timer.ToString();
            _routes = Route.GetRoutes();
        }

        public void StartSimulation()
        {
            thread = new Thread(new ThreadStart(Simulate));
            thread.IsBackground = true;
            thread.Start();
            Debug.WriteLine($"Simulation started");
        }

        public void PauseSimulation()
        {
            Paused = true;
            Debug.WriteLine($"Simulation paused");
        }

        public void ContinueSimulation()
        {
            Paused = false;
            StartSimulation();
        }

        public void SlowSimulation()
        {
            if (tickTime < maxSpeed)
                tickTime += alterSpeed;
            Debug.WriteLine($"Simulation speed is {1 / tickTime * 1000} sec");
        }

        public void SpeedUpSimulation()
        {
            if (tickTime > minSpeed)
                tickTime -= alterSpeed;
            Debug.WriteLine($"Simulation speed is {1 / tickTime * 1000} sec");
        }

        private void Simulate()
        {
            while (!Paused)
            {
                Thread.Sleep((int)tickTime);
                TimerString = (++timer).ToString();
                Station.SimulateAll();
                Route.SimulateAll();
            }
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
            Picked?.OnDetach();
            Picked = null;
        }

        public void AttachModel(IPickable pickable)
        {
            if (Picked == pickable)
                return;
            Picked = pickable;
            Picked.OnPick();
        }

        public void Action()
        {
            Picked?.OnPick();
        }
    }
}
