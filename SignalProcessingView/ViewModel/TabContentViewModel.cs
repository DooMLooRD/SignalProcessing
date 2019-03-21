using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SignalProcessingCore;
using SignalProcessingMethods;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class TabContentViewModel : BaseViewModel
    {
        private int _sliderValue;
        public SeriesCollection ChartSeries { get; set; }
        public bool IsScattered { get; set; }
        public SeriesCollection HistogramSeries { get; set; }
        public int HistogramStep { get; set; }
        public string[] Labels { get; set; }

        public double AvgSignal { get; set; }
        public double AbsAvgSignal { get; set; }
        public double AvgSignalPower { get; set; }
        public double SignalVariance { get; set; }
        public double RMSSignal { get; set; }

        public ICommand Histogram { get; set; }
        public ICommand SaveCharts { get; set; }

        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                LoadHistogram(_sliderValue);
            }
        }

        public DataHandler Data { get; set; }

        public TabContentViewModel()
        {
            Data = new DataHandler();
            Histogram = new RelayCommand<int>(LoadHistogram);
            SaveCharts = new RelayCommand(SaveChartsToFile);
        }

        #region Save Charts

        public void SaveChartsToFile()
        {
            var chart = new LiveCharts.Wpf.CartesianChart()
            {
                Background = new SolidColorBrush(Colors.White),
                DisableAnimations = true,
                Width = 1920,
                Height = 1080,
                DataTooltip = null,
                Hoverable = false,

            };
            var histogram = new LiveCharts.Wpf.CartesianChart()
            {
                Background = new SolidColorBrush(Colors.White),
                DisableAnimations = true,
                Width = 1920,
                Height = 1080,
                DataTooltip = null,
                Hoverable = false,
            };
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
                chart.Series = new SeriesCollection(mapper)
                    {
                        new ScatterSeries()
                        {
                            PointGeometry = new EllipseGeometry(),
                            StrokeThickness = 8,
                            Values = values,
                        }
                    };
            }
            else
            {
                chart.Series = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 1,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values,
                        }
                    };
            }


            var histogramResults = Data.GetDataForHistogram(SliderValue);

            //HistogramStep = (int)Math.Ceiling(SliderValue / 20.0);
            histogram.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3)),
                    ColumnPadding = 0,

                }
            };
            //Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();
            chart.AxisX = new AxesCollection() { new Axis() { FontSize = 20, Title = "t[s]" } };
            chart.AxisY = new AxesCollection() { new Axis() { FontSize = 20, Title = "A" } };

            histogram.AxisY = new AxesCollection() { new Axis() { FontSize = 20, } };
            histogram.AxisX = new AxesCollection() { new Axis() { FontSize = 20, Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray(), LabelsRotation = 60, Separator = new LiveCharts.Wpf.Separator() { Step = (int)Math.Ceiling(SliderValue / 20.0) } } };

            var viewbox = new Viewbox();
            viewbox.Child = chart;
            viewbox.Measure(chart.RenderSize);
            viewbox.Arrange(new Rect(new Point(0, 0), chart.RenderSize));
            chart.Update(true, true); //force chart redraw
            viewbox.UpdateLayout();

            var histViewbox = new Viewbox();
            histViewbox.Child = histogram;
            histViewbox.Measure(histogram.RenderSize);
            histViewbox.Arrange(new Rect(new Point(0, 0), histogram.RenderSize));
            histogram.Update(true, true); //force chart redraw
            histViewbox.UpdateLayout();

            SaveToPng(chart, "../../../Data/chart.png");
            SaveToPng(histogram, "../../../Data/histogram.png");
            MessageBox.Show("Files saved", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            //png file was created at the root directory.
        }

        private void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, fileName, encoder);
        }

        private static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
        }

        #endregion

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
                            Values = values,

                        }
                    };
                }
                else
                {
                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values,

                        }
                    };
                }


                var histogramResults = Data.GetDataForHistogram(5);
                HistogramStep = 1;
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

        public void CalculateSignalInfo(double t1, double t2, bool isDiscrete = false)
        {
            AvgSignal = SignalOperations.AvgSignal(Data.PointsY, t1, t2, isDiscrete);
            AbsAvgSignal = SignalOperations.AbsAvgSignal(Data.PointsY, t1, t2, isDiscrete);
            AvgSignalPower = SignalOperations.AvgSignalPower(Data.PointsY, t1, t2, isDiscrete);
            SignalVariance = SignalOperations.SignalVariance(Data.PointsY, t1, t2, isDiscrete);
            RMSSignal = SignalOperations.RMSSignal(Data.PointsY, t1, t2, isDiscrete);

        }

        public void LoadHistogram(int c)
        {
            if (Data.HasData())
            {
                var histogramResults = Data.GetDataForHistogram(c);
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
