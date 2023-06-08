using application.UseCases;
using booking.application.UseCases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class ReserveAccommodationViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public ReservedDates NewDate { get; set; }
        public int NumOfDays { get; set; }
        public static ObservableCollection<ReservedDates> FreeDates { get; set; }
        public static ReservedDates SelectedDates { get; set; }

        private AccommodationLocationDTO selectedAccommodation;
        public List<ReservedDates> ReservedDates { get; set; }
        public int GuestsNumber { get; set; }

        private Visibility alternativeatesVisibility;
        public Visibility AlternativeDatesVisibility 
        {
            get { return alternativeatesVisibility; }
            set
            {
                if(alternativeatesVisibility != value)
                {
                    alternativeatesVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool searchButtonEnabled;
        public bool SearchButtonEnabled 
        {
            get { return searchButtonEnabled; }
            set
            {
                if(searchButtonEnabled != value)
                {
                    searchButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool reserveButtonEnabled;
        public bool ReserveButtonEnabled
        {
            get { return reserveButtonEnabled; }
            set
            {
                if (reserveButtonEnabled != value)
                {
                    reserveButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;
        public RenovationDatesService _renovationService { get; set; }
        private readonly UserService _userService;

        public ICommand ReserveAccommodationClickCommand => new RelayCommand(ReserveAccommodationClick);
        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand SelectedDateChangedCommand => new RelayCommand(SelectedDateChanged);
        public ICommand SearchFreeDatesCommand => new RelayCommand(SearchFreeDates);
        public ICommand NumValueChangedCommand => new RelayCommand(NumValueChanged);
        public ICommand FreeDateSelectionChangedCommand => new RelayCommand(FreeDateSelectionChanged);
        public ICommand GuestsNumValueChangedCommand => new RelayCommand(GuestsNumValueChanged);

        private int userId;
        public User User { get; set; }
        public ReserveAccommodationViewModel(int userId)
        {
            _renovationService = new RenovationDatesService();
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();
            _userService = new UserService();

            this.userId = userId;
            User = _userService.GetById(userId);
            selectedAccommodation = OverviewViewModel.SelectedAccommodation;
            AlternativeDatesVisibility = Visibility.Hidden;

            NewDate = new ReservedDates(DateTime.Now, DateTime.Now, selectedAccommodation.Id);

            FreeDates = new ObservableCollection<ReservedDates>();
            ReservedDates = _reservedDatesService.GetAllByAccommodationId(selectedAccommodation.Id);
        }

        private void FilterReservedDatesByMonth()
        {
            ReservedDates = ReservedDates.OrderBy(d => d.StartDate).ToList();
            ReservedDates = ReservedDates.Where(d => d.StartDate.Month == NewDate.StartDate.Month || d.StartDate.Month == NewDate.EndDate.Month
                                            || d.EndDate.Month == NewDate.EndDate.Month || d.EndDate.Month == NewDate.StartDate.Month).ToList();
        }

        private void ReserveAccommodationClick()
        {
            if (ReserveButtonEnabled)
            {
                if (SelectedDates == null)
                {
                    MessageBox.Show("You have to pick a date before making a reservation!", "Warning");
                    return;
                }

                int maxCapacity = _accommodationService.FindById(SelectedDates.AccommodationId).MaxCapacity;

                if (GuestsNumber > maxCapacity)
                {
                    MessageBox.Show("Max guest capacity for this accommodation is " + maxCapacity);
                    return;
                }

                SetSelectedDatesParameters();
                _reservedDatesService.Add(SelectedDates);

                DecreaseScoreOfSuper();

                MessageBox.Show("Your reservation has been successfully made!");

                this.CloseCurrentWindow();
            }
        }

        private void DecreaseScoreOfSuper()
        {
            if (User.Score > 0 && User.Super)
            {
                --User.Score;
                _userService.Update(User);
            }
        }

        private void SetSelectedDatesParameters()
        {
            SelectedDates.Id = _reservedDatesService.MakeId();
            SelectedDates.NumOfGuests = GuestsNumber;
            SelectedDates.UserId = userId;
            SelectedDates.DateOfReserving = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchFreeDates()
        {
            if (SearchButtonEnabled)
            {
                if (NumOfDays < selectedAccommodation.MinDaysToUse)
                {
                    MessageBox.Show("You can reserve this accommodation for at least " + selectedAccommodation.MinDaysToUse + " days");
                    return;
                }

                FilterReservedDatesByMonth();
                if (NumOfDays > (NewDate.EndDate - NewDate.StartDate).Days)
                {
                    OfferAlternativeDates();
                }
                else
                {
                    FreeDates.Clear();
                    AlternativeDatesVisibility = Visibility.Hidden;

                    CreateDateIntervals(NewDate.StartDate, NewDate.EndDate);
                    RemoveReservedDatesFromIntervals();

                    if (FreeDates.Count == 0)
                        OfferAlternativeDates();
                }

                //brise iz FreeDates sve datume koji se poklapaju sa renoviranjem
                foreach(ReservedDates reservation in FreeDates.ToList())
                {
                    foreach(RenovationDates renovation in _renovationService.GetAll())
                    {
                        if(!(reservation.StartDate>=renovation.EndDate || reservation.EndDate <= renovation.StartDate) && reservation.AccommodationId==renovation.AccommodationId)
                        {
                            FreeDates.Remove(reservation);
                        }
                    }
                }


            }
        }

        private void CreateDateIntervals(DateTime startDate, DateTime endDate)
        {
            while ((endDate - startDate).Days >= NumOfDays - 1)
            {
                FreeDates.Add(new ReservedDates(startDate, startDate.AddDays(NumOfDays - 1), selectedAccommodation.Id));

                startDate = startDate.AddDays(1);
            }
        }

        private void RemoveReservedDatesFromIntervals()
        {
            foreach (var date in ReservedDates)
            {
                if (FreeDates.Count == 0)
                    return;

                if (date.EndDate < FreeDates[0].StartDate || date.StartDate > FreeDates[FreeDates.Count - 1].EndDate)
                    continue;

                DateTime firstDate = GetStartOfReservedDate(date).AddDays(-NumOfDays + 2);
                DateTime lastDate = GetEndOfReservedDate(date);

                while (firstDate < lastDate)
                {
                    if (FreeDates.Where(d => d.StartDate == firstDate).Count() != 0)
                        FreeDates.Remove(FreeDates.Where(d => d.StartDate == firstDate).ToList()[0]);

                    firstDate = firstDate.AddDays(1);
                }
            }
        }

        private static DateTime GetEndOfReservedDate(ReservedDates date)
        {
            List<ReservedDates> firstDates = FreeDates.Where(d => d.EndDate == date.EndDate).ToList();

            return firstDates.Count() == 0
                ? new DateTime(FreeDates[FreeDates.Count - 1].EndDate.Year, FreeDates[FreeDates.Count - 1].EndDate.Month, FreeDates[FreeDates.Count - 1].EndDate.Day)
                : firstDates[0].EndDate;
        }

        private static DateTime GetStartOfReservedDate(ReservedDates date)
        {
            List<ReservedDates> lastDates = FreeDates.Where(d => d.StartDate == date.StartDate).ToList();

            return lastDates.Count() == 0
                ? new DateTime(FreeDates[0].StartDate.Year, FreeDates[0].StartDate.Month, FreeDates[0].StartDate.Day)
                : lastDates[0].StartDate;
        }

        private void OfferAlternativeDates()
        {
            AlternativeDatesVisibility = Visibility.Visible;

            DateTime startDate = new DateTime(NewDate.StartDate.Year, NewDate.StartDate.Month, 1);
            DateTime endDate = new DateTime(NewDate.EndDate.Year, NewDate.EndDate.Month, DateTime.DaysInMonth(NewDate.EndDate.Year, NewDate.EndDate.Month));

            CreateDateIntervals(startDate, endDate);
            RemoveReservedDatesFromIntervals();
            PickFourClosestDates();
        }

        private void PickFourClosestDates()
        {
            ReservedDates closestDateBefore = FreeDates.MinBy(d => Math.Abs((NewDate.StartDate - d.EndDate).Days));
            ReservedDates closestDateAfter = FreeDates.MinBy(d => Math.Abs((d.StartDate - NewDate.EndDate).Days));

            int closestDateBeforeIndx = FreeDates.IndexOf(closestDateBefore);
            int closestDateAfterIndx = FreeDates.IndexOf(closestDateAfter);

            ReservedDates secondClosestBefore = FreeDates[--closestDateBeforeIndx];
            ReservedDates secondClosestAfter = FreeDates[++closestDateAfterIndx];

            AddClosestDatesToList(closestDateBefore, closestDateAfter, secondClosestBefore, secondClosestAfter);
        }

        private void AddClosestDatesToList(ReservedDates closestDateBefore, ReservedDates closestDateAfter, ReservedDates secondClosestBefore, ReservedDates secondClosestAfter)
        {
            while(FreeDates.Count > 0)
            {
                FreeDates.RemoveAt(0);
            }

            FreeDates.Add(secondClosestBefore);
            FreeDates.Add(closestDateBefore);
            FreeDates.Add(closestDateAfter);
            FreeDates.Add(secondClosestAfter);
        }

        private void SelectedDateChanged()
        {
            SearchButtonEnabled = NewDate.IsValid && IsValid;
        }

        private void NumValueChanged()
        {
            SearchButtonEnabled = NewDate.IsValid && IsValid;
        }

        private void FreeDateSelectionChanged()
        {
            if(GuestsNumber > 0 && GuestsNumber <= 10)
                ReserveButtonEnabled = (SelectedDates != null) ? true : false;
        }

        private void GuestsNumValueChanged()
        {
            ReserveButtonEnabled = SelectedDates != null && (GuestsNumber > 0 && GuestsNumber <= 10);
        }

        private void CloseWindow()
        {
            this.CloseCurrentWindow();
        }

        public override string ToString()
        {
            return $"{NumOfDays}";
        }

        public string Error => null;

        private Regex _numOfDays = new Regex("^[1-9]+$");
        private Regex _numOfGuests = new Regex("^([1-9]|10)$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumOfDays")
                {
                    Match match = _numOfDays.Match(NumOfDays.ToString());
                    if (!match.Success)
                        return "format: numbers greater than 1";
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumOfDays" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
