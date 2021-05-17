using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Animations;
using Simulation.Wpf.UI;
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
    class StationCore : ISimulationEntityCore, IPickable, IHoldBusesUI
    {
        private static bool isLocked = false;
        private readonly ISimulationService simulationService;
        private readonly Station station;
        private MvxObservableCollection<RoadCore> roads = new MvxObservableCollection<RoadCore>();
        private MvxObservableCollection<BusCore> buses = new MvxObservableCollection<BusCore>();
        private IStationPickService pickService;
        public static readonly int defaultZCoord = 2;
        public static MvxObservableCollection<UserControl> Views = new MvxObservableCollection<UserControl>();
        public static MvxObservableCollection<StationCore> Stations = new MvxObservableCollection<StationCore>();
        public readonly Canvas canvas;

        public UserControl View { get; private set; }

        public Uri PickPath => new Uri(@"D:\projects\BusStopUI\Simulation.Wpf\Source\Station.png");

        public Size Size => new Size(50, 50);


        public ISimulationEntityModel SimulationModel => station;

        public Canvas Canvas => canvas;

        public int Delay => station.Delay;

        public IHoldBuses Model => station;

        public StationCore(Canvas ca, ISimulationService ss)
        {
            station = new Station();
            simulationService = ss;
            canvas = ca;

            SetUp();
        }

        public static ISimulationEntityModel GetModelBy(UserControl control)
        {
            foreach (var item in Stations)
            {
                if (item.View == control)
                    return item.SimulationModel;
            }
            throw new Exception();
        }

        public static StationCore GetModelBy(ISimulationEntityModel model)
        {
            foreach (var item in Stations)
            {
                if (item.SimulationModel == model)
                    return item;
            }
            throw new Exception();
        }

        public static void SetStationPickService(IStationPickService service)
        {
            foreach (var station in Stations)
            {
                station.Lock();
                station.pickService.Detach();
                station.pickService = service.Copy();
                station.Unlock();
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
            pickService.Pick(View);
        }

        public void OnDetach()
        {
            pickService.Detach();
        }


        public void AddBus(BusCore bus)
        {
            buses.Add(bus);

            double x = 0, y = 0;
            View.Dispatcher.Invoke(() =>
            {
                x = Canvas.GetLeft(View);
                y = Canvas.GetTop(View);
            });

            y -= 25;
            x += 25;

            bus.View.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(bus.View, x);
                Canvas.SetTop(bus.View, y);
                if (!canvas.Children.Contains(bus.View))
                    canvas.Children.Add(bus.View);
            });
        }

        public void RemoveBus(BusCore bus)
        {
            buses.Remove(bus);
        }

        public void UpdatePos(BusCore bus, int delay) {}

        public void AddRoadToEach(RoadCore road)
        {
            if(road.SimulationModel is Road r)
            {
                GetModelBy(r.First).roads.Add(road);
                GetModelBy(r.Second).roads.Add(road);
            }
            else
            {
                throw new Exception("Can't add road to second station");
            }
        }

        public void Redraw()
        {
            foreach (var road in roads)
            {
                road.Redraw();
            }

            foreach (var bus in buses)
            {
                bus.Redraw();
            }
        }

        private void Lock()
        {
            pickService.Lock(this);
        }

        private void Unlock()
        {
            pickService.Unlock(this);
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

        private void SetUp()
        {
            var Data = new StationViewModel(SimulationModel);
            View = new StationView
            {
                DataContext = Data
            };
            pickService = new DefaultPickService(simulationService);

            if (isLocked)
                Lock();
            else
                Unlock();

            Views.Add(View);
            Stations.Add(this);
        }
    }
}
