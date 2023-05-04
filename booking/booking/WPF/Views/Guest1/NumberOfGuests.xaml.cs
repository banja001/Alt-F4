using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for NumberOfGuests.xaml
    /// </summary>
    public partial class NumberOfGuests : Window
    {
        private readonly ReservedDatesRepository _datesRepository;

        private readonly AccommodationRepository _accommodationRepository;

        private ReservedDates selectedDates;

        private int userId;

        public int GuestsNumber { get; set; }
        public NumberOfGuests(int id)
        {
            InitializeComponent();
            DataContext = this;

            _datesRepository = new ReservedDatesRepository();
            _accommodationRepository = new AccommodationRepository();

            selectedDates = ReserveAccommodation.SelectedDates;

            userId = id;
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            int maxCapacity = _accommodationRepository.FindById(selectedDates.AccommodationId).MaxCapacity;

            if (GuestsNumber > maxCapacity)
            {
                MessageBox.Show("Max guest capacity for this accommodation is " + maxCapacity);
                return;
            }

            SetSelectedDatesParameters();
            _datesRepository.Add(selectedDates);

            MessageBox.Show("Your reservation has been successfully made!");

            this.Close();
            ReserveAccommodation.childWindowClose = true;
        }

        private void SetSelectedDatesParameters()
        {
            selectedDates.Id = _datesRepository.MakeId();
            selectedDates.NumOfGuests = GuestsNumber;
            selectedDates.UserId = userId;
        }

        private void NumOfGuestsTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Int32.Parse(NumOfGuestsTextBox.Text);
                ReserveButton.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("You have to enter a number for number of guests!");
                ReserveButton.IsEnabled = false;
            }
        }
    }
}
