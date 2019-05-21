using SignalProcessingCore.Signal;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad1;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Zad1
{
    public class Zad1ViewModel : BaseViewModel
    {

        public SignalCreator SignalCreator { get; set; }
        public ISignal SelectedSignal { get; set; }
        public SampledSignal SelectedSignal1 { get; set; }
        public SampledSignal SelectedSignal2 { get; set; }
        public string ResultSignalName { get; set; }

        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }

        public bool IsComplex { get; set; }

        public double AvgSignal { get; set; }
        public double AbsAvgSignal { get; set; }
        public double AvgSignalPower { get; set; }
        public double SignalVariance { get; set; }
        public double RMSSignal { get; set; }

        public ICommand ComputeCommand { get; set; }
        public ICommand ComputeSignalsCommand { get; set; }
        public ICommand SaveCharts { get; set; }
        public ICommand DrawHistogramCommand { get; set; }


        public Zad1ViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;

            Operations = new List<string>()
            {
                "(D1) Dodawanie",
                "(D2) Odejmowanie",
                "(D3) Mnożenie",
                "(D4) Dzielenie",
            };
            SelectedOperation = Operations[0];
            ComputeCommand = new RelayCommand(ComputeSignalInfo);
            ComputeSignalsCommand = new RelayCommand(ComputeSignals);
            ResultSignalName = "Result Signal";
        }
        public void ComputeSignals()
        {
            if (SelectedSignal1 != null && SelectedSignal2 != null)
            {
                if(IsComplex)
                {
                    SampledSignal signal = new SampledSignal();
                    List<Complex> points = new List<Complex>();

                    switch (SelectedOperation.Substring(1, 2))
                    {
                        case "D1":
                            points = SignalOperations.AddComplexSignals(SelectedSignal1.ComplexPoints,
                                SelectedSignal2.ComplexPoints);
                            break;
                        case "D2":
                            points= SignalOperations.SubtractComplexSignals(SelectedSignal1.ComplexPoints,
                                SelectedSignal2.ComplexPoints);
                            break;
                        case "D3":
                            points = SignalOperations.MultiplyComplexSignals(SelectedSignal1.ComplexPoints,
                                SelectedSignal2.ComplexPoints);
                            break;
                        case "D4":
                            points = SignalOperations.DivideComplexSignals(SelectedSignal1.ComplexPoints,
                                SelectedSignal2.ComplexPoints);
                            break;
                    }
                    signal.ComplexPoints = points;
                    signal.Name = ResultSignalName + " - S [Complex]";
                    SignalCreator.Signals.Add(signal);
                    SignalCreator.SampledSignals.Add(signal);
                }
                else
                {
                    if (!SelectedSignal2.IsValid(SelectedSignal2))
                    {
                        MessageBox.Show("Given signals are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    SampledSignal signal = new SampledSignal();
                    List<double> pointsY = new List<double>();

                    switch (SelectedOperation.Substring(1, 2))
                    {
                        case "D1":
                            pointsY = SignalOperations.AddSignals(SelectedSignal1.PointsY,
                                SelectedSignal2.PointsY);
                            break;
                        case "D2":
                            pointsY = SignalOperations.SubtractSignals(SelectedSignal1.PointsY,
                                SelectedSignal2.PointsY);
                            break;
                        case "D3":
                            pointsY = SignalOperations.MultiplySignals(SelectedSignal1.PointsY,
                                SelectedSignal2.PointsY);
                            break;
                        case "D4":
                            pointsY = SignalOperations.DivideSignals(SelectedSignal1.PointsY,
                                SelectedSignal2.PointsY);
                            break;
                    }
                    signal.PointsY = pointsY;
                    signal.StartTime = SelectedSignal1.StartTime;
                    signal.Frequency = SelectedSignal1.Frequency;
                    signal.CalculateSamplesX();
                    signal.Name = ResultSignalName + " - S";
                    SignalCreator.Signals.Add(signal);
                    SignalCreator.SampledSignals.Add(signal);
                }
            }
               

        }
        public void ComputeSignalInfo()
        {
            if (SelectedSignal != null && SelectedSignal.HasData())
            {
                var samples = SelectedSignal.PointsY;
                AvgSignal = SignalOperations.AvgSignal(samples);
                AbsAvgSignal = SignalOperations.AbsAvgSignal(samples);
                AvgSignalPower = SignalOperations.AvgSignalPower(samples);
                SignalVariance = SignalOperations.SignalVariance(samples);
                RMSSignal = SignalOperations.RMSSignal(samples);
            }

        }
    }
}
