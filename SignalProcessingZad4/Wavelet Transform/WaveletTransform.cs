using SignalProcessingZad1;
using SignalProcessingZad3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad4.Wavelet_Transform
{
    public class WaveletTransform
    {
        private static List<double> H = new List<double>
        {
            (1 + Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (3 + Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (3 - Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (1 - Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
        };
        private static List<double> HO = new List<double>
        {
            (1 - Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (3 - Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (3 + Math.Sqrt(3)) / (4 * Math.Sqrt(2)),
            (1 + Math.Sqrt(3)) / (4 * Math.Sqrt(2))
        };
        private static List<double> G = new List<double>
        {
            H[3],
            -H[2],
            H[1],
            -H[0],
        };
        private static List<double> GO = new List<double>
        {
            -H[0],
            H[1],
            -H[2],
            H[3]
        };
        public static List<Complex> Transform(List<double> points)
        {
            List<Complex> transformed = new List<Complex>();
            List<double> xh = Convolution.ComputeSignal(points, H).Take(points.Count).ToList();
            List<double> xg = Convolution.ComputeSignal(points, G).Take(points.Count).ToList();
            List<double> xhHalf = new List<double>();
            List<double> xgHalf = new List<double>();

            for (int i = 0; i < xh.Count; i++)
            {
                if (i % 2 == 0)
                {
                    xhHalf.Add(xh[i]);
                }
                else
                {
                    xgHalf.Add(xg[i]);
                }
            }
            for (int i = 0; i < xhHalf.Count; i++)
            {
                transformed.Add(new Complex(xhHalf[i], xgHalf[i]));
            }
            return transformed;
        }
        public static List<double> ReverseTransform(List<Complex> points)
        {
            List<double> xh = new List<double>();
            List<double> xg = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                xh.Add(points[i].Real);
                xh.Add(0);
                xg.Add(0);

                xg.Add(points[i].Imaginary);
            }
            List<double> xhC = Convolution.ComputeSignal(xh, HO).Take(xh.Count).ToList();
            List<double> xgC = Convolution.ComputeSignal(xg, GO).Take(xg.Count).ToList();
            return SignalOperations.AddSignals(xhC, xgC); 
        }
    }
}
