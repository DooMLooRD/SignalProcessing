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
        public double D { get; set; }
        public double T { get; set; }
        public double Kw { get; set; }
        #endregion




        public ICommand AddPageCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>(){new TabViewModel("Tab0")};
            SelectedTab = Tabs[0];
            SelectedSignal1Tab = Tabs[0];
            SelectedSignal2Tab = Tabs[0];
            SelectedResultTab = Tabs[0];

            SignalTypes=new List<string>()
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

            Operations=new List<string>()
            {
                "(D1) Dodawanie",
                "(D2) Odejmowanie",
                "(D3) Mnożenie",
                "(D4) Dzielenie",
            };
            SelectedOperation = Operations[0];
            AddPageCommand = new RelayCommand(AddPage);
        }

        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Tab" + Tabs.Count));
        }
    }
}
