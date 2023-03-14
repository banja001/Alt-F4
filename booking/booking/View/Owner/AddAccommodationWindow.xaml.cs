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

        public AddAccommodationWindow(OwnerWindow win)
        {
            InitializeComponent();
            DataContext = this;
            ownerWindow = win;
            
            accommodationImagesUrl = new List<string>();
            
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            

            int locid;
            if (ownerWindow.locations.Count() == 0)
            {
                locid = 0;
                Location loc = new Location(locid, GradTextBox.Text, DrzavaTextBox.Text);
                ownerWindow.locationRepository.AddLocation(loc);
            }
            else
            {
                Location location = ownerWindow.locations.Find(a => a.State == DrzavaTextBox.Text && a.City == GradTextBox.Text);
                if (location == null)
                {
                    locid = ownerWindow.locations.Max(a => a.Id) + 1;
                    Location loc = new Location(locid, GradTextBox.Text, DrzavaTextBox.Text);
                    ownerWindow.locationRepository.AddLocation(loc);
                }
                else
                {
                    locid = location.Id;
                }
                
            }

            
            int accid;
            if (ownerWindow.accommodations.Count() == 0)
            {
                accid = 0;
            }
            else
            {
                accid= ownerWindow.accommodations.Max(a => a.Id) + 1;
            }
            Accommodation a = new Accommodation(accid,
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
    }
}
