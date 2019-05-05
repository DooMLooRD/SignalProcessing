using Microsoft.Win32;
using SignalProcessingCore;
using SignalProcessingCore.Signal;
using SignalProcessingCore.Utils;
using SignalProcessingView.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel
{
    public class SignalCreator : BaseViewModel
    {
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
        #endregion

        public bool IsContinuous { get; set; }
        public bool IsSampled { get; set; }
        public List<string> SignalTypes { get; set; }
        public string SelectedSignalType { get; set; }
        public double Fp { get; set; }
        public string SignalName { get; set; }

        public ObservableCollection<ISignal> Signals { get; set; }
        public ISignal SelectedSignalToRemove { get; set; }

        public ObservableCollection<SampledSignal> SampledSignals { get; set; }
        public SampledSignal SelectedSignalToSave { get; set; }

        public ObservableCollection<ContinuousSignal> ContinuousSignals { get; set; }

        public ICommand AddSignalCommand { get; set; }
        public ICommand LoadSignalCommand { get; set; }
        public ICommand RemoveSignalCommand { get; set; }
        public ICommand SaveSignalCommand { get; set; }

        public SignalCreator()
        {
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
            Signals = new ObservableCollection<ISignal>();
            ContinuousSignals = new ObservableCollection<ContinuousSignal>();
            SampledSignals = new ObservableCollection<SampledSignal>();
            AddSignalCommand = new RelayCommand(AddSignal);
            LoadSignalCommand = new RelayCommand(LoadSignal);
            RemoveSignalCommand = new RelayCommand(RemoveSignal);
            SaveSignalCommand = new RelayCommand(SaveSignal);
        }
        public void SaveSignal()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Bin File(*.bin)| *.bin",
                RestoreDirectory = true
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
                return;
            }
            DataHandler handler = new DataHandler();
            handler.SaveToFile(saveFileDialog.FileName, SelectedSignalToSave);


        }
        public void AddSignal(ISignal signal)
        {
            int count = Signals.Count(c => c.Name.Contains(signal.Name));
            signal.Name = count != 0 ? $"{ signal.Name}({count})" : signal.Name;
            if (signal.GetType() == typeof(SampledSignal))
            {
                SampledSignals.Add((SampledSignal)signal);
            }
            if(signal.GetType() == typeof(ContinuousSignal))
            {
                ContinuousSignals.Add((ContinuousSignal)signal);
            }
            Signals.Add(signal);
        }
        public void RemoveSignal()
        {
            if (SelectedSignalToRemove != null)
            {
                if (SampledSignals.Contains(SelectedSignalToRemove))
                    SampledSignals.Remove((SampledSignal)SelectedSignalToRemove);
                Signals.Remove(SelectedSignalToRemove);
            }

        }
        public void LoadSignal()
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
                return;
            }
            DataHandler handler = new DataHandler();
            var signal = handler.LoadFromFile(openFileDialog.FileName);
            signal.Name = SignalName + " - S";
            AddSignal(signal);
        }
        public void AddSignal()
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
                if (func.Method.Name.Contains("GenerateUnitPulse"))
                {
                    for (double i = N1 * F; i <= (D + N1) * F; i++)
                    {
                        pointsX.Add(i / F);
                        pointsY.Add(func(i / F));
                    }

                    SampledSignal signal = new SampledSignal();
                    signal.Frequency = F;
                    signal.StartTime = N1;
                    signal.PointsY = pointsY;
                    signal.PointsX = pointsX;
                    signal.Name = SignalName + " - S";
                    AddSignal(signal);
                }
                else if (func.Method.Name.Contains("GenerateImpulseNoise"))
                {
                    for (double i = N1; i <= D + N1; i += 1 / F)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(0));
                    }

                    SampledSignal signal = new SampledSignal();
                    signal.Frequency = F;
                    signal.StartTime = N1;
                    signal.PointsY = pointsY;
                    signal.PointsX = pointsX;
                    signal.Name = SignalName + " - S";
                    AddSignal(signal);
                }
                else
                {

                    if (IsContinuous)
                    {
                        for (double i = T1; i < T1 + D; i += D / 5000)
                        {
                            pointsX.Add(i);
                            pointsY.Add(func(i));
                        }
                        ContinuousSignal signal = new ContinuousSignal();
                        signal.PointsY = pointsY;
                        signal.PointsX = pointsX;
                        signal.Name = SignalName + " - C";
                        AddSignal(signal);
                    }
                    if (IsSampled)
                    {
                        for (double i = T1; i <= T1 + D + 0.00000001; i += 1 / Fp)
                        {
                            samples.Add(func(i));
                        }
                        SampledSignal signal = new SampledSignal();
                        signal.Frequency = Fp;
                        signal.StartTime = T1;
                        signal.PointsY = samples;
                        signal.Name = SignalName + " - S";
                        signal.CalculateSamplesX();
                        AddSignal(signal);
                    }


                }
            }
        }


    }
}
