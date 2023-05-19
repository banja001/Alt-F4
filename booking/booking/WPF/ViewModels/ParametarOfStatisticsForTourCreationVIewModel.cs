using application.UseCases;
using booking.Commands;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels
{
    public class ParametarOfStatisticsForTourCreationVIewModel:BaseViewModel
    {
        public List<string> Parameters { get; set; }
        public string SelectedParameter { get; set; }
        public static string Parameter;
        public static bool Accept;
        public ICommand CreateCommand => new RelayCommand(CreateTour);
        private SimpleRequestService _simpleRequestService;
        public ParametarOfStatisticsForTourCreationVIewModel()
        {
            Parameters= new List<string>() { "State","City","Language","All"};
            _simpleRequestService = new SimpleRequestService();
            Accept = false;
        }
        private void CreateTour()
        {
            Parameter=_simpleRequestService.CreateTourWithHelpOfStatistics(SelectedParameter);
            Accept = true;
            CloseCurrentWindow();
        }
    }
}
