using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
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

        public SeriesCollection HistogramSeries { get; set; }
        public Func<double, string> HistogramFormatter { get; set; }
        public string HistogramXTitle { get; set; }
        public string HistogramYTitle { get; set; }
        public string[] Labels { get; set; }

        public ICommand Histogram { get; set; }

        public DataHandler Data { get; set; }
        public bool HasData { get; set; }

        public TabContentViewModel()
        {
            Data = new DataHandler();
            Histogram = new RelayCommand<int>(LoadHistogram);

        }

        public void DrawCharts()
        {
            if (Data.HasData())
            {
                ChartValues<ObservablePoint> values = new ChartValues<ObservablePoint>();
                for (int i = 0; i < Data.PointsX.Count; i++)
                {
                    values.Add(new ObservablePoint(Data.PointsX[i], Data.PointsY[i]));
                }
                ChartSeries = new SeriesCollection()
                {
                    new LineSeries()
                    {

                        StrokeThickness = 0.5,
                        Fill = Brushes.Transparent,
                        PointGeometry = null,
                        Values = values
                    }
                };

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
        public void LoadData(List<double> x, List<double> y)
        {
            Data.PointsX = x;
            Data.PointsY = y;
            HasData = true;
        }

        public void LoadDataFromFile(string path)
        {
            Data.LoadFromFile(path);
            HasData = true;
        }
    }
}
