using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad4.Fourier
{
    public class FastFourierTransform
    {
        private Dictionary<string, Complex> _factors = new Dictionary<string, Complex>();
        private Dictionary<string, Complex> _factorsReverse = new Dictionary<string, Complex>();
        public List<Complex> Transform(List<double> points)
        {
            List<Complex> transformed = new List<Complex>();
            int N = points.Count;
            transformed = SwitchSamples(points.Select(s => new Complex(s, 0)).ToList());
            return transformed.Select(c => c).ToList();
        }
        public List<double> ReverseTransform(List<Complex> points)
        {
            List<double> transformed = new List<double>();
            int N = points.Count;
            transformed = SwitchSamples(points, true).Select(c => c.Real / N).ToList();
            return transformed;
        }
        public List<Complex> SwitchSamples(List<Complex> points, bool reverse = false)
        {
            if (points.Count < 2)
            {
                return points;
            }
            List<Complex> oddPoints = new List<Complex>();
            List<Complex> evenPoints = new List<Complex>();
            for (int i = 0; i < points.Count / 2; i++)
            {
                evenPoints.Add(points[i * 2]);
                oddPoints.Add(points[i * 2 + 1]);
            }
            var result = Connect(SwitchSamples(evenPoints, reverse), SwitchSamples(oddPoints, reverse), reverse);
            return result;
        }
        private List<Complex> Connect(List<Complex> evenPoints, List<Complex> oddPoints, bool reverse)
        {
            int N = oddPoints.Count * 2;
            Complex[] result = new Complex[N];
            for (int i = 0; i < oddPoints.Count; i++)
            {
                if (reverse)
                {
                    if (!_factorsReverse.ContainsKey($"{i}, {N}"))
                        _factorsReverse[$"{i}, {N}"] = CalculateReverseFactor(i, 1, N);
                    result[i] = evenPoints[i] + (_factorsReverse[$"{i}, {N}"] * oddPoints[i]);
                    result[i + oddPoints.Count] = evenPoints[i] - (_factorsReverse[$"{i}, {N}"] * oddPoints[i]);
                }
                else
                {
                    if (!_factors.ContainsKey($"{i}, {N}"))
                        _factors[$"{i}, {N}"] = CalculateFactor(i, 1, N);
                    result[i] = evenPoints[i] + (_factors[$"{i}, {N}"] * oddPoints[i]);
                    result[i + oddPoints.Count] = evenPoints[i] - (_factors[$"{i}, {N}"] * oddPoints[i]);
                }

            }
            return result.ToList();
        }
        private Complex CalculateFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, -2 * Math.PI * m * n / N));
        }
        private Complex CalculateReverseFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, 2 * Math.PI * m * n / N));
        }
    }
}
