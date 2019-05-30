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
using System.Numerics;
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
        public ObservableCollection<SampledSignal> SignalsW1OnChart { get; set; }
        public ObservableCollection<SampledSignal> SignalsW2OnChart { get; set; }
        public ISignal SelectedSignalToDraw { get; set; }
        public ISignal SelectedSignalToRemove { get; set; }
        public SampledSignal SelectedSignalW1ToRemove { get; set; }
        public SampledSignal SelectedSignalW2ToRemove { get; set; }
        public SampledSignal SelectedSignalW1ToDraw { get; set; }
        public SampledSignal SelectedSignalW2ToDraw { get; set; }

        public ISignal SelectedSignalForHistogram { get; set; }
        public bool DrawLine { get; set; }
        public bool DrawW1Line { get; set; }
        public bool DrawW2Line { get; set; }
        public SeriesCollection ChartSeries { get; set; }
        public SeriesCollection RealSeries { get; set; }
        public SeriesCollection ImaginarySeries { get; set; }
        public SeriesCollection MagnitudeSeries { get; set; }
        public SeriesCollection PhaseSeries { get; set; }
        public SeriesCollection HistogramSeries { get; set; }
        public int HistogramStep { get; set; }
        public string[] Labels { get; set; }
        public ICommand Histogram { get; set; }
        public ICommand DrawCommand { get; set; }
        public ICommand DrawW1Command { get; set; }
        public ICommand DrawW2Command { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand RemoveW1Command { get; set; }
        public ICommand RemoveW2Command { get; set; }

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
            SignalsW1OnChart = new ObservableCollection<SampledSignal>();
            SignalsW2OnChart = new ObservableCollection<SampledSignal>();

            ChartSeries = new SeriesCollection();
            RealSeries = new SeriesCollection();
            ImaginarySeries = new SeriesCollection();
            MagnitudeSeries = new SeriesCollection();
            PhaseSeries = new SeriesCollection();

            DrawCommand = new RelayCommand(DrawSignal);
            DrawW1Command = new RelayCommand(DrawW1Signal);
            DrawW2Command = new RelayCommand(DrawW2Signal);

            RemoveCommand = new RelayCommand(RemoveSignal);
            RemoveW1Command = new RelayCommand(RemoveW1Signal);
            RemoveW2Command = new RelayCommand(RemoveW2Signal);
            Histogram = new RelayCommand<int>(LoadHistogram);
            SliderValue = 20;


        }

        private void RemoveW2Signal()
        {
            if (SelectedSignalW2ToRemove != null)
            {
                var seriesMagnitude = MagnitudeSeries.First(c => c.Title == SelectedSignalW2ToRemove.Name + " Magnitude");
                MagnitudeSeries.Remove(seriesMagnitude);

                var seriesPhase = PhaseSeries.First(c => c.Title == SelectedSignalW2ToRemove.Name + " Phase");
                PhaseSeries.Remove(seriesPhase);

                SignalsW2OnChart.Remove(SelectedSignalW2ToRemove);
            }
        }

        private void RemoveW1Signal()
        {
            if (SelectedSignalW1ToRemove != null)
            {
                var seriesReal = RealSeries.First(c => c.Title == SelectedSignalW1ToRemove.Name + " Real");
                RealSeries.Remove(seriesReal);

                var seriesImaginary = ImaginarySeries.First(c => c.Title == SelectedSignalW1ToRemove.Name + " Imaginary");
                ImaginarySeries.Remove(seriesImaginary);

                SignalsW1OnChart.Remove(SelectedSignalW1ToRemove);
            }
        }

        private void DrawW2Signal()
        {
            if (SelectedSignalW2ToDraw != null)
            {
                ChartValues<double> valuesMagnitude = new ChartValues<double>();
                ChartValues<double> valuesPhase = new ChartValues<double>();
                List<Complex> points = SelectedSignalW2ToDraw.ComplexPoints;

                for (int i = 0; i < points.Count; i++)
                {
                    valuesMagnitude.Add(points[i].Magnitude);
                    valuesPhase.Add(points[i].Phase);
                }

                SignalsW2OnChart.Add(SelectedSignalW2ToDraw);

                if (DrawW2Line)
                {
                    MagnitudeSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent,
                        PointGeometry = new EllipseGeometry(),
                        PointGeometrySize = 5,
                        Values = valuesMagnitude,
                        Title = SelectedSignalW2ToDraw.Name + " Magnitude"
                    });
                    PhaseSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent,
                        PointGeometry = new EllipseGeometry(),
                        PointGeometrySize = 5,
                        Values = valuesPhase,
                        Title = SelectedSignalW2ToDraw.Name + " Phase"
                    });
                }
                else
                {
                    MagnitudeSeries.Add(new ScatterSeries()
                    {
                        PointGeometry = new EllipseGeometry(),
                        StrokeThickness = 5,
                        Values = valuesMagnitude,
                        Title = SelectedSignalW2ToDraw.Name + " Magnitude"
                    });
                    PhaseSeries.Add(new ScatterSeries()
                    {
                        PointGeometry = new EllipseGeometry(),
                        StrokeThickness = 5,
                        Values = valuesPhase,
                        Title = SelectedSignalW2ToDraw.Name + " Phase"
                    });
                }

            }
        }

        private void DrawW1Signal()
        {
            if (SelectedSignalW1ToDraw != null)
            {
                ChartValues<double> valuesReal = new ChartValues<double>();
                ChartValues<double> valuesImaginary = new ChartValues<double>();
                List<Complex> points = SelectedSignalW1ToDraw.ComplexPoints;

                for (int i = 0; i < points.Count; i++)
                {
                    valuesReal.Add(points[i].Real);
                    valuesImaginary.Add(points[i].Imaginary);
                }

                SignalsW1OnChart.Add(SelectedSignalW1ToDraw);

                if (DrawW1Line)
                {
                    RealSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent,
                        PointGeometry = new EllipseGeometry(),
                        PointGeometrySize = 5,
                        Values = valuesReal,
                        Title = SelectedSignalW1ToDraw.Name + " Real"
                    });
                    ImaginarySeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent,
                        PointGeometry = new EllipseGeometry(),
                        PointGeometrySize = 5,
                        Values = valuesImaginary,
                        Title = SelectedSignalW1ToDraw.Name + " Imaginary"
                    });
                }
                else
                {
                    RealSeries.Add(new ScatterSeries()
                    {
                        PointGeometry = new EllipseGeometry(),
                        StrokeThickness = 5,
                        Values = valuesReal,
                        Title = SelectedSignalW1ToDraw.Name + " Real"
                    });
                    ImaginarySeries.Add(new ScatterSeries()
                    {
                        PointGeometry = new EllipseGeometry(),
                        StrokeThickness = 5,
                        Values = valuesImaginary,
                        Title = SelectedSignalW1ToDraw.Name + " Imaginary"
                    });
                }

            }

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
                if (pointsX != null)
                {
                    for (int i = 0; i < pointsY.Count; i++)
                    {
                        values.Add(new ObservablePoint((double)i / pointsY.Count, pointsY[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < pointsY.Count; i++)
                    {
                        values.Add(new ObservablePoint((double)i / pointsY.Count, pointsY[i]));
                    }
                }
                SignalsOnChart.Add(SelectedSignalToDraw);
                if (SelectedSignalToDraw.GetType() == typeof(SampledSignal))
                {
                    if (DrawLine)
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
