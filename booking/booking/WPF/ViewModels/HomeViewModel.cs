using application.UseCases;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace booking.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<SimpleRequestTourDTO> SuggestionNotifications { get; set; }
        public ObservableCollection<SimpleRequestTourDTO> ApprovedNotifications { get; set; }
        private SimpleRequestService _simpleRequestService;
        public HomeViewModel(User user)
        {
            _simpleRequestService = new SimpleRequestService();
            SuggestionNotifications = new ObservableCollection<SimpleRequestTourDTO>(_simpleRequestService.CreateSuggestionNotificationsByGuest2(user));
            ApprovedNotifications = new ObservableCollection<SimpleRequestTourDTO>(_simpleRequestService.CreateApprovedNotificationsByGuest2(user));
        }
    }
}
