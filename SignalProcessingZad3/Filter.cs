using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad3
{
    public class Filter
    {
        public static List<double> CreateFilterSignal(int M, int K, Func<int, int, List<double>> filterFunction, Func<List<double>, int, List<double>> windowFunction)
        {
            return windowFunction(filterFunction(M, K), M);
        }
        public static List<double> LowPassFilter(int M, int K)
        {
            List<double> factors = new List<double>();
            int center = (M - 1) / 2;

            for (int i = 0; i < M; i++)
            {
                double factor;
                if (i == center)
                {
                    factor = 2.0 / K;
                }
                else
                {
                    factor = Math.Sin(2 * Math.PI * (i - center) / K) / (Math.PI * (i - center));
                }
                factors.Add(factor);
            }
            return factors;
        }

        public static List<double> MidPassFilter(int M, int K)
        {
            List<double> lowPassFactors = LowPassFilter(M, K);
            List<double> factors = new List<double>();

            for (int i = 0; i < lowPassFactors.Count; i++)
            {
                factors.Add(lowPassFactors[i] * 2 * Math.Sin(Math.PI * i / 2.0));
            }
            return factors;
        }

        public static List<double> HighPassFilter(int M, int K)
        {
            List<double> lowPassFactors = LowPassFilter(M, K);
            List<double> factors = new List<double>();

            for (int i = 0; i < lowPassFactors.Count; i++)
            {
                factors.Add(lowPassFactors[i] * Math.Pow(-1.0, i));
            }
            return factors;
        }

        public static List<double> RectangularWindow(List<double> filterFactors, int M)
        {
            return filterFactors;
        }
        public static List<double> HammingWindow(List<double> filterFactors, int M)
        {
            List<double> factors = new List<double>();
            for (int i = 0; i < filterFactors.Count; i++)
            {
                double windowFactor = 0.53836 - (0.46164 * Math.Cos(2 * Math.PI * i / M));
                factors.Add(windowFactor * filterFactors[i]);
            }

            return factors;
        }
        public static List<double> HanningWindow(List<double> filterFactors, int M)
        {
            List<double> factors = new List<double>();
            for (int i = 0; i < filterFactors.Count; i++)
            {
                double windowFactor = 0.5 - (0.5 * Math.Cos(2 * Math.PI * i / M));
                factors.Add(windowFactor * filterFactors[i]);
            }

            return factors;
        }
        public static List<double> BlackmanWindow(List<double> filterFactors, int M)
        {
            List<double> factors = new List<double>();
            for (int i = 0; i < filterFactors.Count; i++)
            {
                double windowFactor = 0.42 - (0.5 * Math.Cos(2 * Math.PI * i / M)) + (0.08 * Math.Cos(4 * Math.PI * i / M));
                factors.Add(windowFactor * filterFactors[i]);
            }

            return factors;
        }
    }
}
