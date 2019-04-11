using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using MaterialDesignThemes.Wpf;
using SignalProcessingCore;
using SignalProcessingMethods;
using SignalProcessingView.View;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class TabContentViewModel : BaseViewModel
    {
        public bool IsQuant
        {
            get => _isQuant;
            set
            {
                _isQuant = value;
                if (value)
                {
                    QuantVisibility = Visibility.Visible;
                    OriginalVisibility = Visibility.Collapsed;
                }
                else
                {
                    QuantVisibility = Visibility.Collapsed;
                    OriginalVisibility = Visibility.Visible;
                }
            }
        }

        public Visibility QuantVisibility { get; set; } = Visibility.Collapsed;
        public Visibility OriginalVisibility { get; set; } = Visibility.Visible;
        private int _sliderValue;
        private bool _isQuant;
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
        public double MSE { get; set; }
        public double SNR { get; set; }
        public double PSNR { get; set; }
        public double MD { get; set; }
        public double ENOB { get; set; }
        public bool DrawOriginal { get; set; } = true;
        public bool DrawSamples { get; set; } = true;
        public bool DrawQuants { get; set; } = true;
        public bool DrawReconstructed { get; set; } = true;

        public ICommand Histogram { get; set; }
        public ICommand SaveCharts { get; set; }
        public ICommand RunSignalDialogCommand { get; set; }
        public ICommand RunHistogramDialogCommand { get; set; }

        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                LoadHistogram(_sliderValue);
            }
        }
        private async void ExecuteRunDialog()
        {
            var view = new SignalDialog
            {
                DataContext = new SignalDialogViewModel(OriginalData, ReconstructedData, IsScattered, DrawOriginal, DrawQuants, DrawSamples, DrawReconstructed)
            };


            await DialogHost.Show(view);
        }
        private async void ExecuteRunHistogramDialog()
        {
            var view = new HistogramDialog()
            {
                DataContext = new HistogramDialogViewModel(OriginalData, SliderValue)
            };

            await DialogHost.Show(view);
        }
        public DataHandler OriginalData { get; set; }
        public DataHandler ReconstructedData { get; set; }

        public TabContentViewModel()
        {
            RunSignalDialogCommand = new RelayCommand(ExecuteRunDialog);
            RunHistogramDialogCommand = new RelayCommand(ExecuteRunHistogramDialog);
            OriginalData = new DataHandler();
            ReconstructedData = new DataHandler();
            Histogram = new RelayCommand<int>(LoadHistogram);
            SaveCharts = new RelayCommand(SaveChartsAsync);
            SliderValue = 20;
        }

        #region Save Charts

        public void SaveChartsAsync()
        {
            Thread t = new Thread((SaveChartsToFile));
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
        }
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
            if (OriginalData.FromSamples)
            {
                pointsX = OriginalData.SamplesX;
                pointsY = OriginalData.Samples;
            }
            else
            {
                pointsX = OriginalData.PointsX;
                pointsY = OriginalData.PointsY;
            }
            for (int i = 0; i < pointsX.Count; i++)
            {
                values.Add(new PointXY(pointsX[i], pointsY[i]));
            }

            if (IsScattered || OriginalData.FromSamples)
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


            var histogramResults = OriginalData.GetDataForHistogram(SliderValue);

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
            histogram.AxisX = new AxesCollection() { new Axis() { Title = "Interval", FontSize = 20, Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray(), LabelsRotation = 60, Separator = new LiveCharts.Wpf.Separator() { Step = (int)Math.Ceiling(SliderValue / 20.0) } } };

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

            SaveToPng(chart, "../../../OriginalData/chart.png");
            SaveToPng(histogram, "../../../OriginalData/histogram.png");
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
            if (OriginalData.HasData())
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();
                List<double> pointsX;
                List<double> pointsY;
                if (OriginalData.FromSamples)
                {
                    pointsX = OriginalData.SamplesX;
                    pointsY = OriginalData.Samples;
                }
                else
                {
                    pointsX = OriginalData.PointsX;
                    pointsY = OriginalData.PointsY;
                }
                for (int i = 0; i < pointsX.Count; i++)
                {
                    values.Add(new PointXY(pointsX[i], pointsY[i]));
                }

                if (IsScattered || OriginalData.FromSamples)
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


                var histogramResults = OriginalData.GetDataForHistogram(SliderValue);
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

        public void DrawQuantCharts(bool drawOriginal = true, bool drawQuants = false, bool drawSamples = false, bool drawReconstructed = false)
        {
            DrawOriginal = drawOriginal;
            DrawSamples = drawSamples;
            DrawQuants = drawQuants;
            DrawReconstructed = drawReconstructed;
            if (OriginalData.HasData() && ReconstructedData.Quants != null && ReconstructedData.Quants.Count > 0)
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartSeries = new SeriesCollection(mapper);
                if (drawOriginal)
                {
                    ChartValues<PointXY> values = new ChartValues<PointXY>();
                    var pointsX = OriginalData.PointsX;
                    var pointsY = OriginalData.PointsY;
                    for (int i = 0; i < pointsX.Count; i++)
                    {
                        values.Add(new PointXY(pointsX[i], pointsY[i]));
                    }
                    ChartSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 0.5,
                        Fill = Brushes.Transparent,
                        PointGeometry = null,
                        Values = values,
                        Title = "Oryginalny",
                    });
                }

                if (drawQuants)
                {
                    ChartValues<PointXY> quantsValues = new ChartValues<PointXY>();
                    var quantsX = ReconstructedData.SamplesX;
                    var quants = ReconstructedData.Quants;
                    for (int i = 0; i < quantsX.Count; i++)
                    {
                        quantsValues.Add(new PointXY(quantsX[i], quants[i]));

                    }
                    ChartSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 0.5,
                        Fill = Brushes.Transparent,
                        PointGeometry = null,
                        Values = quantsValues,
                        Title = "Kwantyzacja",
                    });
                }

                if (drawSamples)
                {
                    ChartValues<PointXY> samplesValues = new ChartValues<PointXY>();
                    var samplesX = OriginalData.SamplesX;
                    var samples = OriginalData.Samples;
                    for (int i = 0; i < samplesX.Count; i++)
                    {
                        samplesValues.Add(new PointXY(samplesX[i], samples[i]));
                    }
                    ChartSeries.Add(new ScatterSeries()
                    {
                        PointGeometry = new EllipseGeometry(),
                        StrokeThickness = 5,
                        Values = samplesValues,
                        Title = "Próbkowanie",
                    });
                }

                if (drawReconstructed)
                {
                    ChartValues<PointXY> reconstructedValues = new ChartValues<PointXY>();
                    var reconstructedX = ReconstructedData.PointsX;
                    var reconstructed = ReconstructedData.PointsY;
                    for (int i = 0; i < reconstructedX.Count; i++)
                    {
                        reconstructedValues.Add(new PointXY(reconstructedX[i], reconstructed[i]));
                    }
                    ChartSeries.Add(new LineSeries()
                    {
                        LineSmoothness = 0,
                        StrokeThickness = 0.5,
                        Fill = Brushes.Transparent,
                        PointGeometry = null,
                        Values = reconstructedValues,
                        Title = "Zrekonstruowany",
                    });
                }

                var histogramResults = OriginalData.GetDataForHistogram(SliderValue);
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
        public void CalculateSignalInfo(double t1 = 0, double t2 = 0, bool isDiscrete = false, bool fromSamples = false)
        {
            List<double> points;
            if (fromSamples)
                points = OriginalData.Samples;
            else
                points = OriginalData.PointsY;

            AvgSignal = SignalOperations.AvgSignal(points, t1, t2, isDiscrete);
            AbsAvgSignal = SignalOperations.AbsAvgSignal(points, t1, t2, isDiscrete);
            AvgSignalPower = SignalOperations.AvgSignalPower(points, t1, t2, isDiscrete);
            SignalVariance = SignalOperations.SignalVariance(points, t1, t2, isDiscrete);
            RMSSignal = SignalOperations.RMSSignal(points, t1, t2, isDiscrete);
        }

        public void CalculateSignalDifference()
        {
            Console.WriteLine(OriginalData.PointsX.Last());
            Console.WriteLine(ReconstructedData.PointsX.Last());
            MSE = SimilarityFunctions.CalculateMSE(OriginalData.PointsY, ReconstructedData.PointsY);
            Console.WriteLine(OriginalData.PointsY.Last());
            Console.WriteLine(ReconstructedData.PointsY.Last());
            SNR = SimilarityFunctions.CalculateSNR(OriginalData.PointsY, ReconstructedData.PointsY);
            PSNR = SimilarityFunctions.CalculatePNSR(OriginalData.PointsY, ReconstructedData.PointsY);
            MD = SimilarityFunctions.CalculateMD(OriginalData.PointsY, ReconstructedData.PointsY);
            ENOB = SimilarityFunctions.CalculateENOB(OriginalData.PointsY, ReconstructedData.PointsY);
        }
        public void LoadHistogram(int c)
        {
            SliderValue = c;
            if (OriginalData.HasData())
            {
                var histogramResults = OriginalData.GetDataForHistogram(c);
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
            OriginalData = data;
        }

        public void LoadData(List<double> x, List<double> y, bool fromSamples)
        {
            if (fromSamples)
            {
                OriginalData.FromSamples = true;
                OriginalData.Samples = y;
            }
            else
            {
                OriginalData.FromSamples = false;
                OriginalData.PointsX = x;
                OriginalData.PointsY = y;
            }


        }

        public void SaveDataToFile(string path)
        {
            OriginalData.SaveToFile(path);
        }
        public void LoadDataFromFile(string path)
        {
            OriginalData.FromSamples = true;
            OriginalData.LoadFromFile(path);
        }
    }
}
