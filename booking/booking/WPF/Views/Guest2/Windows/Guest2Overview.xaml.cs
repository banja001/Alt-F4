using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
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
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Guide;

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
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly TourAttendanceRepository _tourAttendanceRepository;
        private readonly AppointmentCheckPointRepository _appointmentCheckPointRepository;
        public ObservableCollection<TourLocationDTO> TourLocationDTOs { get; set; }
        public ObservableCollection<string> States { get; set; }
        public TourLocationDTO SelectedTour { get; set; }

        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }
        public User currentUser { get; set; }   

        public Answer Answer { get; set; }
        public Guest2Overview(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            _locationRepository = new LocationRepository();
            _tourRepository = new TourRepository();
            _tourImageRepository = new TourImageRepository();
            _answerRepository = new AnswerRepository();
            _reservationTourRepository = new ReservationTourRepository();

            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
            States = new ObservableCollection<string>();
            currentUser = user;
            FillStateComboBox();
            RemoveFullTours();

            _tourAttendanceRepository= new TourAttendanceRepository();
            _appointmentCheckPointRepository = new AppointmentCheckPointRepository();

            welcome.Header = "Welcome " + currentUser.Username.ToString();
            FindAnswer();
        }
        private void FillStateComboBox()
        {
            List<Location> locations = _locationRepository.GetAll();
            foreach(Location location in locations)
            {
                String state = location.State;
                if(!States.Contains(state))
                    States.Add(state);
            }
        }

        public List<TourLocationDTO> CreateTourDTOs()
        { 
            List<Location> locations = _locationRepository.GetAll();
            List<TourImage> tourImages = _tourImageRepository.findAll();
            List<TourLocationDTO> localTourLocationDTOs = new List<TourLocationDTO>();
            foreach (Tour tour in _tourRepository.FindAll())
            {
                Location location = locations.Find(l => l.Id == tour.Location.Id);
                List<TourImage> images = tourImages.FindAll(i => i.TourId == tour.Id);

                localTourLocationDTOs.Add(new TourLocationDTO(tour.Id, tour.Name, tour.Description,
                                  location.City + "," + location.State, tour.Language, tour.MaxGuests,
                                  tour.StartTime.Date, tour.Duration, images));
            }
            return localTourLocationDTOs;
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
        private void PeopleCountLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(PeopleCount, "People count");
        }

        private void PeopleCountGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(PeopleCount, "People count");
        }

        private void LanguageGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(Language, "Language");
        }

        private void LanguageLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(Language, "Language");
        }
        private void DurationGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(Duration, "Duration(h)");
        }
        private void DurationLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(Duration, "Duration(h)");
        }

        private void MoreDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            var moreDetailsWindow = new MoreDetailsOverview(this);
            moreDetailsWindow.Owner = this;
            moreDetailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            moreDetailsWindow.ShowDialog();
        }

        private void BookTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                var bookTourWindow = new BookTourOverview(this, currentUser);
                bookTourWindow.Owner = this;
                bookTourWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                bookTourWindow.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("Niste izabrali turu koju zelite da rezervisete!");
        }

        public void RemoveFullTours() 
        {
            foreach (TourLocationDTO tour in TourLocationDTOs.ToList<TourLocationDTO>())
            {
                int numberOfGuests = _reservationTourRepository.GetNumberOfGuestsForTourId(tour.Id);
                if (numberOfGuests >= tour.MaxGuests)
                {
                    TourLocationDTOs.Remove(tour);
                }
            }
        }

        private void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // kada korisnik pretrazi, vraca se index combobox-ova na -1, pa treba pokriti exception
            if (StateComboBox.SelectedIndex != -1)
            {
                CityComboBox.IsEnabled = true;
                List<Location> locations = _locationRepository.GetAll();
                List<string> cities = new List<string>();
                foreach (Location location in locations)
                {
                    String city = location.City;
                    bool isValid = !cities.Contains(city) && SelectedState.Equals(location.State);
                    if (isValid)
                        cities.Add(city);
                }
                CityComboBox.ItemsSource = cities;
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Invalid search format!", "Format warning");

                // Resetuj sva polja za pretrazivanje na default za novi pokusaj
                PeopleCount.Text = "People count";
                Language.Text = "Language";
                Duration.Text = "Duration(h)";
                PeopleCount.Foreground = Brushes.LightGray;
                Language.Foreground = Brushes.LightGray;
                Duration.Foreground = Brushes.LightGray;

                StateComboBox.SelectedIndex = -1;
                CityComboBox.SelectedIndex = -1;

                return;
            }
            FilterTable();
        }

        private void FilterTable()
        { 
            // Kada se filtriria datagrid, mora filtrirati u odnosu na sve slobodne ture
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
            RemoveFullTours();

            if(!PeopleCount.Text.Equals("People count"))
                FilterByPeopleCount(int.Parse(PeopleCount.Text));
            if (!Language.Text.Equals("Language"))
                FilterByLanguage(Language.Text);

            if (string.IsNullOrEmpty(SelectedState))
                SelectedState = "";
            if (string.IsNullOrEmpty(SelectedCity))
                SelectedCity = "";
            FilterByLocation(SelectedState + "," + SelectedCity);

            if (!Duration.Text.Equals("Duration(h)"))
                FilterByDuration(int.Parse(Duration.Text));

            bool checkForReset = PeopleCount.Text.Equals("People count") && Language.Text.Equals("Language")
                                 && string.IsNullOrEmpty(SelectedState) && string.IsNullOrEmpty(SelectedCity)
                                 && Duration.Text.Equals("Duration(h)");
            if (checkForReset)
            {
                TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
                RemoveFullTours();
            }

            tourSelectionTable.ItemsSource = TourLocationDTOs;
            StateComboBox.SelectedIndex = -1;
            CityComboBox.SelectedIndex = -1;
            CityComboBox.IsEnabled = false;
            
        }

        private bool IsInputValid() 
        {
            Regex peopleCountRegex = new Regex("^[1-9][0-9]*$");
            Regex languageRegex = new Regex("^[A-ZČĆŠĐŽ]*[a-zčćšđž]*$");
            Regex durationRegex = new Regex("^[1-9][0-9]*$");
            /* State and City are already valid because customer doesn't enter them explicitly */


            bool validPeopleCount = peopleCountRegex.IsMatch(PeopleCount.Text) || (PeopleCount.Text.Equals("People count"));
            bool validDuration = durationRegex.IsMatch(Duration.Text) || (Duration.Text.Equals("Duration(h)"));
            bool validLanguage = languageRegex.IsMatch(Language.Text) || (Language.Text.Equals("Language"));
            bool isValid = validDuration && validLanguage && validPeopleCount;

            return isValid;
        }

        public void FilterByPeopleCount(int peopleCount)
        {   // U slucaju da je korisnik hteo da pregleda ture na osnovu slobodnog mesta:
            // List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.MaxGuests - _reservationTourRepository.GetNumberOfGuestsForTourId(t.Id) >= peopleCount).ToList();
            
            // Pregled tura na osnovu maksimalnog kapaciteta ture:
            List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.MaxGuests >= peopleCount).ToList();
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(localDTOs);
            
        }
        public void FilterByLanguage(string language)
        {
            List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.Language.ToLower().Contains(language.ToLower())).ToList();
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(localDTOs);
        }
        public void FilterByLocation(string formattedLocation)
        {
            Location location = new Location();
            location.State = formattedLocation.Split(",")[0];
            location.City = formattedLocation.Split(",")[1];

            if (!location.State.Equals(""))
            {
                List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.Location.Split(",")[1].Equals(location.State)).ToList();
                TourLocationDTOs = new ObservableCollection<TourLocationDTO>(localDTOs);
            }
            if (!location.City.Equals(""))
            {
                List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.Location.Split(",")[0].Equals(location.City)).ToList();
                TourLocationDTOs = new ObservableCollection<TourLocationDTO>(localDTOs);
            }
        }
        public void FilterByDuration(int duration)
        {
            List<TourLocationDTO> localDTOs = TourLocationDTOs.Where(t => t.Duration <= duration).ToList();
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(localDTOs);
        }
        private void LogOutButtonClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void FindAnswer()
        {
            foreach (Answer a in _answerRepository.FindAll())
            {
                int userIdInAnswer = _reservationTourRepository.GetById(_tourAttendanceRepository.GetById(a.User.Id).Guest.Id).User.Id;

                if (userIdInAnswer == currentUser.Id)
                {

                    AppointmentCheckPoint currentCheckPoint = _appointmentCheckPointRepository.FindAll().Find(chepo => chepo.Id == a.AppointmentCheckPoint.Id);
                   if(a.HaveToAnswer)
                       if( MessageBox.Show("Are you on check point " + currentCheckPoint.Name + "?","Confirmation",MessageBoxButton.YesNo)==MessageBoxResult.Yes )
                       {
                            a.HaveToAnswer = false;
                            a.AppointmentCheckPoint= currentCheckPoint;
                            _answerRepository.SaveOneInFile(a);
                       }
                }
            }
        }
    }
}
