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

        public SignalDialogViewModel(DataHandler Data, bool isScattered)
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

                if (isScattered || Data.FromSamples)
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
