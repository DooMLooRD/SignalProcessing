using SignalProcessingCore.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad4.Fourier
{
    public class FourierTransform
    {
        public static List<Complex> Transform(List<double> points)
        {
            List<Complex> transformed = new List<Complex>();
            int N = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                Complex transformedValue = Complex.Zero;
                for (int j = 0; j < points.Count; j++)
                {
                    transformedValue += points[j] * Complex.Exp(new Complex(0, -2 * Math.PI * i * j / N));
                }
                transformed.Add(transformedValue / N);
            }
            return transformed;
        }
        public static List<double> ReverseTransform(List<Complex> points)
        {
            List<double> transformed = new List<double>();

            int N = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                double transformedValue = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    transformedValue += (points[j] * Complex.Exp(new Complex(0, 2 * Math.PI * i * j / N))).Real;
                }
                transformed.Add(transformedValue);
            }
            return transformed;
        }
    }
}
