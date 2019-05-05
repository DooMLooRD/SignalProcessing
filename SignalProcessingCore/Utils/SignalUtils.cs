using SignalProcessingCore.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore.Utils
{
    public static class SignalUtils
    {
        public static List<(double, double, int)> GetDataForHistogram(ISignal signal,int count)
        {
            List<(double, double, int)> result = new List<(double, double, int)>(count);

            double max = signal.PointsY.Max();
            double min = signal.PointsY.Min();

            double range = max - min;
            double interval = range / count;
            for (int i = 0; i < count - 1; i++)
            {
                int points = signal.PointsY.Count(n => n >= min + interval * i && n < min + interval * (i + 1));
                result.Add((Math.Round(min + interval * i, 2), Math.Round(min + interval * (i + 1), 2), points));
            }
            int lastPoints = signal.PointsY.Count(n => n >= min + interval * (count - 1) && n <= min + interval * count);
            result.Add((Math.Round(min + interval * (count - 1), 2), Math.Round(min + interval * count, 2), lastPoints));

            return result;
        }
    }
}
