using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Domain.RepositoryInterfaces;
using booking.DTO;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using Domain.DTO;
using Domain.RepositoryInterfaces;

namespace booking.application.UseCases
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IReservationTourRepository _reservationTourRepository;
        private readonly ITourAttendanceRepository _tourAttendanceRepository;
        private readonly LocationRepository _locationRepository;
        private readonly IGuideRatingRepository _guideRatingRepository;
        private readonly AppointmentCheckPointRepository _appointmentCheckPointRepository;
        private readonly UserRepository _userRepository;

        public AppointmentService()
        {
            _appointmentRepository = Injector.Injector.CreateInstance<IAppointmentRepository>();
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            _reservationTourRepository = Injector.Injector.CreateInstance<IReservationTourRepository>();
            _locationRepository = new LocationRepository();
            _guideRatingRepository = Injector.Injector.CreateInstance<IGuideRatingRepository>();
            _tourAttendanceRepository = Injector.Injector.CreateInstance<ITourAttendanceRepository>();
            _appointmentCheckPointRepository = new AppointmentCheckPointRepository();
            _userRepository= new UserRepository();
        }

        public int FindNumberOfGuests(int tourID)
        {
            int numberOfGuests = 0;
            foreach (var rt in _reservationTourRepository.GetAll())
            {
                if (rt.Tour.Id == tourID)
                    numberOfGuests += rt.NumberOfGuests;
            }

            return numberOfGuests;
        }

        public List<AppointmentGuestsDTO> CreateListOfFinishedTours(int userID)
        {
            List<AppointmentGuestsDTO> FinishedTours = new List<AppointmentGuestsDTO>();
            int guestsNumber;
            foreach (var appointment in _appointmentRepository.FindAll())
            {
                if (userID == appointment.Guide.Id && !appointment.Active)
                {
                    guestsNumber = FindNumberOfGuests(appointment.Tour.Id);
                    appointment.Tour = _tourRepository.FindAll().Find(t => t.Id == appointment.Tour.Id);
                    appointment.Tour.Location =_locationRepository.GetAll().Find(l => l.Id == appointment.Tour.Location.Id);
                    AppointmentGuestsDTO FinishedTour = new AppointmentGuestsDTO(appointment.Tour.Name,appointment.Tour.Location, appointment.Tour.Language, appointment.Start, guestsNumber, appointment.Id);
                    FinishedTours.Add(FinishedTour);
                }

            }

            return FinishedTours;
        }

        public AppointmentGuestsDTO FindMostVisitedTour(int userID, string year)
        {
            List<AppointmentGuestsDTO> allTours = CreateListOfFinishedTours(userID);
            int maxNumberOfGuests = allTours.Max(t => t.NumberOfGuests);
            AppointmentGuestsDTO mostVisitedTour = allTours.Find(t =>
                t.NumberOfGuests == maxNumberOfGuests && t.StartTime.Date.Year.ToString() == year);
            return mostVisitedTour;
        }

        public ObservableCollection<int> FindAllYears(int userID)
        {
            List<int> years = new List<int>();
            List<AppointmentGuestsDTO> allTours = CreateListOfFinishedTours(userID);
            foreach (var tours in allTours)
            {
                years.Add(tours.StartTime.Date.Year);
            }

            return DistinctYears(years);
        }

        private static ObservableCollection<int> DistinctYears(List<int> years)
        {
            IEnumerable<int> tempDistinct = years.Distinct();

            ObservableCollection<int> distinctYears = new ObservableCollection<int>();
            foreach (var temp in tempDistinct)
            {
                distinctYears.Add(temp);
            }

            years.Clear();
            return distinctYears;
        }

        public Appointment FindAppointment(int appointmentId)
        {
            Appointment appointment =
                _appointmentRepository.FindAll().Find(a => a.Id == appointmentId);

            return appointment;
        }

        public List<GuideRating> FindComments(Appointment appointment)
        {
            List<GuideRating> reviews = new List<GuideRating>();
            foreach (GuideRating ratings in _guideRatingRepository.GetAll())
            {
                reviews.Add(ratings);
            }

            return reviews.FindAll(r => r.AppointmentId == appointment.Id);
        }

        public string GetName(int id)
        {
            foreach (Appointment app in _appointmentRepository.FindAll())
            {
                if (app.Id == id)
                    return _tourRepository.FindById(app.Tour.Id).Name;
            }
            return null;
        }

        public List<TourRatingDTO> MakeTourRatings(ObservableCollection<GuideRating> guideRatings,
            AppointmentGuestsDTO appointment)
        {
            List<TourRatingDTO> ratings = new List<TourRatingDTO>();
            foreach (var guideRating in guideRatings)
            {
                TourRatingDTO tourRating = new TourRatingDTO();
                tourRating.Rating = guideRating;
                tourRating.TourName = appointment.Name;
                tourRating.AppointmentId = appointment.AppointmentId;
                int checkPointId = _tourAttendanceRepository.GetAll().Find(ta => ta.Guest.Id == tourRating.Rating.GuestId).StartedCheckPoint.Id;
                tourRating.CheckPoint = _appointmentCheckPointRepository.FindAll().Find(cp => cp.Id == checkPointId).Name;
                tourRating.GuestName = _userRepository.GetAll().Find(cp => cp.Id == tourRating.Rating.GuestId).Username;
                tourRating.CalculateAverageRating();
                tourRating.Rating = guideRating;
                ratings.Add(tourRating);
            }

            return ratings;
        }

        public void Update(Appointment appointment)
        {
                _appointmentRepository.Upadte(appointment);
        }

        public List<Appointment> GetCompletedAppointmentByGuest2(User guest2)
        {
            List<ReservationTour> reservedTours =
            _reservationTourRepository.GetAll().FindAll(r => r.User.Id == guest2.Id);
            List<Appointment> appointments = _appointmentRepository.FindAll().FindAll(a => !a.IsRated);
            List<TourAttendance> attendances =
            _tourAttendanceRepository.GetAll().FindAll(a => a.Guest.User.Id == guest2.Id);

            var completedAppointments = GetAllCompletedAppointments(reservedTours, appointments);

            return GetVisitedAppointments(attendances, completedAppointments);
        }
        public AppointmentStatisticsDTO MakeAppointmentStatisticsDTO(int appId )
        {
            List<TourAttendance> tourAttendances = _tourAttendanceRepository.GetAll();
            FindCheckPoint(tourAttendances);
            FindUsers(tourAttendances);
            List<ReservationTour> reservation = _reservationTourRepository.GetAll();
            AppointmentStatisticsDTO appointmentStatistics = new AppointmentStatisticsDTO();
            CalculateStatistics(appId, appointmentStatistics, tourAttendances, reservation);
            appointmentStatistics.TourName = FindTourName(appId);
            return appointmentStatistics;
        }

        private void CalculateStatistics(int appId, AppointmentStatisticsDTO appointmentStatistics, List<TourAttendance> tourAttendances,
            List<ReservationTour> reservation)
        {
            appointmentStatistics.TotalGuests = FindNumberOfGuestsInAppointment(tourAttendances, appId);
            appointmentStatistics.CalculateGuestsUnder18(tourAttendances, reservation, appId);
            appointmentStatistics.CalculateGuestsBetween18And50(tourAttendances, reservation, appId);
            appointmentStatistics.CalculateGuestsAbove50(tourAttendances, reservation, appId);
            appointmentStatistics.CalculateNumberOfGuestsWithVoucher(tourAttendances, reservation, appId);
        }

        public int FindNumberOfGuestsInAppointment(List<TourAttendance> tourAttendances, int appId)
        {
            int numberOfGuests = 0;
            foreach (var ta in tourAttendances)
            {
                if (ta.StartedCheckPoint.AppointmentId ==appId )
                    numberOfGuests += ta.Guest.NumberOfGuests;
            }

            return numberOfGuests;
        }
        public void FindUsers(List<TourAttendance> tourAttendances)
        {
            foreach (var ta in tourAttendances)
            {
                if(ta.Guest!=null)
                    ta.Guest=_reservationTourRepository.GetAll().Find(rtr=>rtr.Id==ta.Guest.Id);
            }

        }
        public void FindCheckPoint(List<TourAttendance> tourAttendances)
        {
            foreach (var ta in tourAttendances)
            {
                ta.StartedCheckPoint = _appointmentCheckPointRepository.FindAll()
                    .Find(cha => cha.Id == ta.StartedCheckPoint.Id);
            }

        }

        public string FindTourName(int appId)
        {
            Appointment selectedAppointment = _appointmentRepository.FindAll().Find(ap => ap.Id == appId);
            return _tourRepository.FindById(selectedAppointment.Tour.Id).Name;
        }

        public List<Appointment> GetAllCompletedAppointments(List<ReservationTour> reservedTours, List<Appointment> appointments)
        {
            var completedAppointments = new List<Appointment>();

            foreach (var reservedTour in reservedTours)
            {
                completedAppointments.AddRange(appointments.FindAll(a =>(reservedTour.Tour.Id == a.Tour.Id) && !a.Active));
                completedAppointments = completedAppointments.Distinct().ToList();
            }
            return completedAppointments;
        }

        public List<Appointment> GetVisitedAppointments(List<TourAttendance> attendances, List<Appointment> completedAppointments)
        {
            foreach (var attendance in attendances)
            {
                Appointment visitedAppointment =
                completedAppointments.Find(c => c.Tour.Id == attendance.Guest.Tour.Id);

                if ((visitedAppointment != null) && attendance.Appeared)
                    continue;
                completedAppointments.Remove(visitedAppointment);
            }

            return completedAppointments;
        }
        public List<Appointment> GetActiveAppointmentByGuest2(User guest2)
        {
            var activeAppointments = new List<Appointment>();
            var reservedTours = _reservationTourRepository.GetAll().FindAll(r => r.User.Id == guest2.Id);
            List<Appointment> appointments = _appointmentRepository.FindAll();

            foreach (var reservedTour in reservedTours)
            {
                activeAppointments.AddRange(appointments.FindAll(a => (reservedTour.Tour.Id == a.Tour.Id) && a.Active));
                activeAppointments = activeAppointments.Distinct().ToList();
            }
            return activeAppointments;
        }
        public List<TourLocationDTO> MakeToursFrom(List<Appointment> appointments)
        {
            List<TourLocationDTO> tours = new List<TourLocationDTO>();
            foreach(var appointment in appointments)
            {
                Tour activeTour = _tourRepository.FindAll().Find(t => t.Id == appointment.Tour.Id);
                Location location = _locationRepository.GetById(appointment.Tour.Location.Id);
                tours.Add(new TourLocationDTO(appointment, activeTour, location));
            }
            return tours;
        }
        public List<Appointment> FindAllAppointmentsByGuide(int id)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach(var app in _appointmentRepository.FindAll())
            {
                if (app.Guide.Id == id && app.Start.Date.AddYears(2)>DateTime.Now)
                {
                    app.Tour = _tourRepository.FindById(app.Tour.Id);
                    appointments.Add(app);
                }
            }
            return appointments;
        }

        public string GroupByLanguage(List<Appointment> appointments)
        {
            var languages = appointments.GroupBy(a => a.Tour.Language).Select(group => new { LanguageId = group.Key, LanguagesCount = group.Count() });
            var mostSpokenLanguage = languages.OrderByDescending(l => l.LanguagesCount).FirstOrDefault();

            return mostSpokenLanguage.LanguageId;
        }
        public bool IsAbove20(List<Appointment> appointments,string language)
        {
            int languageCounter = 0;
            float overallRating = 0;
            int ratingCounter = 0;
            
            foreach(var appointment in appointments) 
            {
                if (appointment.Tour.Language == language)
                {
                    if (_guideRatingRepository.GetByAppointmentId(appointment.Id) != null)
                    {
                        overallRating += _guideRatingRepository.GetByAppointmentId(appointment.Id).LanguageKnowledge;
                        ratingCounter++;
                    }
                    languageCounter++;
                }
            }
            return languageCounter > 19 && overallRating/ratingCounter>=4;
        }
    }
    
}
