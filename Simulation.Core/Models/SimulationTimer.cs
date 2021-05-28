using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    public class SimulationTimer
    {
        int min;
        int hour;

        public int Hour => hour;

        public int Minute => min;


        public SimulationTimer()
        {
            min = 0;
            hour = 0;
        }

        public static SimulationTimer operator ++(SimulationTimer t)
        {
            var res = new SimulationTimer();
            res.min = t.min + 1;
            res.hour = t.hour;
            if (res.min > 59)
            {
                res.min = 0;
                res.hour++;
                if(res.hour > 23)
                {
                    res.hour = 0;
                }
            }
            return res;
        }

        //public static Timer operator --(Timer t)
        //{
        //    var res = new Timer();
        //    res.min = t.min - 1;
        //    res.hour = t.hour;
        //    if (res.min < 0)
        //    {
        //        res.min = 0;
        //        res.hour++;
        //        if (res.hour > 24)
        //        {
        //            res.hour = 0;
        //        }
        //    }
        //    return res;
        //}

        public static SimulationTimer operator -(SimulationTimer a, SimulationTimer b)
        {
            var res = new SimulationTimer();

            res.hour = a.Hour - b.Hour;
            if(a.min < b.min)
            {
                res.hour--;
                res.min = a.min - b.min + 60;
            }
            else
            {
                res.min = a.min - b.min;
            }

            return res;
        }

        public override string ToString()
        {
            string res = string.Empty;

            if(hour < 10)
            {
                res += $"0{hour}";
            }
            else
            {
                res += $"{hour}";
            }
            res += ":";
            if(min < 10)
            {
                res += $"0{min}";
            }
            else
            {
                res += $"{min}";
            }
            return res;
        }
    }
}
