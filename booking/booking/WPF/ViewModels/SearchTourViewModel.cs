using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Guest2;
using booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using booking.WPF.ViewModels;
using System.Windows.Input;
using booking.Commands;
using static System.Net.Mime.MediaTypeNames;
using WPF.Views.Guest2;
using System.Windows.Media.Effects;

namespace WPF.ViewModels
{
    public class SearchTourViewModel : BaseViewModel
    {
        private readonly LocationRepository _locationRepository;
        private readonly TourRepository _tourRepository;
        private readonly TourImageRepository _tourImageRepository;
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly TourAttendanceRepository _tourAttendanceRepository;
        private readonly AppointmentCheckPointRepository _appointmentCheckPointRepository;
        private readonly UserRepository _userRepository;
        public ObservableCollection<TourLocationDTO> TourLocationDTOs { get; set; }
        public ObservableCollection<string> States { get; set; }

        private TourLocationDTO _selectedTour;
        public TourLocationDTO SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                //BookATourCommand.RaiseCanExecuteChanged();
                _selectedTour = value;
            }
        }
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }
        public User currentUser { get; set; }
        public String Duration { get; set; }
        public String Language { get; set; }
        public String PeopleCount { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public Answer Answer { get; set; }
        public ICommand SearchCommand => new RelayCommand(OnSearchButtonClick);
        public ICommand SeeMoreCommand => new RelayCommand(OnMoreDetailsButtonClick);
        public RelayCommand BookATourCommand => new RelayCommand(OnBookTourButtonClick);
        public ICommand FillCitiesCommand => new RelayCommandWithParams(OnStateComboBoxSelectionChanged);
        public SearchTourViewModel(User user)
        {
            _locationRepository = new LocationRepository();
            _tourRepository = new TourRepository();
            _tourImageRepository = new TourImageRepository();
            _answerRepository = new AnswerRepository();
            _reservationTourRepository = new ReservationTourRepository();
            _userRepository= new UserRepository();

            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
            States = new ObservableCollection<string>();
            currentUser = user;
            FillStateComboBox();
            RemoveFullTours();
            RemoveFormerTours();

            _tourAttendanceRepository = new TourAttendanceRepository();
            _appointmentCheckPointRepository = new AppointmentCheckPointRepository();
        }
        private void FillStateComboBox()
        {
            List<Location> locations = _locationRepository.GetAll();
            foreach (Location location in locations)
            {
                String state = location.State;
                if (!States.Contains(state))
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
                                  tour.StartTime.Date, tour.Duration, images,tour.Guide.Id));
            }
            
            return localTourLocationDTOs;
        }
        private void OnMoreDetailsButtonClick()
        {
            Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Effect = new BlurEffect();
            }
            var moreDetailsWindow = new MoreDetailsView(SelectedTour);
            moreDetailsWindow.ShowDialog();
            window.Effect = null;
        }

        private void OnBookTourButtonClick()
        {
            if (SelectedTour != null)
            {
                Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if (window != null)
                {
                    window.Effect = new BlurEffect();
                }
                var bookTourWindow = new BookTourView(SelectedTour, currentUser, this);
                bookTourWindow.ShowDialog();
                window.Effect = null;
                RemoveInvalidTours();
                OnPropertyChanged(nameof(TourLocationDTOs));
            }
            else
                MessageBox.Show("Choose tour for booking!");
        }
        private bool CanBookTourButtonClick()
        {
            return (SelectedTour != null);
        }
        public void RemoveInvalidTours()
        {
            RemoveFullTours();
            RemoveFormerTours();
        }
        public void RemoveFormerTours()
        {
            foreach (TourLocationDTO tour in TourLocationDTOs.ToList<TourLocationDTO>())
            {
                if (tour.StartTime.Date.Date < DateTime.Now.Date)
                {
                    TourLocationDTOs.Remove(tour);
                }
            }
            List<TourLocationDTO> TourLocations=new List<TourLocationDTO>(TourLocationDTOs);
            List<TourLocationDTO> TourLocationsUnder = new List<TourLocationDTO>();
            List<TourLocationDTO> TourLocationsFirst= new List<TourLocationDTO>();


            TourLocationDTOs.Clear();
            foreach (var tour in TourLocations)
            {
                if (_userRepository.GetById(tour.Guide.Id).SuperGuide && _userRepository.GetById(tour.Guide.Id).SuperGuideLanguage == tour.Language)
                {
                    tour.Language=tour.Language+"\n(SuperGuide)";
                    TourLocationsFirst.Add(tour);
                }
                else
                    TourLocationsUnder.Add(tour);
            }
            if (TourLocationsFirst.Count > 0)
            {
                foreach (var tour in TourLocationsFirst)
                {
                    TourLocationDTOs.Add(tour);
                }
            }
            if (TourLocationsUnder.Count > 0)
            {
                foreach (var tour in TourLocationsUnder)
                {
                    TourLocationDTOs.Add(tour);
                }
            }
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

        private void OnStateComboBoxSelectionChanged(object parameter)
        {
            if (int.Parse(parameter.ToString()) != -1)
            {
                List<Location> locations = _locationRepository.GetAll();
                List<string> cities = new List<string>();
                if(Cities != null)
                    Cities.Clear();
                else
                    Cities = new ObservableCollection<string>();

                foreach (Location location in locations)
                {
                    String city = location.City;
                    bool isValid = !cities.Contains(city) && SelectedState.Equals(location.State);
                    if (isValid)
                    {
                        Cities.Add(city);
                        cities.Add(city);   
                    }
                }
                OnPropertyChanged(nameof(Cities));
            }
        }

        private void OnSearchButtonClick()
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Invalid search format!", "Format warning");
                return;
            }
            FilterTable();
            OnPropertyChanged(nameof(TourLocationDTOs));
        }

        private void FilterTable()
        {
            TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
            RemoveFullTours();
            RemoveFormerTours();

            if (!PeopleCount.Equals("People count"))
                FilterByPeopleCount(int.Parse(PeopleCount));
            if (!Language.Equals("Language"))
                FilterByLanguage(Language);

            if (string.IsNullOrEmpty(SelectedState))
                SelectedState = "";
            if (string.IsNullOrEmpty(SelectedCity))
                SelectedCity = "";
            FilterByLocation(SelectedCity + "," + SelectedState);

            if (!Duration.Equals("Duration(h)"))
                FilterByDuration(int.Parse(Duration));

            bool checkForReset = PeopleCount.Equals("People count") && Language.Equals("Language")
                                 && string.IsNullOrEmpty(SelectedState) && string.IsNullOrEmpty(SelectedCity)
                                 && Duration.Equals("Duration(h)");
            if (checkForReset)
            {
                TourLocationDTOs = new ObservableCollection<TourLocationDTO>(CreateTourDTOs());
                RemoveFullTours();
                RemoveFormerTours();
            }

        }

        private bool IsInputValid()
        {
            Regex peopleCountRegex = new Regex("^[1-9][0-9]*$");
            Regex languageRegex = new Regex("^[A-ZČĆŠĐŽ]*[a-zčćšđž]*$");
            Regex durationRegex = new Regex("^[1-9][0-9]*$");

            bool validPeopleCount = peopleCountRegex.IsMatch(PeopleCount) || (PeopleCount.Equals("People count"));
            bool validDuration = durationRegex.IsMatch(Duration) || (Duration.Equals("Duration(h)"));
            bool validLanguage = languageRegex.IsMatch(Language) || (Language.Equals("Language"));
            bool isValid = validDuration && validLanguage && validPeopleCount;

            return isValid;
        }

        public void FilterByPeopleCount(int peopleCount)
        {
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
            location.State = formattedLocation.Split(",")[1];
            location.City = formattedLocation.Split(",")[0];

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

    }
}
