using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class Bus : ISimulationEntityModel, IHoldHumans
    {
        static private int idCnt = 0;
        private const int seatsCount = 30;
        private IList<Human> Humen;
        private int initDelay;
        private int staySeconds;
        private int delaySeconds;

        public int Id { get; set; }

        public bool Next => staySeconds == 0;

        public bool IsForward { get; set; } = true;

        public Bus(int _initDelay)
        {
            Id = ++idCnt;
            initDelay = _initDelay;
            Humen = new List<Human>(seatsCount);
        }

        public bool Init()
        {
            initDelay = initDelay > 0 ? initDelay - 1 : 0;
            return initDelay == 0;
        }

        public void Tick()
        {
            if (staySeconds > -1)
                staySeconds--;
        }

        public void Reset()
        {
            staySeconds = 0;
        }

        public void SetDelay(int _delaySeconds)
        {
            staySeconds = _delaySeconds + 2;
        }

        public void AddHuman(Human human)
        {
            Humen.Add(human);
        }

        public void RemoveHuman(Human human)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => $"{Id}";
    }
}
