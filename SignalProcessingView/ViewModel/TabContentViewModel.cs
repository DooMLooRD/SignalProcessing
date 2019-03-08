using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class TabContentViewModel : BaseViewModel
    {
        private ImageSource _chartSource;
        private ImageSource _histogramSource;

        public ImageSource ChartSource
        {
            get => _chartSource;
            set
            {
                _chartSource = value;
                OnPropertyChanged(nameof(ChartSource));
            }
        }

        public ImageSource HistogramSource
        {
            get => _histogramSource;
            set
            {
                _histogramSource = value;
                OnPropertyChanged(nameof(HistogramSource));
            }
        }

    }
}
