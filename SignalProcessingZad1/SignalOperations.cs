using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace SignalProcessingZad1
{
    public class SignalOperations
    {
        public static List<double> AddSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] + signal2[i]);
            }

            return result;

        }
        public static List<Complex> AddComplexSignals(List<Complex> signal1, List<Complex> signal2)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] + signal2[i]);
            }

            return result;

        }

        public static List<double> SubtractSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] - signal2[i]);
            }

            return result;

        }
        public static List<Complex> SubtractComplexSignals(List<Complex> signal1, List<Complex> signal2)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] - signal2[i]);
            }

            return result;

        }
        public static List<double> MultiplySignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] * signal2[i]);
            }

            return result;

        }
        public static List<Complex> MultiplyComplexSignals(List<Complex> signal1, List<Complex> signal2)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] * signal2[i]);
            }

            return result;

        }
        
        public static List<double> DivideSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] / signal2[i]);
            }

            return result;

        }
        public static List<Complex> DivideComplexSignals(List<Complex> signal1, List<Complex> signal2)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] / signal2[i]);
            }

            return result;

        }
        public static double AvgSignal(List<double> samples, double t1 = 0, double t2 = 0, bool isDiscrete = true)
        {
            if (isDiscrete)
            {
                return 1.0 / samples.Count * Sum(samples);
            }
            return 1 / (t2 - t1) * Integral(Math.Abs((t2 - t1) / samples.Count), samples);
        }

        public static double SignalVariance(List<double> samples, double t1 = 0, double t2 = 0, bool isDiscrete = true)
        {
            if (isDiscrete)
            {
                return 1.0 / samples.Count * Sum(samples, d => Math.Pow(d - AvgSignal(samples, isDiscrete: true), 2));
            }
            return 1 / (t2 - t1) * Integral(Math.Abs((t2 - t1) / samples.Count), samples, d => Math.Pow(d - AvgSignal(samples, t1, t2), 2));

        }
        public static double AbsAvgSignal(List<double> samples, double t1 = 0, double t2 = 0, bool isDiscrete = true)
        {
            if (isDiscrete)
            {
                return 1.0 / samples.Count * Sum(samples, Math.Abs);
            }
            return 1 / (t2 - t1) * Integral(Math.Abs((t2 - t1) / samples.Count), samples, Math.Abs);
        }

        public static double AvgSignalPower(List<double> samples, double t1 = 0, double t2 = 0, bool isDiscrete = true)
        {
            if (isDiscrete)
            {
                return 1.0 / samples.Count * Sum(samples, d => d * d);
            }
            return 1 / (t2 - t1) * Integral(Math.Abs((t2 - t1) / samples.Count), samples, d => d * d);
        }

        public static double RMSSignal(List<double> samples, double t1 = 0, double t2 = 0, bool isDiscrete = true)
        {
            if (isDiscrete)
            {
                return Math.Sqrt(AvgSignalPower(samples, isDiscrete: true));
            }
            return Math.Sqrt(AvgSignalPower(samples, t1, t2));
        }

        private static double Integral(double dx, List<double> samples, Func<double, double> additionalFunc = null)
        {
            double integral = 0;
            foreach (var sample in samples)
            {
                if (additionalFunc != null)
                    integral += additionalFunc(sample);
                else
                    integral += sample;
            }
            integral *= dx;

            return integral;
        }

        private static double Sum(List<double> samples, Func<double, double> additionalFunc = null)
        {
            double sum = 0;
            foreach (var sample in samples)
            {
                if (additionalFunc != null)
                    sum += additionalFunc(sample);
                else
                    sum += sample;
            }
            return sum;
        }
    }
}
