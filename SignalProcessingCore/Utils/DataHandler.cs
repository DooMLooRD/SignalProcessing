using SignalProcessingCore.Signal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessingCore.Utils
{
    public class DataHandler
    {
        public SampledSignal LoadFromFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                SampledSignal signal = new SampledSignal();
                signal.PointsY = new List<double>();
                signal.StartTime = reader.ReadDouble();
                signal.Frequency = reader.ReadDouble();
                signal.Type = reader.ReadByte();

                int length = reader.ReadInt32();
                for (int i = 0; i < length; i++)
                {

                    signal.PointsY.Add(reader.ReadDouble());
                }

                signal.CalculateSamplesX();
                return signal;
            }

        }
        public void SaveToFile(string filePath, SampledSignal signal)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(signal.StartTime);
                writer.Write(signal.Frequency);
                writer.Write(signal.Type);
                writer.Write(signal.PointsY.Count);
                foreach (double sample in signal.PointsY)
                {
                    writer.Write(sample);
                }

            }
            string newPath = Path.ChangeExtension(filePath, ".txt");

            using (StreamWriter writer = new StreamWriter(newPath))
            {
                writer.WriteLine("Start Time: " + signal.StartTime);
                writer.WriteLine("Frequency: " + signal.Frequency);
                writer.WriteLine("Type: " + signal.Type);
                writer.WriteLine("Number of samples: " + signal.PointsY.Count);
                writer.WriteLine("Rational:");
                for (int i = 0; i < signal.PointsY.Count; i++)
                {
                    writer.WriteLine(i + 1 + ". " + signal.PointsY[i]);
                }

            }
        }
    }
}
