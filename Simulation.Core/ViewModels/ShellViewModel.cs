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
        private IPickable picked;
        
        public static IAnimationService DragService { get; set; }

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


        public static bool Paused { get; private set; } = true;

        //public string StationCount => Stations.Count.ToString();

        public ShellViewModel(IMvxNavigationService _navigationService)
        {
            navigationService = _navigationService;
            _routes = Route.GetRoutes();
        }

        public static void StartSimulation()
        {
            thread = new Thread(new ThreadStart(Simulate));
            thread.IsBackground = true;
            thread.Start();
            Debug.WriteLine($"Simulation started");
        }

        public static void PauseSimulation()
        {
            Paused = true;
            Debug.WriteLine($"Simulation paused");
        }

        public static void ContinueSimulation()
        {
            Paused = false;
            StartSimulation();
        }

        public static void SlowSimulation()
        {
            if (tickTime < maxSpeed)
                tickTime += alterSpeed;
            Debug.WriteLine($"Simulation speed is {1 / tickTime * 1000} sec");
        }

        public static void SpeedUpSimulation()
        {
            if (tickTime > minSpeed)
                tickTime -= alterSpeed;
            Debug.WriteLine($"Simulation speed is {1 / tickTime * 1000} sec");
        }

        private static void Simulate()
        {
            while (!Paused)
            {
                Thread.Sleep((int)tickTime);
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
            picked?.OnDetach();
            picked = null;
        }

        public void AttachModel(IPickable pickable)
        {
            if (picked == pickable)
                return;
            picked = pickable;
            picked.OnPick();
        }

        public void Action()
        {
            picked?.OnPick();
        }

        
    }
}
