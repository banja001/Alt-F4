using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

            foreach (Location loc in ownerWindow.locations)
            {
                if (StateList.Find(m=>m==loc.State) == null)
                {
                    StateList.Add(loc.State);
                }
                
            }

            StateComboBox.ItemsSource = StateList;
            

        }

        

        private void Confirm(object sender, RoutedEventArgs e)
        {

            string State=StateComboBox.Text;
            string City = CityComboBox.Text;


            int locid = ownerWindow.locations.Find(m => m.State == State && m.City==City).Id;     
            int accid;
            if (ownerWindow.accommodations.Count() == 0)
            {
                accid = 0;
            }
            else
            {
                accid= ownerWindow.accommodations.Max(a => a.Id) + 1;
            }
            Accommodation a = new Accommodation(accid,ownerWindow.OwnerId,
            NameTextBox.Text,locid,TypeComboBox.Text,Convert.ToInt32(MaxVisitorsTextBox.Text),
            Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));

            ownerWindow.accommodationRepository.AddAccommodation(a);

            
            foreach(string url in accommodationImagesUrl)
            {
                AccommodationImage image;
                if (ownerWindow.accommodationImages.Count() == 0)
                {

                    image = new AccommodationImage(0, url, a.Id);
                }
                else
                {
                    image=new AccommodationImage(ownerWindow.accommodationImages.Max(a => a.Id) + 1,url,a.Id);
                }
                

                ownerWindow.accommodationImageRepository.AddAccommodationImage(image);
            }

            this.Close(); 
        }







        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            
            AddAccommodationImageWindow win = new AddAccommodationImageWindow(ownerWindow.accommodationImageRepository,accommodationImagesUrl);
            win.Show();

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
    }
}
