using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Domain.DTO;
using booking.Model;
using booking.Repository;

namespace booking.application.UseCases
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;  
        private readonly TourRepository _tourRepository;
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly LocationRepository _locationRepository;
        public AppointmentService()
        {
            _appointmentRepository = new AppointmentRepository();
            _tourRepository = new TourRepository();
            _reservationTourRepository= new ReservationTourRepository();
            _locationRepository= new LocationRepository();
        }

        public int FindNumberOfGuests(int tourID)
        {
            int numberOfGuests = 0;
            foreach (var rt in _reservationTourRepository.GetAll())
            {
                if(rt.Tour.Id==tourID)
                    numberOfGuests+=rt.NumberOfGuests;
            }
            return numberOfGuests;
        }

        public List<AppointmentGuestsDTO> CreateListOfFinishedTours(int userID) 
        {
            List<AppointmentGuestsDTO> FinishedTours=new List<AppointmentGuestsDTO>();
            int guestsNumber;
            foreach (var appointment in _appointmentRepository.FindAll())
            {
                if (userID == appointment.Guide.Id && !appointment.Active)
                {
                    guestsNumber = FindNumberOfGuests(appointment.Tour.Id);
                    appointment.Tour = _tourRepository.FindAll().Find(t=>t.Id==appointment.Tour.Id);
                    appointment.Tour.Location =
                        _locationRepository.GetAll().Find(l => l.Id == appointment.Tour.Location.Id);
                    AppointmentGuestsDTO FinishedTour=new AppointmentGuestsDTO(appointment.Tour.Name,appointment.Tour.Location,appointment.Tour.Language,appointment.Start,guestsNumber);
                    FinishedTours.Add(FinishedTour);
                }

            }

            return FinishedTours;
        }

        public AppointmentGuestsDTO FindMostVisitedTour(int userID, string year)
        {
            List<AppointmentGuestsDTO> allTours= CreateListOfFinishedTours(userID);
            int maxNumberOfGuests = allTours.Max(t => t.NumberOfGuests);
            AppointmentGuestsDTO mostVisitedTour = allTours.Find(t => t.NumberOfGuests == maxNumberOfGuests && t.StartTime.Date.Year.ToString()==year);
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
    }
}
