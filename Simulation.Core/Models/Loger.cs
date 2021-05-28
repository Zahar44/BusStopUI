using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Simulation.Core.Models
{
    public class Loger
    {
        private static ExcelPackage ex;
        private static ExcelWorksheet routeSheet;
        private static ExcelWorksheet stationSheet;
        private static ExcelWorksheet humanSheet;
        private static List<Route> routes;
        private static List<Human> humen;
        private int routeCollCount = 1;
        private int routeRowCount = 2;
        private int stationCollCount = 1;
        private int stationRowCount = 2;
        private int humanCollCount = 1;
        private int humanRowCount = 2;

        static Loger()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ex = new ExcelPackage();
            routes = new List<Route>();
            humen = new List<Human>();
        }

        public void LogRoute(Route route)
        {
            routes.Add(route);
        }

        public void LogHuman(Human human)
        {
            humen.Add(human);
        }

        public void MakeLogIntoExcelFile(string _path)
        {
            string path = $"{_path}\\Statistics.xlsx";

            LogRoutes();
            LogStations();
            LogHumans();

            if (!File.Exists(path))
            {
                FileStream file = File.Create(path);
                file.Close();
            }
            ex.SaveAs(new FileInfo(path)); 
        }

        private void LogHumans()
        {
            SetHumanSheet();

            foreach (var human in humen)
            {
                humanSheet.Cells[humanRowCount, 1].Value = human.ToString();
                humanSheet.Cells[humanRowCount, 2].Value = human.StationA;
                humanSheet.Cells[humanRowCount, 3].Value = human.StationB;
                humanSheet.Cells[humanRowCount, 4].Value = human.SitAt - human.CreatedAt;
                humanSheet.Cells[humanRowCount, 5].Value = human.DestroyedAt;
                humanSheet.Cells[humanRowCount, 6].Value = human.DestroyedAt - human.CreatedAt;
                humanRowCount++;
            }
        }

        private void LogRoutes()
        {
            SetRouteSheet();

            foreach (var route in routes)
            {
                routeSheet.Cells[routeRowCount, 1].Value = route.ToString();
                routeSheet.Cells[routeRowCount, 2].Value = route.BusCount;
                routeSheet.Cells[routeRowCount, 3].Value = route.TotalLength;
                routeSheet.Cells[routeRowCount, 4].Value = route.TotalLength / 100 / route.BusCount;
                routeSheet.Cells[routeRowCount, 5].Value = route.TotalHumanSpawnChanse;
                routeSheet.Cells[routeRowCount, 6].Formula = $"=D{routeRowCount}/B{routeRowCount}*E{routeRowCount}";
                routeSheet.Cells[routeRowCount, 6].Calculate();
                routeRowCount++;
            }
        }

        private void SetRouteSheet()
        {
            if (!(routeSheet is null))
            {
                return;
            }

            routeSheet = ex.Workbook.Worksheets.Add("Routes");
            routeSheet.Cells[1, 1].Value = "Route";
            routeSheet.Cells[1, 2].Value = "Buses amount";
            routeSheet.Cells[1, 3].Value = "Total length";
            routeSheet.Cells[1, 4].Value = "Total time";
            routeSheet.Cells[1, 5].Value = "Humans per hour";
            routeSheet.Cells[1, 6].Value = "Сongestion";


            for (int i = 1; i < 7; i++)
            {
                routeSheet.Cells[1, i].AutoFitColumns();
            }
        }

        private void LogStations()
        {
            SetStationSheet();

            foreach (var route in routes)
            {
                foreach (var station in route.Stations)
                {
                    LogStation(station);
                }
            }
        }

        private void LogStation(Station station)
        {
            int hour = 0;
            foreach (var data in station.Datas)
            {
                stationSheet.Cells[stationRowCount, 1].Value = station.ToString();
                stationSheet.Cells[stationRowCount, 2].Value = hour++;
                LogStationData(data);
                stationRowCount++;
            }
        }

        private void LogStationData(Station.Data data)
        {
            stationSheet.Cells[stationRowCount, 3].Value = data.HumanCount;
            stationSheet.Cells[stationRowCount, 4].Value = data.AverageHumanWaitingTime;
        }

        private void SetStationSheet()
        {
            if (!(stationSheet is null))
            {
                return;
            }

            stationSheet = ex.Workbook.Worksheets.Add("Stations");
            stationSheet.Cells[1, 1].Value = "#";
            stationSheet.Cells[1, 2].Value = "hour";
            stationSheet.Cells[1, 3].Value = "Human amount";
            stationSheet.Cells[1, 4].Value = "Avg waiting time";

            for (int i = 1; i < 5; i++)
            {
                stationSheet.Cells[1, i].AutoFitColumns();
            }
        }

        private void SetHumanSheet()
        {
            if (!(humanSheet is null))
            {
                return;
            }

            humanSheet = ex.Workbook.Worksheets.Add("Humans");
            humanSheet.Cells[1, 1].Value = "#";
            humanSheet.Cells[1, 2].Value = "Station A";
            humanSheet.Cells[1, 3].Value = "Station B";
            humanSheet.Cells[1, 4].Value = "Waiting for bus";
            humanSheet.Cells[1, 5].Value = "Arrived";
            humanSheet.Cells[1, 6].Value = "Full time";

            for (int i = 1; i < 7; i++)
            {
                humanSheet.Cells[1, i].AutoFitColumns();
            }
        }
    }
}
