using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using booking.WPF.Views.Guide;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    class GuideMainViewModel:BaseViewModel, INotifyPropertyChanged
    {
        private Brush addTourButton;
        public Brush AddTourButton
        {
            get { return addTourButton; }
            set
            {
                addTourButton = value;
                OnPropertyChanged(nameof(AddTourButton));
            }
        }
        private Brush liveTrackButton;
        public Brush LiveTrackButton
        {
            get { return liveTrackButton; }
            set
            {
                liveTrackButton = value;
                OnPropertyChanged(nameof(LiveTrackButton));
            }
        }
        private Brush upcomingButton;
        public Brush UpcomingButton
        {
            get { return upcomingButton; }
            set
            {
                upcomingButton = value;
                OnPropertyChanged(nameof(UpcomingButton));
            }
        }
        private Brush finishedButton;
        public Brush FinishedButton
        {
            get { return finishedButton; }
            set
            {
                finishedButton = value;
                OnPropertyChanged(nameof(FinishedButton));
            }
        }
        private Brush requestButton;
        public Brush RequestButton
        {
            get { return requestButton; }
            set
            {
                requestButton = value;
                OnPropertyChanged(nameof(RequestButton));
            }
        }
        private Brush statisticsButton;
        public Brush StatisticsButton
        {
            get { return statisticsButton; }
            set
            {
                statisticsButton = value;
                OnPropertyChanged(nameof(StatisticsButton));
            }
        }

        private Brush profileButton;
        public Brush ProfileButton
        {
            get { return profileButton; }
            set
            {
                profileButton = value;
                OnPropertyChanged(nameof(ProfileButton));
            }
        }

        public Brush ClickedButton{ get; set; }
        public Brush DefaultButton { get; set; }
        public User Guide { get; set; }
        public Frame Content { get; set; }
        public ICommand AddTourCommand => new RelayCommand(AddTourWindowOpen);

        public ICommand LiveTrackCommand => new RelayCommand(LiveTrackingWindowOpen);
        public ICommand UpcomingToursCommand => new RelayCommand( TourCancellationOpen);
        public ICommand FinishedToursCommand => new RelayCommand(FinishedToursWindowOpen);
        public ICommand ProfilePageCommand => new RelayCommand(ProfilePageOpen);
        public ICommand TourRequestsCommand => new RelayCommand(TourRequestsOpen);
        public ICommand TourRequestsStatisticsCommand => new RelayCommand(TourRequestsStatisticsOpen);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuideMainViewModel(User guide, Frame content)
        {
            Guide = guide;
            Content=content;
            ClickedButton = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA"));
            DefaultButton = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4FAD6"));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = ClickedButton;
        }

        private void AddTourWindowOpen()
        {
            Content.NavigationService.Navigate(new AddTourWindow(Guide));
            AddTourButton = ClickedButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = DefaultButton;
        }
        private void LiveTrackingWindowOpen()
        {
            Content.NavigationService.Navigate(new LiveTrackingWindow(Guide));
            AddTourButton = DefaultButton;
            LiveTrackButton = ClickedButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = DefaultButton;
        }
        private void FinishedToursWindowOpen()
        {
            Content.NavigationService.Navigate(new ShowReviews(Guide));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = ClickedButton ;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = DefaultButton;
        }
        private void TourCancellationOpen()
        {
            Content.NavigationService.Navigate(new TourCancellation(Guide));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = ClickedButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = DefaultButton;
        }
        private void ProfilePageOpen()
        {
            Content.NavigationService.Navigate(new ProfilePage(Guide));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = DefaultButton;
            ProfileButton = ClickedButton;
        }

        private void TourRequestsOpen()
        {
            Content.NavigationService.Navigate(new TourRequestAcceptancePage(Guide, Content.NavigationService));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = ClickedButton;
            StatisticsButton = DefaultButton;
            ProfileButton = DefaultButton;
        }

        private void TourRequestsStatisticsOpen()
        {
            Content.NavigationService.Navigate(new TourRequestsStatisticsPage(Guide, Content.NavigationService));
            AddTourButton = DefaultButton;
            LiveTrackButton = DefaultButton;
            UpcomingButton = DefaultButton;
            FinishedButton = DefaultButton;
            RequestButton = DefaultButton;
            StatisticsButton = ClickedButton;
            ProfileButton = DefaultButton;
        }
    }
}
