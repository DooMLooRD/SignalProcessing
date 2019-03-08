using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SignalProcessingView.ViewModel.Base;

namespace SignalProcessingView.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<TabViewModel> _tabs;
        private double _a;
        private double _t1;
        private double _d;
        private double _t;
        private double _kw;
        private string _selectedSignalType;
        private string _selectedTab;

        #region Properties

        public List<string> SignalTypes { get; set; }

        public string SelectedSignalType
        {
            get => _selectedSignalType;
            set
            {
                _selectedSignalType = value;
                OnPropertyChanged(nameof(SelectedSignalType));
            }
        }

        public ObservableCollection<TabViewModel> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                OnPropertyChanged(nameof(Tabs));
            }
        }
        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
        public double A
        {
            get => _a;
            set
            {
                _a = value;
                OnPropertyChanged(nameof(A));
            }
        }

        public double T1
        {
            get => _t1;
            set
            {
                _t1 = value;
                OnPropertyChanged(nameof(T1));
            }
        }

        public double D
        {
            get => _d;
            set
            {
                _d = value;
                OnPropertyChanged(nameof(D));
            }
        }

        public double T
        {
            get => _t;
            set
            {
                _t = value;
                OnPropertyChanged(nameof(T));
            }
        }

        public double Kw
        {
            get => _kw;
            set
            {
                _kw = value;
                OnPropertyChanged(nameof(Kw));
            }
        }

        public ICommand AddPageCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>(){new TabViewModel("Tab0")};
            SelectedTab = "Tab0";
            SignalTypes=new List<string>()
            {
                "Szum o rozkładzie jednostajnym",
                "Szum Gaussowski",
                "Sygnał sinusoidalny",
                "Sygnał sinusoidalny wyprostowany jednopołówkowo",
                "Sygnał sinusoidalny wyprostowany dwupołówkowo",
                "Sygnał prostokątny",
                "Sygnał prostokątny symetryczny",
                "Sygnał trójkątny",
                "Skok jednostkowy",
                "Impuls jednostkowy",
                "Szum impulsowy"
            };
            
            SelectedSignalType = "Szum o rozkładzie jednostajnym";
            AddPageCommand = new RelayCommand(AddPage);
        }

        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Tab" + Tabs.Count));
        }
    }
}
