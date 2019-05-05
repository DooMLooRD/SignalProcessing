using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore.Signal
{
    public class ContinuousSignal : ISignal
    {
        public string Name { get; set; }

        public List<double> PointsX { get; set; }
        public List<double> PointsY { get; set; }
        public bool HasData()
        {
            if (PointsY == null || PointsY.Count == 0)
                return false;

            if (PointsX == null || PointsX.Count == 0)
                return false;
            return true;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
