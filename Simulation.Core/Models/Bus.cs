using Simulation.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Simulation.Core.ViewModels
{
    public class Bus : ISimulationEntityModel, IHoldHumans
    {
        private const int seatsCount = 30;
        private static int idCnt = 0;
        private IList<Human> _humen;
        private int initDelay;
        private int staySeconds;

        public int Id { get; set; }

        public bool Next => staySeconds < 0;

        public BusViewModel Data { get; set; }

        public bool IsForward { get; set; } = true;

        public IList<Human> Humen => _humen;


        public Bus(int _initDelay)
        {
            Id = ++idCnt;
            initDelay = _initDelay + 3;
            _humen = new List<Human>(seatsCount);
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
            staySeconds = _delaySeconds;
        }

        public int GetDelay() => staySeconds;

        public bool AddHuman(Human human)
        {
            if(_humen.Count < 30)
            {
                _humen.Add(human);
                Data?.UpdateData();
                return true;
            }
            return false;
        }

        public void DropHuman(Human human)
        {
            _humen.Remove(human);
            Data?.UpdateData();
        }

        // Temporary
        public void RemoveRandomHumen(int chanse)
        {
            for (int i = 0; i < _humen.Count; i++)
            {
                if(chanse < 20)
                {
                    DropHuman(_humen[i]);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString() => $"{Id}";
    }
}
