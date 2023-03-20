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
    public partial class ReserveAccommodation : Window, INotifyPropertyChanged
    {
        public List<ReservedDates> ReservedDates { get; set; }

        public static ObservableCollection<ReservedDates> FreeDates { get; set; }

        public static ReservedDates SelectedDates { get; set; }

        private AccommodationLocationDTO selectedAccommodation;

        public ReservedDates NewDate { get; set; }

        public int NumOfDays { get; set; }
        
        private readonly ReservedDatesRepository _repository;

        private int userId;

        public ReserveAccommodation(int userId)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new ReservedDatesRepository();

            NewDate = new ReservedDates(DateTime.Now, DateTime.Now, AccomodationOverview.SelectedAccommodation.Id);
            selectedAccommodation = AccomodationOverview.SelectedAccommodation;

            ReservedDates = _repository.GetAllByAccommodationId(selectedAccommodation.Id);
            ReservedDates = ReservedDates.OrderBy(d => d.StartDate).ToList();


            FreeDates = new ObservableCollection<ReservedDates>();
            this.userId = userId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            FreeDates.Clear();
            AlternativeDates.Visibility = Visibility.Hidden;
            bool reservedDateExists = false;

            foreach (ReservedDates reservedDate in ReservedDates)
            {
                if (NewDate.StartDate < reservedDate.StartDate && reservedDate.EndDate < NewDate.EndDate)
                {
                    AddDates(NewDate.StartDate, reservedDate.StartDate);
                    reservedDateExists = AddDates(reservedDate.EndDate, NewDate.EndDate);
                }
                else if (NewDate.StartDate < reservedDate.StartDate && NewDate.EndDate > reservedDate.StartDate && NewDate.EndDate < reservedDate.EndDate)
                {
                    reservedDateExists = AddDates(NewDate.StartDate, reservedDate.StartDate);
                }
                else if (NewDate.StartDate >= reservedDate.StartDate && NewDate.StartDate <= reservedDate.EndDate)
                {
                    if (NewDate.EndDate > reservedDate.EndDate)
                        AddDates(reservedDate.EndDate, NewDate.EndDate);

                    reservedDateExists = true;
                }
            }


            if (FreeDates.Count == 0)
            {
                if (!reservedDateExists)
                {
                    AddDates(NewDate.StartDate, NewDate.EndDate);
                }
                else
                {
                    OfferAlternativeDates();
                }
            }
        }
        
        private bool AddDates(DateTime startDate, DateTime endDate)
        {
            while ((endDate - startDate).Days >= NumOfDays - 1)
            {
                FreeDates.Add(new ReservedDates(startDate, startDate.AddDays(NumOfDays - 1), selectedAccommodation.Id));

                startDate = startDate.AddDays(1);
            }

            return true;
        }

        private void OfferAlternativeDates()
        {
            AlternativeDates.Visibility = Visibility.Visible;

            CreatePossibleIntervals();

            ReservedDates closestDateBefore = FreeDates.MinBy(d => Math.Abs((NewDate.StartDate - d.EndDate).Days));
            ReservedDates closestDateAfter = FreeDates.MinBy(d => Math.Abs((d.StartDate - NewDate.EndDate).Days));

            int closestDateBeforeIndx = FreeDates.IndexOf(closestDateBefore);
            int closestDateAfterIndx = FreeDates.IndexOf(closestDateAfter);

            ReservedDates secondClosestBefore = FreeDates[--closestDateBeforeIndx];
            ReservedDates secondClosestAfter = FreeDates[++closestDateAfterIndx];


            FreeDates.Clear();

            FreeDates.Add(secondClosestBefore);
            FreeDates.Add(closestDateBefore);
            FreeDates.Add(closestDateAfter);
            FreeDates.Add(secondClosestAfter);

            accommodationData.ItemsSource = FreeDates;
        }

        private void CreatePossibleIntervals()
        {
            int startMonth = NewDate.StartDate.Month;
            int endMonth = NewDate.EndDate.Month;
            int startYear = NewDate.StartDate.Year;

            List<ReservedDates> filteredReservedDates = ReservedDates.Where(d => d.StartDate.Month == startMonth || d.StartDate.Month == endMonth
            || d.EndDate.Month == startMonth || d.EndDate.Month == endMonth).ToList();

            List <DateTime> dates = Enumerable.Range(1, DateTime.DaysInMonth(startYear, startMonth))
                    .Select(day => new DateTime(startYear, startMonth, day))
                    .ToList();
            
            while (startMonth != endMonth)
            {
                startMonth++;

                if (startMonth == 13)
                {
                    startMonth = 1;
                    startYear++;
                }

                for (var date = new DateTime(startYear, startMonth, 1); date.Month == startMonth; date = date.AddDays(1))
                {
                    dates.Add(date);
                }
            }

            foreach (DateTime date in dates)
            {
                FreeDates.Add(new ReservedDates(date, date.AddDays(NumOfDays - 1), selectedAccommodation.Id));
            }

            List<ReservedDates> freeDatesCpy = new List<ReservedDates>(FreeDates);

            foreach (ReservedDates reservedDate in filteredReservedDates)
            {
                if(NewDate.StartDate.Month > reservedDate.StartDate.Month)
                {
                    reservedDate.StartDate = new DateTime(reservedDate.EndDate.Year, reservedDate.EndDate.Month, NumOfDays);
                }

                if (NewDate.EndDate.Month < reservedDate.EndDate.Month)
                {
                    reservedDate.EndDate = new DateTime(reservedDate.EndDate.Year, reservedDate.EndDate.Month, DateTime.DaysInMonth(reservedDate.EndDate.Year, reservedDate.EndDate.Month) - NumOfDays);
                }

                ReservedDates startDate = FreeDates.Where(d => d.EndDate == reservedDate.StartDate).ToList()[0];
                ReservedDates endDate = FreeDates.Where(d => d.StartDate == reservedDate.EndDate).ToList()[0];

                int startIndx = FreeDates.IndexOf(startDate);

                int i = (endDate.StartDate - FreeDates[startIndx + 1].StartDate).Days - 1;
                int j = 0;

                while ( j <= i )
                {
                    if(freeDatesCpy.Contains(FreeDates[startIndx + 1 + j]))
                        freeDatesCpy.Remove(FreeDates[startIndx + 1 + j]);

                    j++;
                }              
            }

            FreeDates = new ObservableCollection<ReservedDates>(freeDatesCpy);
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
            if (!NewDate.IsValid)
            {
                SearchFreeDatesButton.IsEnabled = false;
            }
            else
            {
                SearchFreeDatesButton.IsEnabled = true;
            }
        }

    }
}
