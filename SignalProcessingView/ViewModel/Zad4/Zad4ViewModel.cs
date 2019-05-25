using SignalProcessingCore.Signal;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingZad4.Fourier;
using SignalProcessingZad4.Wavelet_Transform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Zad4
{
    public class Zad4ViewModel : BaseViewModel
    {
        public SignalCreator SignalCreator { get; set; }

        public SampledSignal SelectedSignal { get; set; }
        public SampledSignal SelectedReverseSignal { get; set; }

        public List<string> Transforms { get; set; }
        public string SelectedTransform { get; set; }
        public string SelectedReverseTransform { get; set; }
        public double Time { get; set; }
        public double TimeReverse { get; set; }

        public ICommand TransformCommand { get; set; }
        public ICommand ReverseTransformCommand { get; set; }



        public Zad4ViewModel(SignalCreator signalCreator)
        {
            SignalCreator = signalCreator;

            Transforms = new List<string>()
            {
                "(F1.1) Transformacja Fouriera",
                "(F1.2) Szybka Transformacja Fouriera",
                "(F1.3) Transformacja Falkowa",
            };
            SelectedTransform = Transforms[0];
            SelectedReverseTransform = Transforms[0];
            TransformCommand = new RelayCommand(TransformMethod);
            ReverseTransformCommand = new RelayCommand(ReverseTransformMethod);
        }

        private void ReverseTransformMethod()
        {
            if (SelectedReverseSignal != null)
            {
                SampledSignal signal = new SampledSignal();
                signal.ComplexPoints = SelectedReverseSignal.ComplexPoints;
                signal.Name = $"{SelectedReverseSignal.Name} Reverse-Transform";
                Stopwatch timer = new Stopwatch();
                timer.Start();
                switch (SelectedReverseTransform.Substring(1, 4))
                {
                    case "F1.1":
                        signal.PointsY = FourierTransform.ReverseTransform(signal.ComplexPoints);
                        break;
                    case "F1.2":
                        FastFourierTransform fourierTransform = new FastFourierTransform();
                        signal.PointsY = fourierTransform.ReverseTransform(signal.ComplexPoints);
                        break;
                    case "F1.3":
                        signal.PointsY = WaveletTransform.ReverseTransform(signal.ComplexPoints);
                        break;
                }
                timer.Stop();
                TimeReverse = timer.ElapsedMilliseconds;
                SignalCreator.AddSignal(signal);
            }
        }

        private void TransformMethod()
        {
            if (SelectedSignal != null)
            {
                SampledSignal signal = new SampledSignal();
                signal.PointsY = SelectedSignal.PointsY;
                signal.Name = $"{SelectedSignal.Name} {SelectedTransform}";
                Stopwatch timer = new Stopwatch();
                timer.Start();
                switch (SelectedTransform.Substring(1, 4))
                {
                    case "F1.1":
                        signal.ComplexPoints = FourierTransform.Transform(signal.PointsY);
                        break;
                    case "F1.2":
                        FastFourierTransform fourierTransform = new FastFourierTransform();
                        signal.ComplexPoints = fourierTransform.Transform(signal.PointsY);
                        break;
                    case "F1.3":
                        signal.ComplexPoints = WaveletTransform.Transform(signal.PointsY);
                        break;
                }
                timer.Stop();
                Time = timer.ElapsedMilliseconds;
                SignalCreator.AddSignal(signal);
            }
        }

    }
}
