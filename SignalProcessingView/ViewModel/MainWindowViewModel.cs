using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SignalProcessingCore;
using SignalProcessingMethods;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        #region Properties
        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }

        public List<string> SignalTypes { get; set; }
        public string SelectedSignalType { get; set; }

        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public TabViewModel SelectedSignal1Tab { get; set; }
        public TabViewModel SelectedSignal2Tab { get; set; }
        public TabViewModel SelectedResultTab { get; set; }

        #region Factors
        public double A { get; set; }
        public double T1 { get; set; }
        public double Ts { get; set; }
        public double D { get; set; }
        public double T { get; set; }
        public double Kw { get; set; }
        public double F { get; set; }
        public double N1 { get; set; }
        public double Ns { get; set; }
        public double P { get; set; }
        #endregion




        public ICommand AddPageCommand { get; set; }
        public ICommand PlotCommand { get; set; }
        public ICommand ComputeCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>() { new TabViewModel("Tab0") };
            SelectedTab = Tabs[0];
            SelectedSignal1Tab = Tabs[0];
            SelectedSignal2Tab = Tabs[0];
            SelectedResultTab = Tabs[0];

            SignalTypes = new List<string>()
            {
                "(S01) Szum o rozkładzie jednostajnym",
                "(S02) Szum Gaussowski",
                "(S03) Sygnał sinusoidalny",
                "(S04) Sygnał sinusoidalny wyprostowany jednopołówkowo",
                "(S05) Sygnał sinusoidalny wyprostowany dwupołówkowo",
                "(S06) Sygnał prostokątny",
                "(S07) Sygnał prostokątny symetryczny",
                "(S08) Sygnał trójkątny",
                "(S09) Skok jednostkowy",
                "(S10) Impuls jednostkowy",
                "(S11) Szum impulsowy"
            };
            SelectedSignalType = SignalTypes[0];

            Operations = new List<string>()
            {
                "(D1) Dodawanie",
                "(D2) Odejmowanie",
                "(D3) Mnożenie",
                "(D4) Dzielenie",
            };
            SelectedOperation = Operations[0];
            AddPageCommand = new RelayCommand(AddPage);
            PlotCommand = new RelayCommand(Plot);
            ComputeCommand = new RelayCommand(Compute);
        }

        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Tab" + Tabs.Count));
        }

        public void Compute()
        {
            SignalOperations singalOps = new SignalOperations();
            if (SelectedSignal1Tab.TabContent.HasData && SelectedSignal2Tab.TabContent.HasData)
            {
                List<double> pointsX = SelectedSignal1Tab.TabContent.Data.PointsX;
                List<double> pointsY = new List<double>();
                switch (SelectedOperation.Substring(1, 2))
                {
                    case "D1":
                        pointsY = singalOps.AddSignals(SelectedSignal1Tab.TabContent.Data.PointsY,
                            SelectedSignal2Tab.TabContent.Data.PointsY);
                        break;
                    case "D2":
                        pointsY = singalOps.SubtractSignals(SelectedSignal1Tab.TabContent.Data.PointsY,
                            SelectedSignal2Tab.TabContent.Data.PointsY);
                        break;
                    case "D3":
                        pointsY = singalOps.MultiplySignals(SelectedSignal1Tab.TabContent.Data.PointsY,
                            SelectedSignal2Tab.TabContent.Data.PointsY);
                        break;
                    case "D4":
                        pointsY = singalOps.DivideSignals(SelectedSignal1Tab.TabContent.Data.PointsY,
                            SelectedSignal2Tab.TabContent.Data.PointsY);
                        break;
                }
                SelectedResultTab.TabContent.LoadData(pointsX, pointsY);
                SelectedResultTab.TabContent.DrawCharts();
            }

        }

        public void Plot()
        {
            SignalGenerator generator = new SignalGenerator()
            {
                Amplitude = A,
                Duration = D,
                FillFactor = Kw,
                Period = T,
                StartTime = T1
            };
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();


            switch (SelectedSignalType.Substring(1, 3))
            {
                case "S01":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateUniformDistributionNoise());
                    }
                    break;
                case "S02":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateGaussianNoise());
                    }
                    break;
                case "S03":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateSinusoidalSignal(i));
                    }
                    break;
                case "S04":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateSinusoidal1PSignal(i));
                    }
                    break;
                case "S05":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateSinusoidal2PSignal(i));
                    }
                    break;
                case "S06":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateRectangularSignal(i));
                    }
                    break;
                case "S07":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateRectangularSymmetricalSignal(i));
                    }
                    break;
                case "S08":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateTriangularSignal(i));
                    }
                    break;
                case "S09":
                    for (double i = T1; i < T1 + D; i += D / 1000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(generator.GenerateUnitJump(i, Ts));
                    }
                    break;
                case "S10":
                    break;
                case "S11":
                    break;

            }
            SelectedTab.TabContent.LoadData(pointsX, pointsY);
            SelectedTab.TabContent.DrawCharts();
        }
    }
}
