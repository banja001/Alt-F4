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

namespace booking.View.Guide
{
    /// <summary>
    /// Interaction logic for LiveTrackingWindow.xaml
    /// </summary>
    public partial class LiveTrackingWindow : Window
    {
        public ObservableCollection<Tour> Tours { get; set; } 
        private TourRepository _tourRepository { get; set; }
        private LocationRepository _locationRepository { get; set; }
        private ReservationTourRepository _reservationTourRepository { get; set; }
        private AppointmentRepository _appointmentRepository { get; set; }
        private CheckPointRepository _checkPointRepository { get; set; }
        private TourAttendanceRepository _tourattendanceRepository { get; set; }
        private UserRepository _userRepository { get; set; }
        public List<Location> Locations { get; set; }
        public ObservableCollection<CheckPoint> CheckPoints { get; set; }
        public ObservableCollection<TourAttendance> GuestsOnTour { get; set; }
        public Tour SelectedTour { get; set; }
        public LiveTrackingWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>();
            _locationRepository = new LocationRepository();
            Locations = _locationRepository.GetAllLocations();
            _reservationTourRepository = new ReservationTourRepository();
            _appointmentRepository= new AppointmentRepository();
            _checkPointRepository= new CheckPointRepository();
            _userRepository= new UserRepository();
            _tourattendanceRepository= new TourAttendanceRepository();  
            SelectedTour= new Tour();   
            CheckPoints= new ObservableCollection<CheckPoint>();
            GuestsOnTour= new ObservableCollection<TourAttendance>();
            ShowTours();
            FindAppropriateLocation();
            LooksOfDataGrid(ToursDG);
        }
        private void ShowTours()
        {
            foreach (Tour tour in _tourRepository.FindAll()) 
            {
                if (tour.StartTime.Date.Date == DateTime.Now.Date)
                {
                    Tours.Add(tour);
                }
            }
        }
        private void FindAppropriateLocation()
        {
            foreach (Tour tour in Tours)
            {
                tour.Location.State = Locations.Find(l => l.Id==tour.Location.Id).State;
                tour.Location.City = Locations.Find(l => l.Id == tour.Location.Id).City;
            }
        }

        public void LooksOfDataGrid(DataGrid d)
        {
            for (int i = 0; i < d.Columns.Count; i++)
                d.Columns[i].Width = (d.Width) / d.Columns.Count;
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                FindCheckPoints();
                UncheckAll();
                CheckPoints[0].Active = true;
                CheckPoints[0].NotChecked = false;
                FindGuests();
                LooksOfDataGrid(GuestsDG);
                Appointment appointment=new Appointment();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void FindGuests()
        {
            //ObservableCollection < ReservationTour > guests = new ObservableCollection<ReservationTour>();
            foreach (TourAttendance rt in _tourattendanceRepository.GetAll())
            {
                if (rt.Tour.Id == SelectedTour.Id)
                {
                    rt.User.Username = _userRepository.GetUsers().Find(u=> u.Id==rt.User.Id).Username;
                    GuestsOnTour.Add(rt);
                }
            }
        }

        public void FindCheckPoints() 
        {
            CheckPoints.Clear();
            foreach (CheckPoint chp in _checkPointRepository.FindAll())
            {
                if(chp.TourId==SelectedTour.Id)
                    CheckPoints.Add( chp );
            }
        }
        public void UncheckAll()
        {
            foreach (CheckPoint chp in CheckPoints)
            {
                chp.Active = false;
                chp.NotChecked = true;
            }
        }

        private void CheckPointCHBClick(object sender, RoutedEventArgs e)
        {
            CheckBox cb=(CheckBox)sender;
            cb.IsEnabled = false;
            CheckPoint chp = (CheckPoint) cb.DataContext;
            GuestsOnTour.Clear();
            FindGuests();
            GuestsOnTour[0].StartedCheckPoint.Id = chp.Id;
            
        }

        private void CheckPointsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*CheckPoint chp = (CheckPoint)CheckPointsLB.SelectedItem;
            foreach (CheckPoint cp in CheckPoints)
            {
                if (chp.Id == cp.Id)
                {
                    cp.Active = true;
                    cp.NotChecked = false;
                }
            }*/
        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                CheckPoints.Clear();
                GuestsOnTour.Clear();
            }
        }
    }
}
