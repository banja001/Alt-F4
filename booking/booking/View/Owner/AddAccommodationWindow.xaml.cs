using booking.Manager;
using booking.Model;
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
        private AccommodationRepository accommodationrepository;
        private AccommodationImageRepository accommodationImageRepository;
        private LocationRepository locationRepository;
        private List<string> accommodationImagesUrl;
        public AddAccommodationWindow(AccommodationRepository accMen, LocationRepository locrep,AccommodationImageRepository accirep)
        {
            InitializeComponent();
            DataContext = this;
            accommodationrepository = accMen;
            accommodationImageRepository = accirep;
            accommodationImagesUrl = new List<string>();
            locationRepository = locrep;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            List<Location> locations=locationRepository.GetAllLocations();

            int locid;
            if (locations.Count() == 0)
            {
                locid = 0;
                Location loc = new Location(locid, GradTextBox.Text, DrzavaTextBox.Text);
                locationRepository.AddLocation(loc);
            }
            else
            {
                Location location = locations.Find(a => a.Drzava == DrzavaTextBox.Text && a.Grad == GradTextBox.Text);
                if (location == null)
                {
                    locid = locations.Max(a => a.Id) + 1;
                    Location loc = new Location(locid, GradTextBox.Text, DrzavaTextBox.Text);
                    locationRepository.AddLocation(loc);
                }
                else
                {
                    locid = location.Id;
                }
                
            }
            
            
            

            List<Accommodation> acc = accommodationrepository.GetAllAccommodations();
            int accid;
            if (acc.Count() == 0)
            {
                accid = 0;
            }
            else
            {
                accid=acc.Max(a => a.Id) + 1;
            }
            Accommodation a = new Accommodation(accid,
            NameTextBox.Text,locid,TypeComboBox.Text,Convert.ToInt32(MaxVisitorsTextBox.Text),
            Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));
            accommodationrepository.AddAccommodation(a);

            List<AccommodationImage> acci=accommodationImageRepository.GetAllImages();
            foreach(string url in accommodationImagesUrl)
            {
                AccommodationImage image;
                if (acci.Count() == 0)
                {

                    image = new AccommodationImage(0, url, a.Id);
                }
                else
                {
                    image=new AccommodationImage(acci.Max(a => a.Id) + 1,url,a.Id);
                }
                

                accommodationImageRepository.AddAccommodationImage(image);
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
            
            AddAccommodationImageWindow win = new AddAccommodationImageWindow(accommodationImageRepository,accommodationImagesUrl);
            win.Show();

        }
    }
}
