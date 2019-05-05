using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SignalProcessingCore.Signal;
using SignalProcessingCore.Utils;
using SignalProcessingView.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SignalProcessingView.ViewModel
{
    public class ChartWindowViewModel : BaseViewModel
    {
        private int _sliderValue;

        public SignalCreator SignalCreator { get; set; }
        public ObservableCollection<ISignal> SignalsOnChart { get; set; }
        public ISignal SelectedSignalToDraw { get; set; }
        public ISignal SelectedSignalToRemove { get; set; }
        public ISignal SelectedSignalForHistogram { get; set; }
        public bool DrawLine { get; set; }
        public SeriesCollection ChartSeries { get; set; }
        public SeriesCollection HistogramSeries { get; set; }
        public int HistogramStep { get; set; }
        public string[] Labels { get; set; }
        public ICommand Histogram { get; set; }
        public ICommand DrawCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                LoadHistogram(_sliderValue);
            }
        }

        public ChartWindowViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;
            SignalsOnChart = new ObservableCollection<ISignal>();
            ChartSeries = new SeriesCollection();
            DrawCommand = new RelayCommand(DrawSignal);
            RemoveCommand = new RelayCommand(RemoveSignal);
            Histogram = new RelayCommand<int>(LoadHistogram);
            SliderValue = 20;


        }
        public void RemoveSignal()
        {
            if (SelectedSignalToRemove != null)
            {
                var series = ChartSeries.First(c => c.Title == SelectedSignalToRemove.Name);
                ChartSeries.Remove(series);
                SignalsOnChart.Remove(SelectedSignalToRemove);
            }

        }
        public void LoadHistogram()
        {
            LoadHistogram(SliderValue);

        }
        public void LoadHistogram(int c)
        {
            if (SelectedSignalForHistogram != null && SelectedSignalForHistogram.HasData())
            {
                var histogramResults = SignalUtils.GetDataForHistogram(SelectedSignalForHistogram, c);
                HistogramStep = (int)Math.Ceiling(c / 20.0);
                HistogramSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3)),
                        ColumnPadding = 0,
                        CacheMode = new BitmapCache()
                    }
                };
                Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();

            }
        }

        public void DrawSignal()
        {
            if (SelectedSignalToDraw != null && SelectedSignalToDraw.HasData())
            {
                ChartValues<ObservablePoint> values = new ChartValues<ObservablePoint>();
                List<double> pointsX = SelectedSignalToDraw.PointsX;
                List<double> pointsY = SelectedSignalToDraw.PointsY;
                if(pointsX!=null)
                {
                    for (int i = 0; i < pointsX.Count; i++)
                    {
                        values.Add(new ObservablePoint(pointsX[i], pointsY[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < pointsY.Count; i++)
                    {
                        values.Add(new ObservablePoint(i, pointsY[i]));
                    }
                }
                SignalsOnChart.Add(SelectedSignalToDraw);
                if (SelectedSignalToDraw.GetType() == typeof(SampledSignal))
                {
                    if(DrawLine)
                    {
                        ChartSeries.Add(new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 1,
                            Fill = Brushes.Transparent,
                            PointGeometry = new EllipseGeometry(),
                            PointGeometrySize = 5,
                            Values = values,
                            Title = SelectedSignalToDraw.Name
                        });
                    }
                    else
                    {
                        ChartSeries.Add(new ScatterSeries()
                        {
                            PointGeometry = new EllipseGeometry(),
                            StrokeThickness = 5,
                            Values = values,
                            Title = SelectedSignalToDraw.Name
                        });
                    }
                  
                }
                else
                {
                    ChartSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent,
                        PointGeometry = null,
                        Values = values,
                        Title = SelectedSignalToDraw.Name
                    });
                }
            }


        }
    }
}
