using application.UseCases;
using booking.Commands;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Guide;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WPF.ViewModels
{
    public class ParametarOfStatisticsForTourCreationVIewModel:BaseViewModel,INotifyPropertyChanged
    {
        public List<string> Parameters { get; set; }
        private string selectedParameter;

        public string SelectedParameter
        {
            get { return selectedParameter; }
            set
            {
                selectedParameter = value;
                OnPropertyChanged(nameof(SelectedParameter));
            }
        }
        public bool DemoOn { get; set; }
        public static string Parameter;
        public static bool Accept;
        public ICommand CreateCommand => new RelayCommand(CreateTour);
        private SimpleRequestService _simpleRequestService;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ParametarOfStatisticsForTourCreationVIewModel(bool demoOn)
        {
            Parameters= new List<string>() { "State","City","Language","All"};
            _simpleRequestService = new SimpleRequestService();
            Accept = false;
            DemoOn = demoOn;
            if (DemoOn)
            {
                DemoIsOn(new CancellationToken());
            }
        }
        private void CreateTour()
        {
            Parameter=_simpleRequestService.CreateTourWithHelpOfStatistics(SelectedParameter);
            Accept = true;
            CloseCurrentWindow();
        }

        private async Task DemoIsOn(CancellationToken ct)
        {


            ct.ThrowIfCancellationRequested();
            //MessageBox.Show("Demo has started!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Information);
            await Task.Delay(2000, ct);
            SelectedParameter = "All";
            await Task.Delay(2000, ct);

            CreateTour();
            await Task.Delay(2000, ct);
            FinishedDemoWindow fdw = new FinishedDemoWindow();
            fdw.Show();

            await Task.Delay(2000, ct);
            fdw.Close();
        }
    }
}
