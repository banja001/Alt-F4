using application.UseCases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;
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

namespace WPF.ViewModels.Guest1
{
    public class ReserveAccommodationViewModel : BaseViewModel, INotifyPropertyChanged
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

        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;

        public ICommand ReserveAccommodationClickCommand => new RelayCommand(ReserveAccommodationClick);
        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand SelectedDateChangedCommand => new RelayCommand(SelectedDateChanged);
        public ICommand SearchFreeDatesCommand => new RelayCommand(SearchFreeDates);

        private int userId;
        public ReserveAccommodationViewModel(int userId)
        {
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();

            this.userId = userId;
            selectedAccommodation = Guest1ViewViewModel.SelectedAccommodation;
            AlternativeDatesVisibility = Visibility.Hidden;
            SearchButtonEnabled = true;

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

            MessageBox.Show("Your reservation has been successfully made!");

            this.CloseCurrentWindow();
        }

        private void SetSelectedDatesParameters()
        {
            SelectedDates.Id = _reservedDatesService.MakeId();
            SelectedDates.NumOfGuests = GuestsNumber;
            SelectedDates.UserId = userId;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchFreeDates()
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
            SearchButtonEnabled = NewDate.IsValid;
        }

        private void CloseWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}
