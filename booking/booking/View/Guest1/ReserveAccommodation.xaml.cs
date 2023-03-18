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

namespace booking.View.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        public List<ReservedDates> ReservedDates { get; set; }

        public static ObservableCollection<ReservedDates> FreeDates { get; set; }

        private AccommodationLocationDTO selectedAccommodation;

        public ReservedDates NewDate { get; set; }

        public int NumOfDays { get; set; }

        private readonly ReservedDatesRepository _repository;

        public ReserveAccommodation()
        {
            InitializeComponent();
            DataContext = this;

            _repository = new ReservedDatesRepository();

            NewDate = new ReservedDates(DateTime.Now, DateTime.Now, AccomodationOverview.SelectedAccommodation.Id);
            selectedAccommodation = AccomodationOverview.SelectedAccommodation;

            ReservedDates = _repository.GetAllByAccommodationId(selectedAccommodation.Id);
            FreeDates = new ObservableCollection<ReservedDates>();
        }

        private void ReserveAccommodationClick(object sender, RoutedEventArgs e)
        {
            

            
        }

        private void SearchFreeDates(object sender, RoutedEventArgs e)
        {
            if (NumOfDays < selectedAccommodation.MinDaysToUse)
            {
                MessageBox.Show("You can reserve this accommodation for at least " + selectedAccommodation.MinDaysToUse + " days");
                return;
            }

            FreeDates.Clear();
            foreach (ReservedDates reservedDate in ReservedDates)
            {
                if (NewDate.StartDate < reservedDate.StartDate && reservedDate.EndDate < NewDate.EndDate)
                {
                    AddDates(NewDate.StartDate, reservedDate.StartDate);
                    AddDates(reservedDate.EndDate, NewDate.EndDate);
                }
                else if(NewDate.StartDate < reservedDate.StartDate && NewDate.EndDate > reservedDate.StartDate && NewDate.EndDate < reservedDate.EndDate)
                {
                    AddDates(NewDate.StartDate, reservedDate.StartDate);
                }
                else if(NewDate.StartDate > reservedDate.StartDate && NewDate.StartDate < reservedDate.EndDate && NewDate.EndDate > reservedDate.EndDate)
                {
                    AddDates(reservedDate.EndDate, NewDate.EndDate);
                }
            }
        }
        
        private void AddDates(DateTime startDate, DateTime endDate)
        {
            while((endDate - startDate).Days >= NumOfDays)
            {
                FreeDates.Add(new ReservedDates(startDate, startDate.AddDays(NumOfDays), selectedAccommodation.Id));

                startDate = startDate.AddDays(NumOfDays);
            }
        }
    }
}
