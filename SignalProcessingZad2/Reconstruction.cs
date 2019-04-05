using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad2
{
    public class Reconstruction
    {
        public static double Rect(double value)
        {
            if (Math.Abs(value) > 0.5)
                return 0;
            if (Math.Abs(value).Equals(0.5))
                return 0.5;

            return 1;
        }

        public static double Sinc(double value)
        {
            if (Math.Abs(value) < 0.001)
                return 1;
            return Math.Sin(Math.PI * value) / (Math.PI * value);
        }

        public static List<(double, double)> ZeroOrderHold(List<(double, double)> signal, double samplingFreq, int n = 8)
        {
            List<(double, double)> reconstructed = new List<(double, double)>();
            double min = signal.Min(c => c.Item1);
            double max = signal.Max(c => c.Item1);
            double interval = (max - min) / 5000;
            double T = 1.0 / samplingFreq;
            //var list = signal.Select(k => k.Item1).ToList();
            for (double i = min; i < max; i += interval)
            {
                double sum = 0;
                //(int, int) between = GetIntervalIndex(list, i);
                // if (between.Item2 + n + 1 > signal.Count || between.Item1 - n < 0)
                //    continue;
                for (int j = 0; j < signal.Count; j++)
                {
                    sum += signal[j].Item2 * Rect((i - (T / 2) - j * T) / T);
                }
                reconstructed.Add((i, sum));
            }
            return reconstructed;
        }

        public static List<(double, double)> SincReconstruction(List<(double, double)> signal, double samplingFreq, int n = 100)
        {
            List<(double, double)> reconstructed = new List<(double, double)>();
            double min = signal.Min(c => c.Item1);
            double max = signal.Max(c => c.Item1);
            double interval = (max - min) / 10000;
            List<double> sig = new List<double>();
            for (int i = 0; i < 10000; i++)
            {
                sig.Add(Math.Round(min + i * interval, 5));
            }

            double val = signal.Last().Item1;
            double T = 1.0 / samplingFreq;
           // var temp = signal.Skip(1).Select(c => (c.Item1 + val, c.Item2)).ToList();
            //signal.AddRange(temp);
            //signal.InsertRange(0, signal.Skip(1).Take(2 * n).Reverse().Select(c => (-c.Item1, c.Item2)).ToList());
           // var list = signal.Select(k => k.Item1).ToList();
            foreach (var t in sig)
            {
                double sum = 0;
               // (int, int) between = GetIntervalIndex(list, t);
               // if (between.Item2 + n + 1 > signal.Count || between.Item1 - n < 0)
                //    continue;
                for (int j = 0; j < signal.Count; j++)
                {
                    var test = t / T - j;
                    var test1 = Sinc(test);
                    sum += signal[j].Item2 * test1;
                }
                reconstructed.Add((t, sum));
            }
            return reconstructed;
        }

        private static (int, int) GetIntervalIndex(List<double> values, double value)
        {
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (value >= values[i] && value < values[i + 1])
                {
                    return (i, i + 1);
                }
            }

            return (0, 1);
        }
    }
}
