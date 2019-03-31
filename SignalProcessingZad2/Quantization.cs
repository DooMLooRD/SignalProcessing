using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad2
{
    public class Quantization
    {
        public static List<double> Quantize(List<double> values, int levels)
        {
            double max = values.Max();
            double min = values.Min();
            return values.Select(n => Math.Floor((n - min) / (max - min) * levels) / levels * (max - min) + min).ToList();
        }
    }
}
