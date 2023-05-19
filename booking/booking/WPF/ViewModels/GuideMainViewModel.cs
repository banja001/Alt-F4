using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using booking.WPF.Views.Guide;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    class GuideMainViewModel:BaseViewModel
    {
        public User Guide { get; set; }
        public Frame Content { get; set; }
        public ICommand AddTourCommand => new RelayCommand(AddTourWindowOpen);

        public ICommand LiveTrackCommand => new RelayCommand(LiveTrackingWindowOpen);
        public ICommand UpcomingToursCommand => new RelayCommand( TourCancellationOpen);
        public ICommand FinishedToursCommand => new RelayCommand(UnfinishedToursWindowOpen);
        public ICommand ProfilePageCommand => new RelayCommand(ProfilePageOpen);
        public ICommand TourRequestsCommand => new RelayCommand(TourRequestsOpen);
        public ICommand TourRequestsStatisticsCommand => new RelayCommand(TourRequestsStatisticsOpen);

        public GuideMainViewModel(User guide, Frame content)
        {
            Guide = guide;
            Content=content;
        }

        private void AddTourWindowOpen()
        {
            Content.NavigationService.Navigate(new AddTourWindow(Guide));
        }
        private void LiveTrackingWindowOpen()
        {
            Content.NavigationService.Navigate(new LiveTrackingWindow(Guide));
        }
        private void UnfinishedToursWindowOpen()
        {
            Content.NavigationService.Navigate(new ShowReviews(Guide));
        }
        private void TourCancellationOpen()
        {
            Content.NavigationService.Navigate(new TourCancellation(Guide));
        }
        private void ProfilePageOpen()
        {
            Content.NavigationService.Navigate(new ProfilePage(Guide));
        }

        private void TourRequestsOpen()
        {
            Content.NavigationService.Navigate(new TourRequestAcceptancePage(Guide, Content.NavigationService));
        }

        private void TourRequestsStatisticsOpen()
        {
            Content.NavigationService.Navigate(new TourRequestsStatisticsPage(Guide));
        }
    }
}
