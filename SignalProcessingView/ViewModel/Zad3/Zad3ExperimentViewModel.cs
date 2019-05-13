using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SignalProcessingCore.Signal;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;

namespace SignalProcessingView.ViewModel.Zad3
{
    public class Zad3ExperimentViewModel : BaseViewModel
    {
        private Timer _timer;
        public SignalCreator SignalCreator { get; set; }
        public SeriesCollection ChartSeriesOriginal { get; set; }
        public SeriesCollection ChartSeriesReceived { get; set; }
        public SeriesCollection ChartSeriesCorrelate { get; set; }
        public ChartValues<double> ReceivedValues { get; set; }
        public ChartValues<double> CorrelateValues { get; set; }
        public bool IsStarted { get; set; }
        public double S { get; set; }
        public double ObjectV { get; set; }
        public double V { get; set; }
        public double CalculatedS { get; set; }
        public double T { get; set; }

        public ISignal SelectedSignal { get; set; }

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public Zad3ExperimentViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }


        private void Start()
        {
            if (SelectedSignal != null && SelectedSignal.HasData())
            {
                IsStarted = true;
                FirstDraw();
                _timer = new Timer(T * 1000);
                _timer.Elapsed += Timer_Elapsed;
                _timer.Start();
            }
        }

        private void Stop()
        {
            _timer.Stop();
            IsStarted = false;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            S += ObjectV * T;

            SampledSignal signal = (SampledSignal)SelectedSignal;
            int samplesToMove = (int)(S / V * signal.Frequency * 2);
            int samplesLeft = signal.PointsY.Count - samplesToMove;
            List<double> pointsLeft = signal.PointsY.Take(samplesLeft).ToList();
            List<double> receivedSignal = signal.PointsY.Skip(samplesLeft).ToList();
            receivedSignal.AddRange(pointsLeft);
            List<double> correlateSignal = Correlation.ComputeSignal(SelectedSignal.PointsY, receivedSignal);
            DrawReceived(receivedSignal);
            DrawCorrelate(correlateSignal);
            List<double> rightHalf = correlateSignal.Skip((correlateSignal.Count - 1) / 2).ToList();
            int maximum = rightHalf.FindIndex(c => c == rightHalf.Max());
            CalculatedS = V * (maximum / signal.Frequency) / 2;
        }
        private void FirstDraw()
        {
            ChartSeriesOriginal = new SeriesCollection();
            ChartSeriesReceived = new SeriesCollection();
            ChartSeriesCorrelate = new SeriesCollection();
            ChartValues<double> values = new ChartValues<double>();
            ReceivedValues = new ChartValues<double>();
            CorrelateValues = new ChartValues<double>();

            List<double> pointsY = SelectedSignal.PointsY;
            List<double> correlateSignal = Correlation.ComputeSignal(SelectedSignal.PointsY, SelectedSignal.PointsY);

            for (int i = 0; i < pointsY.Count; i++)
            {
                values.Add(pointsY[i]);
                ReceivedValues.Add(pointsY[i]);
            }
            for (int i = 0; i < correlateSignal.Count; i++)
            {
                CorrelateValues.Add(correlateSignal[i]);
            }

            ChartSeriesOriginal.Add(new ScatterSeries()
            {
                StrokeThickness = 5,
                PointGeometry = new EllipseGeometry(),
                Values = values
            });
            ChartSeriesReceived.Add(new ScatterSeries()
            {
                StrokeThickness = 5,
                PointGeometry = new EllipseGeometry(),
                Values = ReceivedValues
            });
            ChartSeriesCorrelate.Add(new ScatterSeries()
            {
                StrokeThickness = 5,
                PointGeometry = new EllipseGeometry(),
                Values = CorrelateValues
            });
        }
       
        private void DrawReceived(List<double> receivedSignal)
        {
            for (int i = 0; i < receivedSignal.Count; i++)
            {
                ReceivedValues[i] = receivedSignal[i];
            }
        }
        private void DrawCorrelate(List<double> correlateSignal)
        {
            for (int i = 0; i < correlateSignal.Count; i++)
            {
                CorrelateValues[i] = correlateSignal[i];
            }
        }

    }
}
