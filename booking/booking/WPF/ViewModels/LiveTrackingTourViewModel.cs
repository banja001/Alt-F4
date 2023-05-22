using application.UseCases;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using booking.Commands;
using booking.WPF.ViewModels;

namespace WPF.ViewModels
{
    internal class LiveTrackingTourViewModel: BaseViewModel
    {
        public ObservableCollection<Tour> Tours { get; set; }
        private TourRepository _tourRepository { get; set; }
        private LocationRepository _locationRepository { get; set; }
        private ReservationTourRepository _reservationTourRepository { get; set; }
        private AppointmentRepository _appointmentRepository { get; set; }
        private CheckPointRepository _checkPointRepository { get; set; }
        private AppointmentCheckPointRepository _appointmentCheckPointRepository { get; set; }
        private TourAttendanceRepository _tourattendanceRepository { get; set; }

        private AppointmentCheckpointService _appointmentCheckpointService;
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
        public ICommand StartCommand => new RelayCommand(StartTour, CanStart);
        public ICommand CancelCommand=> new RelayCommand(CancelTour, CanCancel);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public bool CheckBox { get; set; }
        

        public LiveTrackingTourViewModel(User guide)
        {
            InitializeRepositories();
            Answer = new Answer();
            //SelectedTour = new Tour();
            //CheckBox=new CheckBox();
            AppointmentCheckPoints = new ObservableCollection<AppointmentCheckPoint>();
            GuestsOnTour = new ObservableCollection<TourAttendance>();
            Appointments = _appointmentRepository.FindAll();
            Guide = guide;
            Tours = new ObservableCollection<Tour>();
            Locations = _locationRepository.GetAll();
            ShowTours();
            ShowAppointment();
            FindAppropriateLocation();
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
            _appointmentCheckpointService = new AppointmentCheckpointService();
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

        public bool CanStart()
        {
            return SelectedTour != null && !CanCancel();
        }

        public bool CanCancel()
        {
            return CurrentAppointment!=null? CurrentAppointment.Active:false;
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
                }
            }
        }
        private void FindAppropriateLocation()
        {
            foreach (Tour tour in Tours)
            {
                tour.Location.State = Locations.Find(l => l.Id == tour.Location.Id).State;
                tour.Location.City = Locations.Find(l => l.Id == tour.Location.Id).City;
            }
        }

        

        private void StartTour()
        {
            if (SelectedTour.Id > 0 && !string.IsNullOrEmpty(SelectedTour.Name))
            {
                CreateAppointment();
                FindCheckPoints();
                FindGuests();
                UncheckAll();
                SaveCheckPoint();
                //SendRequestToTourAttendance();
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

        private void Exit()
        {
            this.CloseCurrentWindow();
        }
        public void FindGuests()
        {
            GuestsOnTour.Clear();
            List<TourAttendance> allAttendances = _tourattendanceRepository.GetAll();
            foreach (var ta in allAttendances)
            {
                ta.Guest = _reservationTourRepository.GetAll().Find(rt => rt.Id == ta.Guest.Id);
                ta.Guest.User = _userRepository.GetAll().Find(u => u.Id == ta.Guest.User.Id);
                ta.StartedCheckPoint = _appointmentCheckPointRepository.FindAll()
                    .Find(ach => ach.Id == ta.StartedCheckPoint.Id);
            }

            foreach (var ta in allAttendances)
            {
                if (ta.Guest.Tour.Id == CurrentAppointment.Tour.Id)
                    GuestsOnTour.Add(ta);
            }
        }

        public void FindAppointmentCheckPoints()
        {
            AppointmentCheckPoints.Clear();
            foreach (AppointmentCheckPoint chp in _appointmentCheckPointRepository.FindAll())
            {
                if (chp.AppointmentId == CurrentAppointment.Id)
                    AppointmentCheckPoints.Add(chp);
            }
        }
        public void FindCheckPoints()
        {
            AppointmentCheckPoints.Clear();
            foreach (CheckPoint chp in _checkPointRepository.FindAll())
            {
                if (chp.TourId == CurrentAppointment.Tour.Id)
                {
                    AppointmentCheckPoint apch = new AppointmentCheckPoint(_appointmentCheckPointRepository.MakeID() + AppointmentCheckPoints.Count, chp.Name, false, true, CurrentAppointment.Id, chp.Order);
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

        /*private void CheckPointCHBClick()
        {
            CheckBox cb = (CheckBox)sender;
            cb.IsEnabled = false;
            AppointmentCheckPoint chp = (AppointmentCheckPoint)cb.DataContext;
            chp.NotChecked = false;
            chp.Active = true;
            _appointmentCheckPointRepository.SaveOneInFile(chp);
            TourEnd();
        }*/

        

        private void CancelTour()
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
            MessageBox.Show("Tour is over!","Tour Over", MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
