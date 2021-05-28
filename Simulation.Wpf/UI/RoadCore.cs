using MvvmCross.ViewModels;
using Simulation.Core.Models;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Animations;
using Simulation.Wpf.Helpers;
using Simulation.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simulation.Wpf.UI
{
    class RoadCore : ISimulationEntityCore, IPickable, IHoldBusesUI
    {
        private const int padding = 20;
        private static MvxObservableCollection<RoadCore> Roads = new MvxObservableCollection<RoadCore>();
        private static bool isLocked = false;
        private readonly IStationPickService pickService;
        private readonly ISimulationService simulationService;
        private readonly Canvas canvas;
        private readonly UserControl first;
        private readonly UserControl second;
        private readonly Road road;
        private IList<BusCore> buses = new List<BusCore>();
        private Line line;
        private UserControl view;

        public UserControl View => view;
        public Canvas Canvas => canvas;

        public UserControl Control { get; set; }

        public ISimulationEntityModel SimulationModel => road;
        public Uri PickPath => throw new NotImplementedException();
        public Size Size => throw new NotImplementedException();

        public int Delay => road.Delay;

        public IHoldBuses Model => road;

        public RoadCore(Canvas _canvas, UserControl _first, UserControl _second, ISimulationService _simulationService, int _length)
        {
            canvas = _canvas;
            first = _first;
            second = _second;
            simulationService = _simulationService;
            road = new Road(StationCore.GetModelBy(first), StationCore.GetModelBy(second), _length);
            view = new RoadView();
            pickService = new DefaultPickService(simulationService, false);

            Activate();
            Roads.Add(this);
        }

        private void Activate()
        {
            line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 5;
            ColisionHelper.CalculateRoadPosition(line, first, second);

            view.Content = line;
            canvas.Children.Add(view);

            if (isLocked)
                Lock();
            else
                Unlock();
        }

        public void Redraw()
        {
            ColisionHelper.CalculateRoadPosition(line, first, second);

            foreach (var bus in buses)
            {
                bus.Redraw();
            }
        }

        public static void LockAll()
        {
            if (isLocked)
                return;

            foreach (var road in Roads)
            {
                road.Lock();
            }
            isLocked = true;
        }

        public static void UnlockAll()
        {
            if (!isLocked)
                return;

            foreach (var road in Roads)
            {
                road.Unlock();
            }
            isLocked = false;
        }

        public static RoadCore FindBy(ISimulationEntityModel model)
        {
            foreach (var road in Roads)
            {
                if (road.SimulationModel == model)
                    return road;
            }
            throw new Exception();
        }

        public void OnPick()
        {
            pickService.Pick(View);
        }

        public void OnDetach()
        {
            pickService.Detach();
        }

        private void Lock()
        {
            pickService.Lock(this);
        }

        private void Unlock()
        {
            pickService.Unlock(this);
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void AddBus(BusCore bus)
        {
            buses.Add(bus);
            var pos = CalculateCoordinate(bus, road.Length, road.Length);
            //double x = CalculateLeft(bus, _simulationModel.Length, _simulationModel.Length);
            //double y = CalculateTop(bus, _simulationModel.Length, _simulationModel.Length);

            bus.View.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(bus.View, pos.X);
                Canvas.SetTop(bus.View, pos.Y);
                if(!canvas.Children.Contains(bus.View))
                    canvas.Children.Add(bus.View);
            });
        }

        public void RemoveBus(BusCore bus)
        {
            buses.Remove(bus);
        }

        public void UpdatePos(BusCore bus, int delay)
        {
            int segments = Delay;
            var pos = CalculateCoordinate(bus, segments, delay);

            //Debug.WriteLine($"Bus #{bus.SimulationModel} set to [{(int)pos.X}:{(int)pos.Y}]");

            bus.View.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(bus.View, pos.X);
                Canvas.SetTop(bus.View, pos.Y);
                if (!canvas.Children.Contains(bus.View))
                    canvas.Children.Add(bus.View);
            });
        }

        private Point CalculateCoordinate(BusCore bus, int segments, int segment)
        {
            double x, y;
            double lX1 = 0, lX2 = 0;
            double lY1 = 0, lY2 = 0;
            double prevX = 0, prevY = 0;
            bool left = false, up = false;

            View.Dispatcher.Invoke(() =>
            {
                lX1 = line.X1; // + padding;
                lX2 = line.X2; // - padding;
                lY1 = line.Y1;
                lY2 = line.Y2;
            });

            bus.Previous.View.Dispatcher.Invoke(() =>
            {
                prevY = Canvas.GetTop(bus.Previous.View);
                prevX = Canvas.GetLeft(bus.Previous.View);
                left = Math.MaxMagnitude(prevX - lX1, prevX - lX2) != prevX - lX1;
                up = Math.MaxMagnitude(prevY - lY1, prevY - lY2) != prevY - lY1;
            });

            x = left ? CalculateCoordinate(lX1, lX2, segments, segment) : CalculateCoordinate(lX2, lX1, segments, segment);
            y = up   ? CalculateCoordinate(lY1, lY2, segments, segment) : CalculateCoordinate(lY2, lY1, segments, segment);
            if(!up && !left)
            {
                x -= padding;
                y += padding;
            }

            if (up && left)
            {
                x += padding;
                y -= padding;
            }

            return new Point(x, y);
        }

        private double CalculateCoordinate(double x1, double x2, int segments, int segment)
        {
            double x = 0, xd = 0, sub = 0;

            View.Dispatcher.Invoke(() =>
            {
                x = x1;
                sub = x1 - x2;
                xd = (sub / segments) * (segments - segment);
            });

            x -= xd;

            return !Double.IsInfinity(x) ? x : x1;
        }

        public void Dispose()
        {
            SimulationModel.Dispose();
            Roads.Remove(this);
            
            Canvas.Children.Remove(View);

            RouteCore.DisposeBy(this);
            GC.SuppressFinalize(this);
        }

        public void Destroy()
        {
            Dispose();
        }
    }
}
