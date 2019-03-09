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
        public ImageSource ChartSource { get; set; }
        public ImageSource HistogramSource { get; set; }

        public bool HasData { get; set; }

    }
}
