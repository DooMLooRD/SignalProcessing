using SignalProcessingCore.Signal;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad4.Fourier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Zad4
{
    public class Zad4ViewModel : BaseViewModel
    {
        public SignalCreator SignalCreator { get; set; }

        public SampledSignal SelectedFourierSignal { get; set; }
        public SampledSignal SelectedReverseFourierSignal { get; set; }

        public List<string> FourierTransforms { get; set; }
        public string SelectedFourierTransform { get; set; }
        public string SelectedReverseFourierTransform { get; set; }


        public ICommand FourierTransformCommand { get; set; }
        public ICommand ReverseFourierTransformCommand { get; set; }



        public Zad4ViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;

            FourierTransforms = new List<string>()
            {
                "(F1.1) Transformacja Fouriera",
                "(F1.2) Szybka Transformacja Fouriera",
            };
            SelectedFourierTransform = FourierTransforms[0];
            SelectedReverseFourierTransform = FourierTransforms[0];
            FourierTransformCommand = new RelayCommand(FourierTransformMethod);
            ReverseFourierTransformCommand = new RelayCommand(ReverseFourierTransformMethod);
        }

        private void ReverseFourierTransformMethod()
        {
            if (SelectedReverseFourierSignal != null)
            {
                SampledSignal signal = new SampledSignal();
                signal.ComplexPoints = SelectedReverseFourierSignal.ComplexPoints;
                signal.Name = $"{SelectedReverseFourierSignal.Name} Reverse-Transform";
                switch (SelectedReverseFourierTransform.Substring(1, 4))
                {
                    case "F1.1":
                        signal.PointsY = FourierTransform.ReverseTransform(signal.ComplexPoints);
                        break;
                    case "F1.2":
                        FastFourierTransform fourierTransform = new FastFourierTransform();
                        signal.PointsY = fourierTransform.ReverseTransform(signal.ComplexPoints);
                        break;
                }
                SignalCreator.AddSignal(signal);
            }
        }

        private void FourierTransformMethod()
        {
            if (SelectedFourierSignal != null)
            {
                SampledSignal signal = new SampledSignal();
                signal.PointsY = SelectedFourierSignal.PointsY;
                signal.Name = $"{SelectedFourierSignal.Name} {SelectedFourierTransform}";
                switch (SelectedFourierTransform.Substring(1, 4))
                {
                    case "F1.1":
                        signal.ComplexPoints = FourierTransform.Transform(signal.PointsY);
                        break;
                    case "F1.2":
                        FastFourierTransform fourierTransform = new FastFourierTransform();
                        signal.ComplexPoints = fourierTransform.Transform(signal.PointsY);
                        break;
                }
                SignalCreator.AddSignal(signal);
            }
        }

    }
}
