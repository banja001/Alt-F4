using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using booking.Converter;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace booking.View.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        public List<ReservedDates> ReservedDates { get; set; }

        public static ObservableCollection<ReservedDates> FreeDates { get; set; }

        public static ReservedDates SelectedDates { get; set; }

        private AccommodationLocationDTO selectedAccommodation;

        public ReservedDates NewDate { get; set; }

        public int NumOfDays { get; set; }
        
        private readonly ReservedDatesRepository _repository;

        private int userId;

        public static bool childWindowClose = false;

        public ReserveAccommodation(int userId)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new ReservedDatesRepository();

            NewDate = new ReservedDates(DateTime.Now, DateTime.Now, Guest1View.SelectedAccommodation.Id);
            selectedAccommodation = Guest1View.SelectedAccommodation;

            ReservedDates = _repository.GetAllByAccommodationId(selectedAccommodation.Id);

            FreeDates = new ObservableCollection<ReservedDates>();
            this.userId = userId;
        }

        private void FilterReservedDatesByMonth()
        {
            ReservedDates = ReservedDates.OrderBy(d => d.StartDate).ToList();
            ReservedDates = ReservedDates.Where(d => d.StartDate.Month == NewDate.StartDate.Month || d.StartDate.Month == NewDate.EndDate.Month
                                            || d.EndDate.Month == NewDate.EndDate.Month || d.EndDate.Month == NewDate.StartDate.Month).ToList();
        }

        private void ReserveAccommodationClick(object sender, RoutedEventArgs e)
        {
            if(SelectedDates == null)
            {
                MessageBox.Show("You have to pick a date before making a reservation!", "Warning");
                return;
            }

            NumberOfGuests numOfGuestsWindow = new NumberOfGuests(userId);
            numOfGuestsWindow.Owner = this;
            numOfGuestsWindow.ShowDialog();
        }

        private void SearchFreeDates(object sender, RoutedEventArgs e)
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
                AlternativeDates.Visibility = Visibility.Hidden;

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
            foreach(var date in ReservedDates)
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
            AlternativeDates.Visibility = Visibility.Visible;

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
            FreeDates.Clear();

            FreeDates.Add(secondClosestBefore);
            FreeDates.Add(closestDateBefore);
            FreeDates.Add(closestDateAfter);
            FreeDates.Add(secondClosestAfter);

            accommodationData.ItemsSource = FreeDates;
        }

        private void NumOfDaysTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Int32.Parse(NumOfDaysTextBox.Text);
                SearchFreeDatesButton.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("You have to enter numbers for number of days!", "Warning");
                SearchFreeDatesButton.IsEnabled = false;
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchFreeDatesButton.IsEnabled = NewDate.IsValid;
        }
    }
}
