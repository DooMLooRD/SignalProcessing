using SignalProcessingCore.Signal;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Zad3
{
    public class Zad3ViewModel : BaseViewModel
    {
        public SignalCreator SignalCreator { get; set; }

        public ISignal SelectedSignal1 { get; set; }
        public ISignal SelectedSignal2 { get; set; }
        public ISignal SelectedSignalCorrelate1 { get; set; }
        public ISignal SelectedSignalCorrelate2 { get; set; }

        public List<string> Filters { get; set; }
        public string SelectedFilter { get; set; }
        public List<string> Windows { get; set; }
        public string SelectedWindow { get; set; }
        public int M { get; set; }
        public int K { get; set; }
        public string FilterName { get; set; }

        public ICommand ConvoluteCommand { get; set; }
        public ICommand CorrelateCommand { get; set; }
        public ICommand CreateFilterCommand { get; set; }


        public Zad3ViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;

            Filters = new List<string>()
            {
                "(F0) Filtr dolnoprzepustowy",
                "(F1) Filtr środkowoprzepustowy",
                "(F2) Filtr górnoprzepustowy",
            };
            SelectedFilter = Filters[0];
            Windows = new List<string>()
            {
                "(O0) Okno Prostokątne",
                "(O1) Okno Hamminga",
                "(O2) Okno Hanninga",
                "(O3) Okno Blackmana",
            };
            SelectedWindow = Windows[0];
            ConvoluteCommand = new RelayCommand(Convolute);
            CorrelateCommand = new RelayCommand(Correlate);
            CreateFilterCommand = new RelayCommand(CreateFilter);


        }
        public void Convolute()
        {
            if (SelectedSignal1 != null && SelectedSignal1.HasData() && SelectedSignal2 != null && SelectedSignal2.HasData())
            {
                SampledSignal signal = new SampledSignal();
                signal.PointsY = Convolution.ComputeSignal(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
                signal.Name = $"({SelectedSignal1.Name})*({SelectedSignal2.Name})";
                SignalCreator.AddSignal(signal);
            }

        }
        public void Correlate()
        {
            if (SelectedSignalCorrelate1 != null && SelectedSignalCorrelate1.HasData() && SelectedSignalCorrelate2 != null && SelectedSignalCorrelate2.HasData())
            {
                SampledSignal signal = new SampledSignal();
                signal.PointsY = Correlation.ComputeSignal(SelectedSignalCorrelate1.PointsY, SelectedSignalCorrelate2.PointsY);
                signal.Name = $"({SelectedSignalCorrelate1.Name})C({SelectedSignalCorrelate2.Name})";
                SignalCreator.AddSignal(signal);
            }

        }
        public void CreateFilter()
        {
            Func<int, int, List<double>> filterFunction = null;
            Func<List<double>, int, List<double>> windowFunction = null;

            switch (SelectedFilter.Substring(1, 2))
            {
                case "F0":
                    filterFunction = Filter.LowPassFilter;
                    break;
                case "F1":
                    filterFunction = Filter.MidPassFilter;
                    break;
                case "F2":
                    filterFunction = Filter.HighPassFilter;
                    break;
            }
            switch (SelectedWindow.Substring(1, 2))
            {
                case "O0":
                    windowFunction = Filter.RectangularWindow;
                    break;
                case "O1":
                    windowFunction = Filter.HammingWindow;
                    break;
                case "O2":
                    windowFunction = Filter.HanningWindow;
                    break;
                case "O3":
                    windowFunction = Filter.BlackmanWindow;
                    break;
            }
            SampledSignal signal = new SampledSignal();
            signal.PointsY = Filter.CreateFilterSignal(M, K, filterFunction, windowFunction);
            signal.Name = FilterName + " - F";
            SignalCreator.AddSignal(signal);

        }

    }
}
