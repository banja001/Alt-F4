using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AddAccommodationWindow.xaml
    /// </summary>
    public partial class AddAccommodationWindow : Window
    {

        private List<string> accommodationImagesUrl;
        public OwnerWindow ownerWindow;
        public List<string> StateList;
        public AddAccommodationWindow(OwnerWindow win)
        {
            InitializeComponent();
            DataContext = this;
            ownerWindow = win;
            accommodationImagesUrl = new List<string>();
            StateList = new List<string>();
            StateList=ownerWindow.locationService.InitializeStateList(StateList);
            StateComboBox.ItemsSource = StateList;
        }
        public Regex intRegex = new Regex("^[0-9]{1,4}$");
        private bool isValid()
        {
            if (string.IsNullOrEmpty(StateComboBox.Text) || string.IsNullOrEmpty(CityComboBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(TypeComboBox.Text) ||
                string.IsNullOrEmpty(MaxVisitorsTextBox.Text) || string.IsNullOrEmpty(MinDaysToUseTextBox.Text) || string.IsNullOrEmpty(DaysToCancelTextBox.Text))
            {
                MessageBox.Show("Please fill in all of the textboxes", "Error");
                return false;
            }
            Match match = intRegex.Match(MaxVisitorsTextBox.Text);
            if (!match.Success)
            {
                MessageBox.Show("Max visitors should be a valid number", "Error");
                return false; ;
            }
            match = intRegex.Match(MinDaysToUseTextBox.Text);
            if (!match.Success)
            {
                MessageBox.Show("Min reservation should be a valid number", "Error");
                return false;
            }
            match = intRegex.Match(DaysToCancelTextBox.Text);
            if (!match.Success)
            {
                MessageBox.Show("Days to cancel should be a valid number", "Error");
                return false;
            }
            if (accommodationImagesUrl.Count() == 0)
            {
                MessageBox.Show("Please enter atleast one image", "Error");
                return false;
            }
            if (NameTextBox.Text.Last().Equals('*'))
            {
                MessageBox.Show("Accommodation name cant end with *", "Error");
                return false;
            }


            return true;
        }
        private void Confirm(object sender, RoutedEventArgs e)
        {

            if (!isValid())
            {
                return;
            }

            Accommodation a = AddAccommodation();
            ownerWindow.accommodationRepository.Add(a);
            ownerWindow.accommodationImageService.AddImages(a, accommodationImagesUrl);
            this.Close();
        }

        private Accommodation AddAccommodation()
        {
            string State = StateComboBox.Text;
            string City = CityComboBox.Text;
            int locid = ownerWindow.locationService.GetLocationId(State, City);
            int accid = ownerWindow.accommodations.Count() == 0 ? 0 : ownerWindow.accommodations.Max(a => a.Id) + 1;
            Accommodation a = new Accommodation(accid, ownerWindow.OwnerId, NameTextBox.Text, locid, TypeComboBox.Text, Convert.ToInt32(MaxVisitorsTextBox.Text), Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));
            return a;
        }

        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            
            AddAccommodationImageWindow win = new AddAccommodationImageWindow(accommodationImagesUrl);
            win.ShowDialog();

        }
        private void StateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> CityList = new List<string>();
            CityComboBox.SelectedItem = null;
            string SelectedState = StateComboBox.SelectedItem.ToString();
            CityList=ownerWindow.locationService.FillCityList(CityList,SelectedState);
            CityComboBox.ItemsSource = CityList;
        }
        private void RemoveImageClick(object sender, RoutedEventArgs e)
        {
            if (accommodationImagesUrl.Count() > 0)
            {
                accommodationImagesUrl.RemoveAt(accommodationImagesUrl.Count() - 1);
                MessageBox.Show("Image removed","Message");
            }
            
        }
    }
}
