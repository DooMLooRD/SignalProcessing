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
            double interval = (max - min) / 10000;
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
            double interval = (max - min) / 10000;
            double T = 1.0 / samplingFreq;

            do
            {
                signal = ExtendSignal(signal);
            } while (signal.Count <= 2 * n);
            signal.InsertRange(0, signal.Skip(1).Take(n).Reverse().Select(c => (-c.Item1, c.Item2)).ToList());
            var list = signal.Select(k => k.Item1).ToList();
            for (double i = min; i <= max; i += interval)
            {
                double sum = 0;
                (int, int) between = GetIntervalIndex(list, i);
                if (between.Item1 == between.Item2)
                    sum = signal[between.Item1].Item2;
                else
                {
                    for (int j = between.Item1 - n; j <= between.Item2 + n; j++)
                    {
                        var test = i / T - (j - n);
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
            double val1 = signal[1].Item1 - signal[0].Item1;
            var temp = signal.Select(c => (c.Item1 + val + val1, c.Item2)).ToList();
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
