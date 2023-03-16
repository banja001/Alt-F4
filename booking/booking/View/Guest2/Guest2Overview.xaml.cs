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
using booking.DTO;
using booking.Model;
using booking.Repository;

namespace booking.View.Guest2
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {

        private readonly LocationRepository _locationRepository;
        private readonly TourRepository _tourRepository;
        private readonly TourImageRepository _tourImageRepository;
        public ObservableCollection<TourLocationDTO> TourLocationDTOs { get; set; }
        public TourLocationDTO SelectedTour { get; set; }
        public Guest2Overview(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            _locationRepository = new LocationRepository();
            _tourRepository = new TourRepository();
            _tourImageRepository = new TourImageRepository();
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(); 
            CreateTourDTOs();
            

            welcome.Header = "Welcome " + user.Username.ToString();

        }

        public void CreateTourDTOs()
        { 
            List<Location> locations = _locationRepository.GetAllLocations();
            List<TourImage> tourImages = _tourImageRepository.findAll();
            foreach (Tour tour in _tourRepository.findAll())
            {
                Location location = locations.Find(l => l.Id == tour.Location.Id);
                List<TourImage> images = tourImages.FindAll(i => i.TourId == tour.Id);
                TourLocationDTOs.Add(new TourLocationDTO(tour.Id, tour.Name, tour.Description, 
                                     location.City + "," + location.State, tour.Language, tour.MaxGuests,
                                     tour.StartTime, tour.Duration, images));
            }
        }
        private void SetContentToDefault(TextBox selectedTextbox, string defaultText)
        {
            if (selectedTextbox.Text.Equals(""))
            {
                selectedTextbox.Text = defaultText;
                selectedTextbox.Foreground = Brushes.LightGray;
            }
        }
        private void RemoveContent(TextBox selectedTextBox, string defaultText)
        {
            if (selectedTextBox.Text.Equals(defaultText))
            {
                selectedTextBox.Text = "";
                selectedTextBox.Foreground = Brushes.Black;
            }
        }
        private void PeopleCount_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(peopleCount, "People count");
        }

        private void PeopleCount_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(peopleCount, "People count");
        }

        private void Language_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(language, "Language");
        }

        private void Language_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(language, "Language");
        }

        private void Location_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(location, "Location");
        }
        private void Location_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(location, "Location");
        }
        private void TimeSpan_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(timeSpan, "Time span");
        }
        private void TimeSpan_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(timeSpan, "Time span");
        }

        private void MoreDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var moreDetailsWindow = new MoreDetailsOverview(this);
            moreDetailsWindow.Owner = this;
            moreDetailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            moreDetailsWindow.ShowDialog();
        }

        private void BookTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                var bookTourWindow = new BookTourOverview(this);
                bookTourWindow.Owner = this;
                bookTourWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                bookTourWindow.ShowDialog();
            }
            else
                MessageBox.Show("Niste izabrali turu koju zelite da rezervisete!");
        }
    }
}
