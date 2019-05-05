using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad3
{
    public class Convolution
    {
        public static List<double> ComputeSignal(List<double> signal1, List<double> signal2)
        {
            var result = new List<double>();
            for (int i = 0; i < signal1.Count + signal2.Count - 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < signal1.Count; j++)
                {
                    if (i - j < 0 || i - j >= signal2.Count)
                        continue;
                    sum += signal1[j] * signal2[i - j];
                }
                result.Add(sum);
            }
            return result;
        }

        

    }
}
