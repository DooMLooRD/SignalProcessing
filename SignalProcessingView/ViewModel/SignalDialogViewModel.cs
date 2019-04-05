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

        public SignalDialogViewModel(DataHandler DataOriginal, DataHandler ReconstructedData, bool isScattered, bool drawOriginal, bool drawQuants, bool drawSamples, bool drawReconstructed)
        {
            if (DataOriginal.HasData())
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();


                if (ReconstructedData.Quants != null && ReconstructedData.Quants.Count > 0)
                {
                    ChartSeries = new SeriesCollection(mapper);
                    if (drawOriginal)
                    {
                        var pointsX = DataOriginal.PointsX;
                        var pointsY = DataOriginal.PointsY;
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
                        var samplesX = DataOriginal.SamplesX;
                        var samples = DataOriginal.Samples;
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
