using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;

namespace SignalProcessingCore
{
    public class SignalGenerator
    {
        private static Random random = new Random();
        public double Amplitude { get; set; }
        public double StartTime { get; set; }
        public double Duration { get; set; }
        public double Period { get; set; }
        public double FillFactor { get; set; }

        public double GenerateUniformDistributionNoise()
        {
            return random.NextDouble() * 2 * Amplitude - Amplitude;
        }

        public double GenerateSinusoidalSignal(double time)
        {
            return Amplitude * Math.Sin((2 * Math.PI / Period) * (time - StartTime));
        }

        public double GenerateSinusoidal1PSignal(double time)
        {
            return 0.5 * Amplitude * (Math.Sin((2 * Math.PI / Period) * (time - StartTime)) +
                       Math.Abs(Math.Sin((2 * Math.PI / Period) * (time - StartTime))));
        }

        public double GenerateSinusoidal2PSignal(double time)
        {
            return Amplitude * Math.Abs(Math.Sin((2 * Math.PI / Period) * (time - StartTime)));
        }

        public double GenerateRectangularSignal(double time)
        {
            int k = (int)(time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return Amplitude;
            //else if(time >= FillFactor * Period - k * Period + StartTime && time < Period + k * Period + StartTime)

            return 0;
        }

        public double GenerateRectangularSymmetricalSignal(double time)
        {
            int k = (int)(time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return Amplitude;

            return -Amplitude;
        }

        public double GenerateTriangularSignal(double time)
        {
            int k = (int)(time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return (Amplitude / (FillFactor * Period)) * (time - k * Period - StartTime);

            return -Amplitude / (Period * (1 - FillFactor)) * (time - k * Period - StartTime);
        }

        public double GenerateUnitJump(double time, double stime)
        {
            if (time > stime)
                return Amplitude;
            if (time.Equals(stime))
                return 0.5 * Amplitude;
            return -Amplitude;
        }

        public double GenerateGaussianNoise()
        {
            double mean = 2 * Amplitude;
            double stdDev = -Amplitude/3;

            //nuget version - simpler
            //Normal normalDist = new Normal(mean, stdDev);
            //return normalDist.Sample();

            double u1 = 1.0 - random.NextDouble(); //to avoid log(0)=Inf
            double u2 = 1.0 - random.NextDouble();
            double normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            return normal * stdDev + mean;
        }

    }
}
