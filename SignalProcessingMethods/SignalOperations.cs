using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace SignalProcessingMethods
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

        public static List<double> SubtractSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
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

        public static List<double> DivideSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result.Add(signal1[i] / signal2[i]);
            }

            return result;

        }

        public static double AvgSignal(double t1, double t2, Func<double, double> func)
        {

            return 1 / (t2 - t1) * Integral(t1, t2, func);
        }

        public static double SignalVariance(double t1, double t2, Func<double, double> func)
        {
            return 1 / (t2 - t1) * Integral(t1, t2, func, d => Math.Pow(d - AvgSignal(t1, t2, func), 2));

        }
        public static double AbsAvgSignal(double t1, double t2, Func<double, double> func)
        {
            return 1 / (t2 - t1) * Integral(t1, t2, func, Math.Abs);
        }

        public static double AvgSignalPower(double t1, double t2, Func<double, double> func)
        {
            return 1 / (t2 - t1) * Integral(t1, t2, func, d => d * d);
        }

        public static double RMSSignal(double t1, double t2, Func<double, double> func)
        {
            return Math.Sqrt(AvgSignalPower(t1, t2, func));
        }

        private static double Integral(double t1, double t2, Func<double, double> func, Func<double, double> additionalFunc = null)
        {
            var dx = (t2 - t1) / 1000;

            double integral = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (additionalFunc != null)
                    integral += additionalFunc(func(t1 + i * dx));
                else
                    integral += func(t1 + i * dx);

            }
            integral *= dx;

            return integral;
        }
    }
}
