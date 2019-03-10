using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingMethods
{
    public class DataHandler
    {
        public double StartTime { get; set; }
        public double Frequency { get; set; }
        public byte Type { get; set; }
        public int Samples { get; set; }


        public List<double> PointsX { get; set; }
        public List<double> PointsY { get; set; }

        public double GetMinX()
        {
            return PointsX.Min();
        }
        public double GetMaxX()
        {
            return PointsX.Max();
        }
        public List<(double, double, int)> GetDataForHistogram(int count)
        {
            List<(double, double, int)> result = new List<(double, double, int)>(count);
            double max = PointsY.Max();
            double min = PointsY.Min();

            double range = max - min;
            double interval = range / count;
            for (int i = 0; i < count; i++)
            {
                int points = PointsY.Count(n => n >= min + interval * i && n < min + interval * (i + 1));
                result.Add((Math.Round(min + interval * i, 2), Math.Round(min + interval * (i + 1), 2), points));
            }

            return result;
        }

        public void LoadFromFile(string filePath)
        {
            
        }
        public void SaveToFile(string filePath)
        {
            using (BinaryWriter writer=new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(StartTime);
                writer.Write(Frequency);
                writer.Write(Type);
                writer.Write(Samples); 

                /*foreach (double amplitude in SamplesList)
                {
                    writer.Write(amplitude);
                }*/
            }
        }
    }
}
