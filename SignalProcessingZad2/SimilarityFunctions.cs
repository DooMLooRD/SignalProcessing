using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingMethods
{
    public class SimilarityFunctions
    {
        public static double CalculateMSE(List<double> original, List<double> reconstructed)
        {
            return original.Take(reconstructed.Count).Zip(reconstructed, (d, d1) => Math.Pow(d - d1, 2)).Sum()/ reconstructed.Count;
        }

        public static double CalculateSNR(List<double> original, List<double> reconstructed)
        {
            double squareSum = original.Take(reconstructed.Count).Sum(n => Math.Pow(n, 2));
            double differenceSum = original.Take(reconstructed.Count).Zip(reconstructed, (d, d1) => Math.Pow(d - d1, 2)).Sum();
            return 10 * Math.Log10(squareSum/differenceSum);
        }

        public static double CalculatePNSR(List<double> original, List<double> reconstructed)
        {
            return 10 * Math.Log10(original.Take(reconstructed.Count).Max()/CalculateMSE(original, reconstructed));
        }

        public static double CalculateMD(List<double> original, List<double> reconstructed)
        {
            return original.Take(reconstructed.Count).Zip(reconstructed, (d, d1) => Math.Abs(d - d1)).Max(); 
        }

        public static double CalculateSNRAC(int levels)
        {
            return 6.02 * (Math.Log(levels, 2)) + 1.76;
        }

        public static double CalculateENOB(List<double> original, List<double> reconstructed)
        {
            return (CalculateSNR(original, reconstructed) - 1.76) / 6.02;
        }
    }
}
