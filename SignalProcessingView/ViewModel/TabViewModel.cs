using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class TabViewModel : BaseViewModel
    {
        private string _header;
        private TabContentViewModel _tabContent;

        public string Header
        {
            get => _header;
            set
            {
                _header = value; 
                OnPropertyChanged(nameof(Header));
            }
        }
        public TabContentViewModel TabContent
        {
            get => _tabContent;
            set
            {
                _tabContent = value; 
                OnPropertyChanged(nameof(TabContent));
            }
        }

        public TabViewModel(string header)
        {
            Header = header;
            TabContent = new TabContentViewModel();
        }

        public override string ToString()
        {
            return Header;
        }
    }
}
