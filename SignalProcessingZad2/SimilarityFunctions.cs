using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingMethods
{
    public class SimilarityFunctions
    {
        public double CalculateMSE(List<double> original, List<double> reconstructed)
        {
            return original.Zip(reconstructed, (d, d1) => Math.Pow(d - d1, 2)).Sum()/ reconstructed.Count;
        }

        public double CalculateSNR(List<double> original, List<double> reconstructed)
        {
            double squareSum = original.Sum(n => Math.Pow(n, 2));
            double differenceSum = original.Zip(reconstructed, (d, d1) => Math.Pow(d - d1, 2)).Sum();
            return 10 * Math.Log10(squareSum/differenceSum);
        }

        public double CalculatePNSR(List<double> original, List<double> reconstructed)
        {
            return 10 * Math.Log10(original.Max()/CalculateMSE(original, reconstructed));
        }

        public double CalculateMD(List<double> original, List<double> reconstructed)
        {
            return original.Zip(reconstructed, (d, d1) => Math.Abs(d - d1)).Max(); 
        }

        public double CalculateSNRAC(int levels)
        {
            return 6.02 * (Math.Log(levels, 2)) + 1.76;
        }

        public double CalculateENOB(List<double> original, List<double> reconstructed)
        {
            return (CalculateSNR(original, reconstructed) - 1.76) / 6.02;
        }
    }
}
