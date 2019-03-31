using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingZad2
{
    public class Quantization
    {
        public List<double> Quantize(List<double> values, int levels)
        {
            return values.Select(n => Math.Round(n * levels) / levels).ToList();
        }
    }
}
