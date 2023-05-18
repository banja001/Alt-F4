using application.UseCases;
using booking.Model;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WPF.ViewModels
{
    class TourRequestAcceptanceViewModel
    {
        public User Guide { get; set; }   
        public ObservableCollection<SimpleAndComplexTourRequestsDTO> AllRequests { get; set; }
        private SimpleRequestService _simpleRequestsService { get; set; }
        public SimpleAndComplexTourRequestsDTO SelectedTourRequest { get; set; }
        public ObservableCollection<string> States { get; set; }
        private LocationService _locationService { get; set; }
        public TourRequestAcceptanceViewModel(User guide) 
        {
            Guide = guide;
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>();
            _simpleRequestsService =new SimpleRequestService();
            _locationService =new LocationService();
            States = new ObservableCollection<string>( _locationService.InitializeListOfStates());
            LoadRequests();
        }
        private void LoadRequests()
        {
            AllRequests.Clear();
            foreach (var request in _simpleRequestsService.CreateListOfSimpleRequests()) 
            {
                AllRequests.Add(request);
            }
        }
    }
}
