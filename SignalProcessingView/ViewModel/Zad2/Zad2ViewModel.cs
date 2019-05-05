using SignalProcessingCore.Signal;
using SignalProcessingMethods;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad2;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Zad2
{
    public class Zad2ViewModel : BaseViewModel
    {
        public SignalCreator SignalCreator { get; set; }
        public ISignal SelectedSampledSignal { get; set; }
        public ISignal SelectedReconstructSignal { get; set; }
        public ISignal SelectedSignal1 { get; set; }
        public ISignal SelectedSignal2 { get; set; }
        public string ReconstructedSignalName { get; set; }
        public string QuantSignalName { get; set; }
        public double MSE { get; set; }
        public double SNR { get; set; }
        public double PSNR { get; set; }
        public double MD { get; set; }
        public double ENOB { get; set; }
        public int QuantCount { get; set; }
        public int NSamples { get; set; }
        public List<string> Reconstructions { get; set; }
        public string SelectedReconstruction { get; set; }


        public ICommand ComputeCommand { get; set; }
        public ICommand QuantCommand { get; set; }
        public ICommand ReconstructCommand { get; set; }



        public Zad2ViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;

            Reconstructions = new List<string>()
            {
                "(R1) Ekstrapolacja zerowego rzędu",
                "(R3) Rekonstrukcja w oparciu o funkcję sinc",
            };
            SelectedReconstruction = Reconstructions[0];
            QuantCommand = new RelayCommand(Quant);
            ReconstructCommand = new RelayCommand(Reconstruct);
            ComputeCommand = new RelayCommand(ComputeSignalsDifference);


        }
        public void Quant()
        {
            if (SelectedSampledSignal != null && SelectedSampledSignal.HasData())
            {
                var sampledSignal = (SampledSignal)SelectedSampledSignal;
                SampledSignal signal = new SampledSignal();
                signal.PointsX = sampledSignal.PointsX;
                signal.StartTime = sampledSignal.StartTime;
                signal.Frequency = sampledSignal.Frequency;
                signal.PointsY = Quantization.Quantize(sampledSignal.PointsY, QuantCount);
                signal.Name = QuantSignalName + " - Q";
                SignalCreator.AddSignal(signal);
            }

        }
        public void Reconstruct()
        {
            if (SelectedReconstructSignal != null && SelectedReconstructSignal.HasData())
            {
                var sampledSignal = (SampledSignal)SelectedReconstructSignal;
                ContinuousSignal signal = new ContinuousSignal();
                List<(double, double)> reconstructed;
                if (SelectedReconstruction.Substring(1, 2) == "R1")
                {
                    reconstructed = Reconstruction
                              .ZeroOrderHold(sampledSignal.PointsX.Zip(sampledSignal.PointsY, (d, d1) => (d, d1)).ToList(), sampledSignal.Frequency, NSamples)
                              .ToList();
                }
                else
                {
                    reconstructed = Reconstruction
                          .SincReconstruction(sampledSignal.PointsX.Zip(sampledSignal.PointsY, (d, d1) => (d, d1)).ToList(), sampledSignal.Frequency, NSamples)
                          .ToList();
                }
                signal.PointsX = reconstructed.Select(c => c.Item1).ToList();
                signal.PointsY = reconstructed.Select(c => c.Item2).ToList();
                signal.Name = ReconstructedSignalName + " - R";
                SignalCreator.AddSignal(signal);
            }

        }
        public void ComputeSignalsDifference()
        {
            if (SelectedSignal1 != null && SelectedSignal2 != null && SelectedSignal1.HasData() && SelectedSignal2.HasData())
            {
                MSE = SimilarityFunctions.CalculateMSE(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
                SNR = SimilarityFunctions.CalculateSNR(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
                PSNR = SimilarityFunctions.CalculatePNSR(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
                MD = SimilarityFunctions.CalculateMD(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
                ENOB = SimilarityFunctions.CalculateENOB(SelectedSignal1.PointsY, SelectedSignal2.PointsY);
            }

        }
    }
}
