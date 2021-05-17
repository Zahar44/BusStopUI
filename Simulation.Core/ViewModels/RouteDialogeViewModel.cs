using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Simulation.Core.ViewModels
{
    public class RouteDialogeViewModel : MvxViewModel
    {
        private string zeroStationsMessage = "There's no more available stations";
        private List<Station> ways = new List<Station>();
        private ObservableCollection<string> _items = new ObservableCollection<string>();
        
        public IMvxCommand CreateCommand { get; set; }
        
        public ObservableCollection<string> Items
        {
            get
            {
                if (_items.Count == 0)
                    _items.Add(zeroStationsMessage);
                return _items;
            }
            set { SetProperty(ref _items, value); }
        }

        private int _busCount;
        public int BusCount
        {
            get => _busCount;
            set { SetProperty(ref _busCount, value); }
        }

        private string _routeString;

        public string RouteString
        {
            get => _routeString;
            set { SetProperty(ref _routeString, value); }
        }
        public RouteDialogeViewModel()
        {
            CreateCommand = new MvxCommand(Create);
            Init();
        }

        public void Set(object obj)
        {
            try
            {
                int id = int.Parse(obj.ToString());
                _Set(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public List<Station> GetWays() => ways;

        public int GetDelay()
        {
            int delay = 0;
            for (int i = 1; i < ways.Count; i++)
            {
                var road = Road.Find(ways[i - 1], ways[i]);
                delay += road.Delay + ways[i].Delay;
            }

            return delay * 2 / (BusCount + 1);
        }

        private void _Set(int id)
        {
            var station = Station.Find(id);
            if(String.IsNullOrEmpty(RouteString))
                RouteString += $"{station}";
            else
                RouteString += $" -> {station}";
            ways.Add(station);
            SetPossibleRoutes();
        }

        private void SetPossibleRoutes()
        {
            var station = this.ways[this.ways.Count - 1];
            ObservableCollection<string> ways = new ObservableCollection<string>();

            foreach (var road in station.GetRoads())
            {
                var _station = road.First == station ? road.Second : road.First;

                if(this.ways.Count > 1)
                {
                    if (_station == this.ways[this.ways.Count - 2])
                        continue;
                }

                ways.Add(_station.ToString());
            }
            Items = ways;
        }

        private void Create()
        {
            //var route = new Route(ways, BusCount);
        }

        private void Init()
        {
            foreach (var item in Station.Stations)
            {
                Items.Add(item.Id.ToString());
            }

            if (Items.Count > 1)
            {
                Items.Remove(zeroStationsMessage);
            }
        }
    }
}
