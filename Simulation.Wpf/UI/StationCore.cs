using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Animations;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Simulation.Wpf.Helpers
{
    class StationCore : ISimulationEntityCore, IPickable
    {
        private static UserControl picked;
        private static bool isLocked = false;
        private readonly Canvas canvas;
        private readonly ISimulationService simulationService;
        private readonly ISimulationEntityModel _simulationEntity;
        private IStationPickService pickService;
        public static readonly int defaultZCoord = 2;
        public static MvxObservableCollection<UserControl> Views = new MvxObservableCollection<UserControl>();
        public static List<StationCore> Stations = new List<StationCore>();

        public UserControl View { get; private set; }

        public Uri PickPath => new Uri(@"D:\projects\BusStopUI\Simulation.Wpf\Source\Station.png");

        public Size Size => new Size(50, 50);


        public ISimulationEntityModel SimulationModel => _simulationEntity;

        public StationCore(Canvas ca, ISimulationService ss)
        {
            if (ca == null || ss == null)
                throw new NullReferenceException();

            _simulationEntity = new Station();
            simulationService = ss;
            canvas = ca;

            SetUp();
        }
        
        public static void SetStationPickService(IStationPickService service)
        {
            foreach (var station in Stations)
            {
                station.pickService = service;
            }
        }

        public static void LockAll()
        {
            if (isLocked)
                return;

            foreach (var station in Stations)
            {
                station.Lock();
            }
            isLocked = true;
        }
        
        public static void UnlockAll()
        {
            if (!isLocked)
                return;

            foreach (var station in Stations)
            {
                station.Unlock();
            }
            isLocked = false;
        }

        public void Create()
        {
            canvas.Children.Add(View);

            View.Measure(Size);
            View.Arrange(new Rect(0, 0, View.DesiredSize.Width, View.DesiredSize.Height));

            PlaceView(View, new MouseEventArgs(Mouse.PrimaryDevice, (int)DateTime.Now.Ticks));
        }

        public void OnPick()
        {
            pickService.Pick();
            picked = View;
        }

        public void OnDetach()
        {
            pickService.Detach();
            picked = null;
        }

        private void Lock()
        {
            View.MouseDoubleClick -= SelectElement;
        }

        private void Unlock()
        {
            View.MouseDoubleClick += SelectElement;
        }

        private void DragMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var view = sender as UserControl;
                var cursorPos = Mouse.GetPosition(canvas);
                var colisionHelper = new ColisionHelper(view, cursorPos, canvas, Views);

                if(colisionHelper.CanMove() == false)
                    return;
                
                Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
                Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);
            }
        }

        private void PlaceView(object sender, MouseEventArgs e)
        {
            var view = sender as UserControl;
            var cursorPos = Mouse.GetPosition(canvas);
            var colisionHelper = new ColisionHelper(view, cursorPos, canvas, Views);

            Point response  = colisionHelper.CalculateOffset();
            cursorPos.Offset(response.X, response.Y);

            Panel.SetZIndex(view, defaultZCoord);
            Canvas.SetLeft(view, cursorPos.X - view.ActualWidth / 2);
            Canvas.SetTop(view, cursorPos.Y - view.ActualHeight / 2);
        }

        private void SelectElement(object sender, MouseButtonEventArgs e)
        {
            var view = sender as UserControl;
            bool pick = picked == null ? true : false;

            simulationService.DetachModel();

            if (pick)
            {
                simulationService.AttachModel(this);
            }
        }
        private void SetUp()
        {
            View = new StationView();
            pickService = new DefaultPickService(canvas, View);

            if (isLocked)
                Lock();
            else
                Unlock();

            Views.Add(View);
            Stations.Add(this);
        }
    }
}
