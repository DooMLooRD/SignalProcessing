using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore.Signal
{
    public interface ISignal
    {
        string Name { get; set; }
        List<double> PointsX { get; set; }
        List<double> PointsY { get; set; }
        bool HasData();
    }
}
