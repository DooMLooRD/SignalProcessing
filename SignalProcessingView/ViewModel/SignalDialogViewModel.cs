using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using SignalProcessingMethods;

namespace SignalProcessingView.ViewModel
{
    class SignalDialogViewModel
    {
        public SeriesCollection ChartSeries { get; set; }

        public SignalDialogViewModel(DataHandler DataOriginal,DataHandler ReconstructedData, bool isScattered)
        {
            if (DataOriginal.HasData())
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();
                

                if (ReconstructedData.Quants != null && ReconstructedData.Quants.Count > 0)
                {
                    ChartValues<PointXY> quantsValues = new ChartValues<PointXY>();

                    var pointsX = DataOriginal.PointsX;
                    var pointsY = DataOriginal.PointsY;

                    for (int i = 0; i < pointsX.Count; i++)
                    {
                        values.Add(new PointXY(pointsX[i], pointsY[i]));
                    }

                    var samplesX = ReconstructedData.SamplesX;
                    var quant = ReconstructedData.Quants;
                    for (int i = 0; i < samplesX.Count; i++)
                    {
                        quantsValues.Add(new PointXY(samplesX[i], quant[i]));
                    }

                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values,
                        },
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = quantsValues,
                        }
                    };
                }
                else
                {
                    List<double> pointsX;
                    List<double> pointsY;
                    if (DataOriginal.FromSamples)
                    {
                        pointsX = DataOriginal.SamplesX;
                        pointsY = DataOriginal.Samples;
                    }
                    else
                    {
                        pointsX = DataOriginal.PointsX;
                        pointsY = DataOriginal.PointsY;
                    }

                    for (int i = 0; i < pointsX.Count; i++)
                    {
                        values.Add(new PointXY(pointsX[i], pointsY[i]));
                    }
                    if (isScattered || DataOriginal.FromSamples)
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
                }

            }
        }
    }
}
