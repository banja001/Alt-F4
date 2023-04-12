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
        private AppointmentCheckPointRepository _appointmentCheckPointRepository { get; set; }
        private TourAttendanceRepository _tourattendanceRepository { get; set; }
        private UserRepository _userRepository { get; set; }
        private AnswerRepository _answerRepository { get; set; }
        public List<Location> Locations { get; set; }
        public ObservableCollection<AppointmentCheckPoint> AppointmentCheckPoints { get; set; }
        public ObservableCollection<TourAttendance> GuestsOnTour { get; set; }
        public Tour SelectedTour { get; set; }
        public User Guide { get; set; }
        public Answer Answer { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Appointment CurrentAppointment { get; set; }
        public LiveTrackingWindow(User guide)
        {
            InitializeComponent();
            this.DataContext = this;
            InitializeRepositories();
            Answer = new Answer();
            SelectedTour= new Tour();   
            AppointmentCheckPoints= new ObservableCollection<AppointmentCheckPoint>();
            GuestsOnTour= new ObservableCollection<TourAttendance>();
            Appointments= _appointmentRepository.FindAll();
            Guide = guide;
            Tours = new ObservableCollection<Tour>();
            Locations = _locationRepository.GetAll();
            ShowTours();
            ShowAppointment();
            FindAppropriateLocation();
            LooksOfDataGrid(ToursDG);
        }

        private void InitializeRepositories()
        {
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _reservationTourRepository = new ReservationTourRepository();
            _appointmentRepository = new AppointmentRepository();
            _checkPointRepository = new CheckPointRepository();
            _userRepository = new UserRepository();
            _tourattendanceRepository = new TourAttendanceRepository();
            _answerRepository = new AnswerRepository();
            _appointmentCheckPointRepository = new AppointmentCheckPointRepository();
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
        private void ShowAppointment()
        {
            foreach (Appointment ap in Appointments)
            {
                if (ap.Active)
                {
                    CurrentAppointment = ap;
                    SelectedTour = ap.Tour;
                    FindAppointmentCheckPoints();
                    FindGuests();
                    StartTourB.IsEnabled = false;
                    CancelTourB.IsEnabled = true;
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
            if (SelectedTour.Id>0 && !string.IsNullOrEmpty(SelectedTour.Name))
            { 
                CreateAppointment();
                FindCheckPoints();
                FindGuests();
                UncheckAll();
                SaveCheckPoint();
                LooksOfDataGrid(GuestsDG);
                SendRequestToTourAttendance();
                StartTourB.IsEnabled = false;
                CancelTourB.IsEnabled = true;
            }
            else
                MessageBox.Show("Select tour, please!");
        }

        private void CreateAppointment()
        {
            DateAndTime EndDate = new DateAndTime(SelectedTour.StartTime.Date, SelectedTour.StartTime.Time);
            EndDate.AddTime(SelectedTour.Duration);
            Appointment appointment = new Appointment(_appointmentRepository.MakeID(), SelectedTour.StartTime, EndDate,
                SelectedTour.Id, Guide.Id, true);
            _appointmentRepository.Add(appointment);
            CurrentAppointment = appointment;
        }

        private void SaveCheckPoint()
        {
            AppointmentCheckPoints[0].Active = true;
            AppointmentCheckPoints[0].NotChecked = false;
            _appointmentCheckPointRepository.SaveOneInFile(AppointmentCheckPoints[0]);
        }

        private void SendRequestToTourAttendance()
        {
            foreach (TourAttendance ta in GuestsOnTour)
            {
                Answer = new Answer(_answerRepository.MakeID(), ta, AppointmentCheckPoints[0], false, true);
                _answerRepository.Add(Answer);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void FindGuests()
        {
            GuestsOnTour.Clear();
            List<ReservationTour> AllReservationTours = _reservationTourRepository.GetAll();
            foreach (ReservationTour rt in AllReservationTours)
            {
                if (rt.Tour.Id == SelectedTour.Id)
                {
                    TourAttendance newTourAttendence= new TourAttendance(_tourattendanceRepository.MakeID() + GuestsOnTour.Count, rt.Id, AppointmentCheckPoints[0].Id,true);
                    newTourAttendence.Guest = AllReservationTours.Find(tr=> tr.Id==newTourAttendence.Guest.Id);
                    newTourAttendence.Guest.User=AllReservationTours.Find(tr => tr.User.Id == newTourAttendence.Guest.User.Id).User;
                    newTourAttendence.Guest.User.Username= _userRepository.GetAll().Find(u => u.Id == newTourAttendence.Guest.User.Id).Username;
                    newTourAttendence.StartedCheckPoint = AppointmentCheckPoints[0];
                    _tourattendanceRepository.Add(newTourAttendence);
                    GuestsOnTour.Add(newTourAttendence);
                }
            }
        }

        public void FindAppointmentCheckPoints() 
        {
            AppointmentCheckPoints.Clear();
            foreach (AppointmentCheckPoint chp in _appointmentCheckPointRepository.FindAll())
            {
                if(chp.AppointmentId==CurrentAppointment.Id)
                    AppointmentCheckPoints.Add( chp );
            }
        }
        public void FindCheckPoints()
        {
            AppointmentCheckPoints.Clear();
            foreach (CheckPoint chp in _checkPointRepository.FindAll())
            {
                if (chp.TourId == CurrentAppointment.Tour.Id)
                {
                    AppointmentCheckPoint apch = new AppointmentCheckPoint(_appointmentCheckPointRepository.MakeID() + AppointmentCheckPoints.Count,chp.Name,false,true,CurrentAppointment.Id,chp.Order);
                    AppointmentCheckPoints.Add(apch);
                }
            }
            List<AppointmentCheckPoint> appointmentCheckPoints = new List<AppointmentCheckPoint>();
            appointmentCheckPoints.AddRange(AppointmentCheckPoints);
            _appointmentCheckPointRepository.AddRange(appointmentCheckPoints);
        }
        public void UncheckAll()
        {
            foreach (AppointmentCheckPoint chp in AppointmentCheckPoints)
            {
                chp.Active = false;
                chp.NotChecked = true;
            }
        }

        private void CheckPointCHBClick(object sender, RoutedEventArgs e)
        {
            CheckBox cb=(CheckBox)sender;
            cb.IsEnabled = false;
            AppointmentCheckPoint chp = (AppointmentCheckPoint) cb.DataContext;
            chp.NotChecked= false;
            chp.Active= true;
            _appointmentCheckPointRepository.SaveOneInFile( chp );
            TourEnd();
        }


        private void CancelTour(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                TourIsOver();
            }
        }
        private void TourEnd()
        {
            if (AppointmentCheckPoints[AppointmentCheckPoints.Count - 1].Active)
            {

                TourIsOver();
            }
            
        }
        private void TourIsOver()
        {
            AppointmentCheckPoints.Clear();
            GuestsOnTour.Clear();
            Appointments.Find(a => a.Id == CurrentAppointment.Id).Active = false;
            Appointments.Find(a => a.Id == CurrentAppointment.Id).End.Date = DateTime.Now;
            Appointments.Find(a => a.Id == CurrentAppointment.Id).End.Time = DateTime.Now.ToString("HH:mm");
            _appointmentRepository.Save(Appointments);
            StartTourB.IsEnabled = true;
            CancelTourB.IsEnabled = false;
            MessageBox.Show("Tour is over!");
        }
    }
    
}
