using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.Model;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels
{
    public class AddComplexRequestViewModel : BaseViewModel
    {
        private string _complexRequestName;
        public string ComplexRequestName 
        { 
            get { return _complexRequestName; }
            set { _complexRequestName = value; OnPropertyChanged(nameof(_complexRequestName)); }
        }  
        private readonly ComplexRequestService _complexRequestService;
        private readonly SimpleRequestService _simpleRequestService;
        private User _user;
        private ComplexRequest _complexRequest;
        public ICommand SubmitComplexRequest => new RelayCommand(OnSubmitComplexRequest);
        public ICommand ExitButtonCommand => new RelayCommand(OnExitButton);
        public AddComplexRequestViewModel() { } 
        public AddComplexRequestViewModel(User user, ComplexRequest complexRequest) 
        {
            _complexRequest = complexRequest;
            _user = user;
            _complexRequestService = new ComplexRequestService();
            _simpleRequestService = new SimpleRequestService();
        }
        private void OnExitButton()
        {
            this.CloseCurrentWindow();
        }
        private void OnSubmitComplexRequest()
        {
            if (_complexRequestName.IsNullOrWhiteSpace())
            {
                MessageBox.Show("You need to enter complex tour name!", "Error");
                return;
            }
            if(_complexRequest.SimpleRequests.Count < 2)
            {
                MessageBox.Show("Complex request needs to have at least 2 simple requests!", "Error");
                return;
            }

            _complexRequest.Name = ComplexRequestName;
            foreach (var simpleRequest in _complexRequest.SimpleRequests)
            {
                simpleRequest.IsPartOfComplex = true;
                _simpleRequestService.Add(simpleRequest);
            }
            _complexRequestService.Add(_complexRequest);
            MessageBox.Show("Successfully added complex tour with name " + ComplexRequestName + "!", "Success");
            this.CloseCurrentWindow();
        }
    }
}
