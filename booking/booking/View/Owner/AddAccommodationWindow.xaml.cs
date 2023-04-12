using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

            InitializeStateList();
            StateComboBox.ItemsSource = StateList;
            
            


        }

        private void InitializeStateList()
        {
            foreach (Location loc in ownerWindow.locations)
            {

                if (StateList.Find(m => m == loc.State) == null)

                {
                    StateList.Add(loc.State);
                }

            }
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


            return true;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {

            if (!isValid())
            {
                return;
            }

            string State = StateComboBox.Text;
            string City = CityComboBox.Text;
            int locid = ownerWindow.locations.Find(m => m.State == State && m.City == City).Id;
            int accid = ownerWindow.accommodations.Count() == 0 ? 0 : ownerWindow.accommodations.Max(a => a.Id) + 1;
            Accommodation a = new Accommodation(accid, ownerWindow.OwnerId, NameTextBox.Text, locid, TypeComboBox.Text, Convert.ToInt32(MaxVisitorsTextBox.Text), Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));
            ownerWindow.accommodationRepository.AddAccommodation(a);
            AddImage(a);
            this.Close();
        }
        private void AddImage(Accommodation a)
        {
            foreach (string url in accommodationImagesUrl)
            {
                AccommodationImage image;
                if (ownerWindow.accommodationImages.Count() == 0)
                {

                    image = new AccommodationImage(0, url, a.Id);
                }
                else
                {
                    image = new AccommodationImage(ownerWindow.accommodationImages.Max(a => a.Id) + 1, url, a.Id);
                }
                ownerWindow.accommodationImageRepository.AddAccommodationImage(image);
            }
        }
        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            
            AddAccommodationImageWindow win = new AddAccommodationImageWindow(ownerWindow.accommodationImageRepository,accommodationImagesUrl);
            win.ShowDialog();

        }

        private void StateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> CityList=new List<string>();
            CityComboBox.SelectedItem=null;
            foreach (var loc in ownerWindow.locations)
            {
                if (StateComboBox.SelectedItem.ToString() == loc.State)
                {
                    CityList.Add(loc.City);
                }
            }
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
