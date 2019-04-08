using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dragablz;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SignalProcessingCore;
using SignalProcessingMethods;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad2;

namespace SignalProcessingView.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        #region Properties
        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }

        public List<string> SignalTypes { get; set; }
        public string SelectedSignalType { get; set; }

        public List<string> Reconstructions { get; set; }
        public string SelectedReconstruction { get; set; }

        public List<string> ReconstructFrom { get; set; }
        public string SelectedReconstructFrom { get; set; }

        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public TabViewModel SelectedSignal1Tab { get; set; }
        public TabViewModel SelectedSignal2Tab { get; set; }
        public TabViewModel SelectedResultTab { get; set; }
        public TabViewModel SelectedQuantTab { get; set; }
        public TabViewModel SelectedQuantResultTab { get; set; }

        #region Factors
        public double A { get; set; }
        public double T1 { get; set; }
        public double Ts { get; set; }
        public double D { get; set; }
        public double T { get; set; }
        public double Kw { get; set; }
        public double F { get; set; }
        public double N { get; set; }
        public double N1 { get; set; }
        public double Ns { get; set; }
        public double P { get; set; }
        public double Fp { get; set; }
        public int QuantCount { get; set; }
        public int NSamples { get; set; }
        #endregion

        public bool DrawOriginal { get; set; } = true;
        public bool DrawSamples { get; set; } = true;
        public bool DrawQuants { get; set; } = true;
        public bool DrawReconstructed { get; set; } = true;

        public ICommand AddPageCommand { get; set; }
        public ICommand PlotCommand { get; set; }
        public ICommand ComputeCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand ToggleBaseCommand { get; }
        public ICommand QuantCommand { get; set; }
        #endregion


        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>() { new TabViewModel("Tab0") };
            SelectedTab = Tabs[0];
            SelectedSignal1Tab = Tabs[0];
            SelectedSignal2Tab = Tabs[0];
            SelectedResultTab = Tabs[0];
            SelectedQuantTab = Tabs[0];
            SelectedQuantResultTab = Tabs[0];

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

            Reconstructions = new List<string>()
            {
                "(R1) Ekstrapolacja zerowego rzędu",
                "(R3) Rekonstrukcja w oparciu o funkcję sinc",
            };
            SelectedReconstruction = Reconstructions[0];

            ReconstructFrom = new List<string>()
            {
                "Próbki",
                "Kwanty"
            };
            SelectedReconstructFrom = ReconstructFrom[0];

            AddPageCommand = new RelayCommand(AddPage);
            PlotCommand = new RelayCommand(Plot);
            ComputeCommand = new RelayCommand(Compute);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            ToggleBaseCommand = new RelayCommand<bool>(ApplyBase);
            QuantCommand = new RelayCommand(Quant);
        }
        private static void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
        }
        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Tab" + Tabs.Count));
        }

        public void Save()
        {
            SelectedTab.TabContent.SaveDataToFile(LoadPath(false));
        }

        public void Load()
        {
            SelectedTab.TabContent.LoadDataFromFile(LoadPath(true));
            SelectedTab.TabContent.DrawCharts();
            SelectedTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
        }
        public string LoadPath(bool loadMode)
        {
            if (loadMode)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Bin File(*.bin)| *.bin",
                    RestoreDirectory = true
                };
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length == 0)
                {
                    MessageBox.Show("No files selected");
                    return null;
                }

                return openFileDialog.FileName;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Bin File(*.bin)| *.bin",
                RestoreDirectory = true
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
                return null;
            }

            return saveFileDialog.FileName;
        }

        public void Quant()
        {
            PlotTab(SelectedQuantResultTab);
            var data = SelectedQuantResultTab.TabContent.ReconstructedData;
            SelectedQuantResultTab.TabContent.IsQuant = true;
            SelectedQuantResultTab.TabContent.ReconstructedData.Quants = Quantization.Quantize(data.Samples, QuantCount);
            List<double> forReconstruct;
            if (SelectedReconstructFrom == "Próbki")
                forReconstruct = data.Samples;
            else
                forReconstruct = data.Quants;
            List<(double, double)> reconstructed;
            if (SelectedReconstruction.Substring(1, 2) == "R1")
                reconstructed = Reconstruction
                    .ZeroOrderHold(data.SamplesX.Zip(forReconstruct, (d, d1) => (d, d1)).ToList(), data.Frequency)
                    .ToList();
            else
                reconstructed = Reconstruction
                    .SincReconstruction(data.SamplesX.Zip(forReconstruct, (d, d1) => (d, d1)).ToList(), data.Frequency, NSamples)
                    .ToList();

            SelectedQuantResultTab.TabContent.ReconstructedData.PointsX = reconstructed.Select(c => c.Item1).ToList();
            SelectedQuantResultTab.TabContent.ReconstructedData.PointsY = reconstructed.Select(c => c.Item2).ToList();
            SelectedQuantResultTab.TabContent.DrawQuantCharts(DrawOriginal, DrawQuants, DrawSamples, DrawReconstructed);

        }
        public void Compute()
        {
            SelectedSignal1Tab.TabContent.OriginalData.FromSamples = true;
            SelectedSignal2Tab.TabContent.OriginalData.FromSamples = true;
            if (SelectedSignal1Tab.TabContent.OriginalData.HasData() && SelectedSignal2Tab.TabContent.OriginalData.HasData())
            {
                if (!SelectedSignal2Tab.TabContent.OriginalData.IsValid(SelectedSignal1Tab.TabContent.OriginalData))
                {
                    MessageBox.Show("Given signals are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataHandler data = new DataHandler();
                List<double> pointsY = new List<double>();

                switch (SelectedOperation.Substring(1, 2))
                {
                    case "D1":
                        pointsY = SignalOperations.AddSignals(SelectedSignal1Tab.TabContent.OriginalData.Samples,
                            SelectedSignal2Tab.TabContent.OriginalData.Samples);
                        break;
                    case "D2":
                        pointsY = SignalOperations.SubtractSignals(SelectedSignal1Tab.TabContent.OriginalData.Samples,
                            SelectedSignal2Tab.TabContent.OriginalData.Samples);
                        break;
                    case "D3":
                        pointsY = SignalOperations.MultiplySignals(SelectedSignal1Tab.TabContent.OriginalData.Samples,
                            SelectedSignal2Tab.TabContent.OriginalData.Samples);
                        break;
                    case "D4":
                        pointsY = SignalOperations.DivideSignals(SelectedSignal1Tab.TabContent.OriginalData.Samples,
                            SelectedSignal2Tab.TabContent.OriginalData.Samples);
                        break;
                }

                data.StartTime = SelectedSignal1Tab.TabContent.OriginalData.StartTime;
                data.Frequency = SelectedSignal1Tab.TabContent.OriginalData.Frequency;
                data.Samples = pointsY;
                data.FromSamples = true;
                SelectedResultTab.TabContent.IsScattered = true;
                SelectedResultTab.TabContent.LoadData(data);
                SelectedResultTab.TabContent.DrawCharts();
                SelectedResultTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
            }

        }

        private void PlotTab(TabViewModel signalType)
        {
            SignalGenerator generator = new SignalGenerator()
            {
                Amplitude = A,
                FillFactor = Kw,
                Period = T,
                StartTime = T1,
                JumpTime = Ts,
                JumpN = Ns,
                Probability = P
            };
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();
            List<double> samples = new List<double>();

            Func<double, double> func = null;
            switch (SelectedSignalType.Substring(1, 3))
            {
                case "S01":
                    func = generator.GenerateUniformDistributionNoise;
                    break;
                case "S02":
                    func = generator.GenerateGaussianNoise;
                    break;
                case "S03":
                    func = generator.GenerateSinusoidalSignal;
                    break;
                case "S04":
                    func = generator.GenerateSinusoidal1PSignal;
                    break;
                case "S05":
                    func = generator.GenerateSinusoidal2PSignal;
                    break;
                case "S06":
                    func = generator.GenerateRectangularSignal;
                    break;
                case "S07":
                    func = generator.GenerateRectangularSymmetricalSignal;
                    break;
                case "S08":
                    func = generator.GenerateTriangularSignal;
                    break;
                case "S09":
                    func = generator.GenerateUnitJump;
                    break;
                case "S10":
                    func = generator.GenerateUnitPulse;
                    break;
                case "S11":
                    func = generator.GenerateImpulseNoise;
                    break;

            }


            if (func != null)
            {
                generator.Func = func;
                bool isScattered = false;
                if (func.Method.Name.Contains("GenerateUnitPulse"))
                {
                    isScattered = true;
                    for (double i = N1 * F; i <= (D + N1) * F; i++)
                    {
                        pointsX.Add(i / F);
                        pointsY.Add(func(i / F));
                    }
                    signalType.TabContent.OriginalData.Frequency = F;
                    signalType.TabContent.OriginalData.StartTime = N1;
                    signalType.TabContent.OriginalData.Samples = pointsY;
                    signalType.TabContent.ReconstructedData.Frequency = F;
                    signalType.TabContent.ReconstructedData.StartTime = N1;
                    signalType.TabContent.ReconstructedData.Samples = pointsY;
                    signalType.TabContent.LoadData(pointsX, pointsY, false);
                    signalType.TabContent.CalculateSignalInfo(N1, N1 + D, true);
                }
                else if (func.Method.Name.Contains("GenerateImpulseNoise"))
                {
                    isScattered = true;
                    for (double i = N1; i <= D + N1; i += 1 / F)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(0));
                    }

                    signalType.TabContent.OriginalData.Frequency = F;
                    signalType.TabContent.OriginalData.StartTime = N1;
                    signalType.TabContent.OriginalData.Samples = pointsY;
                    signalType.TabContent.ReconstructedData.Frequency = F;
                    signalType.TabContent.ReconstructedData.StartTime = N1;
                    signalType.TabContent.ReconstructedData.Samples = pointsY;
                    signalType.TabContent.LoadData(pointsX, pointsY, false);
                    signalType.TabContent.CalculateSignalInfo(N1, N1 + D, true);
                }
                else
                {
                    for (double i = T1; i <= T1 + D; i += 1 / Fp)
                    {
                        samples.Add(func(i));
                    }
                    for (double i = T1; i < T1 + D; i += D / 5000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(i));
                    }
                    signalType.TabContent.OriginalData.Frequency = Fp;
                    signalType.TabContent.OriginalData.StartTime = T1;
                    signalType.TabContent.OriginalData.Samples = samples;
                    signalType.TabContent.ReconstructedData.Frequency = Fp;
                    signalType.TabContent.ReconstructedData.StartTime = N1;
                    signalType.TabContent.ReconstructedData.Samples = samples;
                    signalType.TabContent.LoadData(pointsX, pointsY, false);
                    signalType.TabContent.CalculateSignalInfo(T1, T1 + D);
                }

                signalType.TabContent.IsScattered = isScattered;
            }
        }
        public void Plot()
        {

            PlotTab(SelectedTab);
            SelectedTab.TabContent.DrawCharts();
        }
    }
}
