using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore.Signal
{
    public class SampledSignal : ISignal
    {
        public string Name { get; set; }
        public double StartTime { get; set; }
        public double Frequency { get; set; }
        public byte Type { get; set; }

        public List<double> PointsX { get; set; }
        public List<double> PointsY { get; set; }

        public void CalculateSamplesX()
        {
            List<double> points = new List<double>();
            for (int i = 0; i < PointsY.Count; i++)
            {
                points.Add(StartTime + i / Frequency);
            }

            PointsX = points;
        }

        public bool HasData()
        {

            if (PointsY == null || PointsY.Count == 0)
                return false;

            return true;

        }

        public bool IsValid(SampledSignal signal)
        {
            if (signal.PointsY.Count != PointsY.Count)
                return false;
            return true;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
