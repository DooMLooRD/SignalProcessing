using SignalProcessingZad3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = Convolution.ComputeSignal(new List<double> { 0, 0, 1, 2, 3, 2, 1 }, new List<double> { 1, 2, 3, 2, 1 });
        }
    }
}
