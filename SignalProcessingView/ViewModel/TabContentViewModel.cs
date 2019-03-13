using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SignalProcessingMethods;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class TabContentViewModel : BaseViewModel
    {
        public SeriesCollection ChartSeries { get; set; }
        public Func<double, string> ChartFormatter { get; set; }
        public string ChartXTitle { get; set; }
        public string ChartYTitle { get; set; }
        public bool IsScattered { get; set; }
        public SeriesCollection HistogramSeries { get; set; }
        public Func<double, string> HistogramFormatter { get; set; }
        public string HistogramXTitle { get; set; }
        public string HistogramYTitle { get; set; }
        public string[] Labels { get; set; }

        public double AvgSignal { get; set; }
        public double AbsAvgSignal { get; set; }
        public double AvgSignalPower { get; set; }
        public double SignalVariance { get; set; }
        public double RMSSignal { get; set; }

        public ICommand Histogram { get; set; }

        public DataHandler Data { get; set; }

        public TabContentViewModel()
        {
            Data = new DataHandler();
            Histogram = new RelayCommand<int>(LoadHistogram);

        }

        public void DrawCharts()
        {
            if (Data.HasData())
            {
                var mapper = Mappers.Xy<PointXY>() 
                    .X(value => value.X) 
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();
                List<double> pointsX;
                List<double> pointsY;
                if (Data.FromSamples)
                {
                    pointsX = Data.SamplesX;
                    pointsY = Data.Samples;
                }
                else
                {
                    pointsX = Data.PointsX;
                    pointsY = Data.PointsY;
                }
                for (int i = 0; i < pointsX.Count; i++)
                {
                    values.Add(new PointXY(pointsX[i], pointsY[i]));
                }

                if (IsScattered || Data.FromSamples)
                {
                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new ScatterSeries()
                        {
                            PointGeometry = new EllipseGeometry(),
                            StrokeThickness = 5,
                            Values = values
                        }
                    };
                }
                else
                {
                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values
                        }
                    };
                }
                

                var histogramResults = Data.GetDataForHistogram(5);
                HistogramSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3))
                    }
                };
                Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();
            }
           
        }

        public void CalculateSignalInfo(double t1,double t2,Func<double,double> func)
        {
            AvgSignal = SignalOperations.AvgSignal(t1, t2, func);
            AbsAvgSignal = SignalOperations.AbsAvgSignal(t1, t2, func);
            AvgSignalPower = SignalOperations.AvgSignalPower(t1, t2, func);
            SignalVariance = SignalOperations.SignalVariance(t1, t2, func);
            RMSSignal = SignalOperations.RMSSignal(t1, t2, func);

        }

        public void LoadHistogram(int c)
        {
            if (Data.HasData())
            {
                var histogramResults = Data.GetDataForHistogram(c);
                HistogramSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3))
                    }
                };
                Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();
            }
           
        }

        public void LoadData(DataHandler data)
        {
            Data = data;
        }

        public void LoadData(List<double> x, List<double> y, bool fromSamples)
        {
            if (fromSamples)
            {
                Data.FromSamples = true;
                Data.Samples = y;
            }
            else
            {
                Data.FromSamples = false;
                Data.PointsX = x;
                Data.PointsY = y;
            }
            
            
        }

        public void SaveDataToFile(string path)
        {
            Data.SaveToFile(path);
        }
        public void LoadDataFromFile(string path)
        {
            Data.FromSamples = true;
            Data.LoadFromFile(path);
        }
    }
}
