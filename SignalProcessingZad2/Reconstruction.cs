using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad2
{
    public class Reconstruction
    {
        public double Rect(double value)
        {
            if (Math.Abs(value) > 0.5)
                return 0;
            if (Math.Abs(value).Equals(0.5))
                return 0.5;

            return 1;
        }

        public double Sinc(double value)
        {
            if (value.Equals(0))
                return 1;
            return Math.Sin(Math.PI * value) / (Math.PI * value);
        }

        public List<(double, double)> ZeroOrderHold(List<(double, double)> signal, double samplingFreq)
        {
            List<(double, double)> reconstructed = new List<(double, double)>();
            double T = 1.0 / samplingFreq;
            for (int i = 0; i < signal.Count; i++)
            {
                reconstructed.Add((signal[i].Item1, signal.Sum(n => n.Item2 * Rect((signal[i].Item1 - T / 2 - i * T) / T))));
            }
            return reconstructed;
        }

        public List<(double, double)> SincReconstruction(List<(double, double)> signal, double samplingFreq)
        {
            List<(double, double)> reconstructed = new List<(double, double)>();
            double T = 1.0 / samplingFreq;
            for (int i = 0; i < signal.Count; i++)
            {
                reconstructed.Add((signal[i].Item1, signal.Sum(n=>n.Item2*Sinc(signal[i].Item1/T-i))));
            }
            return reconstructed;
        }
    }
}
