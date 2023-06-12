using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View.Guest1;
using booking.WPF.ViewModels;
using Domain.Model;
using Egor92.MvvmNavigation.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using WPF.Views.Guest1;
using static System.Formats.Asn1.AsnWriter;

namespace WPF.ViewModels.Guest1
{
    public class Guest1ViewViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private int selectedIndexTabControl;
        public int SelectedIndexTabControl
        {
            get { return selectedIndexTabControl; }
            set
            {
                if (selectedIndexTabControl != value)
                {
                    selectedIndexTabControl = value;
                    OnPropertyChanged();
                }
            }
        }

        private int userId;
        public string UserName { get; set; }

        private string score;
        public string Score 
        {
            get { return score; }
            set
            {
                if(score != value)
                {
                    score = value;
                    OnPropertyChanged(nameof(Score));
                }
            } 
        }
        public User User { get; set; }

        private readonly UserService _userService;
        private readonly ReservedDatesService _reservedDatesService;

        public ICommand OpenFirstTabCommand => new RelayCommand(OpenFirstTab);
        public ICommand OpenSecondTabCommand => new RelayCommand(OpenSecondTab);
        public ICommand OpenThirdTabCommand => new RelayCommand(OpenThirdTab);
        public ICommand OpenFourthTabCommand => new RelayCommand(OpenFourthTab);
        public ICommand TutorialCommand => new RelayCommand(Tutorial);

        public Guest1ViewViewModel(int userId)
        {
            _userService = new UserService();
            _reservedDatesService = new ReservedDatesService();

            this.userId = userId;

            InitializeGuestStatus();
            KeepTrackOfScore();

            SelectedIndexTabControl = 0;
        }

        private void KeepTrackOfScore()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tick_event;
            timer.Start();
        }

        private void tick_event(object sender, EventArgs e)
        {
            User = _userService.GetById(userId);
            Score = User.Score.ToString();
        }

        private void CountReservations()
        {
            User = _userService.GetById(userId);

            List<ReservedDates> reservedDates = _reservedDatesService.GetByGuestId(userId).Where(d => d.EndDate <= DateTime.Now 
                                                                                        && d.EndDate > User.DateOfBecomingSuper).ToList();

            User.NumOfAccommodationReservations = reservedDates.Count;

            _userService.Update(User);
        }
        private void InitializeGuestStatus()
        {
            CountReservations();

            UserName = User.Username;

            if (User.Super)
            {
                if ((DateTime.Now - User.DateOfBecomingSuper).Days >= 365)
                {
                    if (User.NumOfAccommodationReservations >= 10)
                    {
                        UpdateGuestsFields(User.Super, User.DateOfBecomingSuper.AddDays(365), 5, 0);
                    }
                    else
                    {
                        UpdateGuestsFields(false, new DateTime(0001, 01, 01), 0, User.NumOfAccommodationReservations);
                    }
                }
            }
            else
            {
                if (User.NumOfAccommodationReservations >= 10)
                {
                    UpdateGuestsFields(true, DateTime.Now, 5, 0);

                    MessageBox.Show("Congratulations! You have made 10 reservations and now have become a super guest.\n\n" +
                        "You will be granted 5 points to spend on your next 5 reservations during the next year(starting today) " +
                        "which will reduce the price of reservation.", "Congratulations");
                }
            }

            _userService.Update(User);

            Score = User.Score.ToString();
        }

        private void UpdateGuestsFields(bool super, DateTime dateOfBecomingSuper, int score, int numOfAcommodationreservations)
        {
            User.Super = super;
            User.DateOfBecomingSuper = dateOfBecomingSuper;
            User.Score = score;
            User.NumOfAccommodationReservations = numOfAcommodationreservations;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenFirstTab()
        {
            SelectedIndexTabControl = 0;
        }

        private void OpenSecondTab()
        {
            SelectedIndexTabControl = 1;
        }

        private void OpenThirdTab()
        {
            SelectedIndexTabControl = 2;
        }

        private void OpenFourthTab()
        {
            SelectedIndexTabControl = 3;
        }

        private void Tutorial()
        {
            TutorialView window;
            if (SelectedIndexTabControl == 0)
                window = new TutorialView("search");
            else if(SelectedIndexTabControl == 1)
                window = new TutorialView("rate");
            else if(selectedIndexTabControl == 2)
                window = new TutorialView("reservations");
            else
                window = new TutorialView("forums");
            window.ShowDialog();
        }
    }
}
