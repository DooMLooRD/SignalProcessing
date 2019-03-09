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
        public Func<double,string> ChartFormatter { get; set; }
        public string ChartXTitle { get; set; }
        public string ChartYTitle { get; set; }

        public SeriesCollection HistogramSeries { get; set; }
        public Func<double, string> HistogramFormatter { get; set; }
        public string HistogramXTitle { get; set; }
        public string HistogramYTitle { get; set; }


        public DataHandler Data { get; set; }  
        public bool HasData { get; set; }

        public TabContentViewModel()
        {
            Data=new DataHandler();
        }

        public void DrawCharts()
        {
            ChartValues<ObservablePoint> values=new ChartValues<ObservablePoint>();
            for (int i = 0; i < Data.PointsX.Count; i++)
            {
                values.Add(new ObservablePoint(Data.PointsX[i],Data.PointsY[i]));
            }
            ChartSeries=new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = values
                }
            };
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
