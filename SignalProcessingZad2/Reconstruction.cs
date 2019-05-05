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
            if (value.Equals(0))
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
            for (double i = min; i <= max; i += interval)
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

        public static List<(double, double)> SincReconstruction(List<(double, double)> signal, double samplingFreq, int n)
        {
            List<(double, double)> reconstructed = new List<(double, double)>();
            double min = signal.Min(c => c.Item1);
            double max = signal.Max(c => c.Item1);
            double interval = (max - min) / 2001;
            double T = 1.0 / samplingFreq;
            for (double i = min; i <= max; i += interval)
            {
                double sum = 0;
                (int, int) between = GetIntervalIndex(signal.Select(c=>c.Item1).ToList(), i);
                if (between.Item1 == between.Item2)
                    sum = signal[between.Item1].Item2;
                else
                {
                    int start = between.Item1 - n;
                    int end = between.Item2 + n;
                    if (start < 0)
                        start = 0;
                    if (end > signal.Count-1)
                        end = signal.Count - 1;
                    for (int j = start; j <= end; j++)
                    {
                        var test = i / T - j;
                        var test1 = Sinc(test);
                        sum += signal[j].Item2 * test1;
                    }
                }

                reconstructed.Add((i, sum));
            }
            return reconstructed;
        }

        public static List<(double, double)> ExtendSignal(List<(double, double)> signal)
        {
            double val = signal.Last().Item1;
            var temp = signal.Skip(1).Select(c => (c.Item1 + val, c.Item2)).ToList();
            signal.AddRange(temp);
            return signal;
        }
        private static (int, int) GetIntervalIndex(List<double> values, double value)
        {
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (value > values[i] && value < values[i + 1])
                {
                    return (i, i + 1);
                }

                if (values[i].Equals(value))
                    return (i, i);
            }

            return (0, 1);
        }
    }
}
