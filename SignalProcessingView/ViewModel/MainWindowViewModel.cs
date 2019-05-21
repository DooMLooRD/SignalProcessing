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
using SignalProcessingView.View;
using SignalProcessingView.ViewModel.Base;
using SignalProcessingView.ViewModel.Zad1;
using SignalProcessingView.ViewModel.Zad2;
using SignalProcessingView.ViewModel.Zad3;
using SignalProcessingView.ViewModel.Zad4;
using SignalProcessingZad2;

namespace SignalProcessingView.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        public SignalCreator SignalCreator { get; set; }
        public Zad1ViewModel Zad1ViewModel { get; set; }
        public Zad2ViewModel Zad2ViewModel { get; set; }
        public Zad3ViewModel Zad3ViewModel { get; set; }
        public Zad4ViewModel Zad4ViewModel { get; set; }
        public Zad3ExperimentViewModel Zad3ExperimentViewModel { get; set; }
        public ICommand AddChartWindow { get; set; }
        public ICommand ToggleBaseCommand { get; set; }

        public MainWindowViewModel()
        {
            SignalCreator = new SignalCreator();
            Zad1ViewModel = new Zad1ViewModel(SignalCreator);
            Zad2ViewModel = new Zad2ViewModel(SignalCreator);
            Zad3ViewModel = new Zad3ViewModel(SignalCreator);
            Zad4ViewModel = new Zad4ViewModel(SignalCreator);
            Zad3ExperimentViewModel = new Zad3ExperimentViewModel(SignalCreator);
            AddChartWindow = new RelayCommand(AddWindow);
            ToggleBaseCommand = new RelayCommand<bool>(ApplyBase);
        }
        public void AddWindow()
        {
            ChartWindow window = new ChartWindow() { DataContext = new ChartWindowViewModel(SignalCreator) };
            window.Show();
        }
        private static void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
        }

    }
}
