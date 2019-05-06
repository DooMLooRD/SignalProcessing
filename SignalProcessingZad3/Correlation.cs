using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad3
{
    public class Correlation
    {
        public static List<double> ComputeSignal(List<double> signal1, List<double> signal2)
        {
            List<double> signal = signal1.ToList();
            signal.Reverse();
            return Convolution.ComputeSignal(signal, signal2);
        }
    }
}
