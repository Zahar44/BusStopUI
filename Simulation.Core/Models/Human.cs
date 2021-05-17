using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation.Core.Models
{
    public class Human : ISimulationEntityModel
    {
        static private int idCnt = 0;
        public int Id { get; set; }

        public Human()
        {
            Id = ++idCnt;
        }

        public override string ToString() => $"{Id}";
    }
}
