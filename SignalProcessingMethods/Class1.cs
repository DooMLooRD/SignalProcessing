using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore
{
    public class Generator
    {
        public double Amplitude { get; set; }
        public double StartTime { get; set; }
        public double Duration { get; set; }
        public double Period { get; set; }
        public double FillFactor { get; set; }

        public double GenerateUniformDistributionNoise()
        {
            Random r = new Random();
            return r.NextDouble() * 2 * Amplitude - Amplitude;
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
            int k = (int) (time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return Amplitude;
            //else if(time >= FillFactor * Period - k * Period + StartTime && time < Period + k * Period + StartTime)
            else
                return 0;
        }

        public double GenerateRectangularSymmetricalSignal(double time)
        {
            int k = (int)(time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return Amplitude;
            else
                return -Amplitude;
        }

        public double GenerateTriangularSignal(double time)
        {
            int k = (int)(time / Period);
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return (Amplitude/(FillFactor * Period)) * (time - k * Period - StartTime);
            else
                return -Amplitude/(Period*(1-FillFactor)) * (time - k * Period - StartTime);
        }

        public double GenerateUnitJump(double time, double stime)
        {
            if (time > stime)
                return Amplitude;
            else if (time == stime)
                return 0.5 * Amplitude;
            else
                return -Amplitude;
        }

    }
}
