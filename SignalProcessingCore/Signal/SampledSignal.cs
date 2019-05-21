using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public List<Complex> ComplexPoints { get; set; }

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
        public void RoundComplex()
        {
            double eps = 10E-14;
            for (int i = 0; i < ComplexPoints.Count; i++)
            {
                var real = ComplexPoints[i].Real;
                var imaginary = ComplexPoints[i].Imaginary;
                if (real - Math.Floor(real) < eps)
                {
                    real = Math.Floor(real);
                }
                else if (Math.Ceiling(real) - real < eps)
                {
                    real = Math.Ceiling(real);
                }

                if (imaginary - Math.Floor(imaginary) < eps)
                {
                    imaginary = Math.Floor(imaginary);
                }
                else if (Math.Ceiling(imaginary) - imaginary < eps)
                {
                    imaginary = Math.Ceiling(imaginary);
                }
                ComplexPoints[i] = new Complex(real, imaginary);
            }
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
