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

        public Guest1ViewViewModel(int userId)
        {
            _userService = new UserService();
            _reservedDatesService = new ReservedDatesService();

            this.userId = userId;

            CountReservations();
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
            UserName = User.Username;

            if (User.Super)
            {
                if ((DateTime.Now - User.DateOfBecomingSuper).Days < 365)
                    Score = User.Score.ToString();
                else
                {
                    if (User.NumOfAccommodationReservations >= 10)
                    {
                        User.NumOfAccommodationReservations = 0;
                        User.Score = 5;
                        User.DateOfBecomingSuper = User.DateOfBecomingSuper.AddDays(365);
                    }
                    else
                    {
                        User.Score = 0;
                        User.Super = false;
                        User.DateOfBecomingSuper = new DateTime(0001, 01, 01);
                    }
                }
            }
            else
            {
                if (User.NumOfAccommodationReservations >= 10)
                {
                    User.NumOfAccommodationReservations = 0;
                    User.Score = 5;
                    User.DateOfBecomingSuper = DateTime.Now;
                    User.Super = true;

                    MessageBox.Show("Congratulations! You have made 10 reservations and now have become a super guest.\n\n" +
                        "You will be granted 5 points to spend on your next 5 reservations during the next year(starting today) " +
                        "which will reduce the price of reservation.", "Congratulations");
                }
            }

            _userService.Update(User);

            Score = User.Score.ToString();
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
    }
}
