﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
using booking.Repository;

namespace booking.application.UseCases
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;  
        private readonly TourRepository _tourRepository;
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly TourAttendanceRepository _tourAttendanceRepository;
        private readonly LocationRepository _locationRepository;
        private readonly GuideRatingRepository _guideRatingRepository;
        public AppointmentService()
        {
            _appointmentRepository = new AppointmentRepository();
            _tourRepository = new TourRepository();
            _reservationTourRepository= new ReservationTourRepository();
            _locationRepository= new LocationRepository();
            _guideRatingRepository= new GuideRatingRepository();
            _tourAttendanceRepository = new TourAttendanceRepository();
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
                    AppointmentGuestsDTO FinishedTour=new AppointmentGuestsDTO(appointment.Tour.Name,appointment.Tour.Location,appointment.Tour.Language,appointment.Start,guestsNumber,appointment.Id);
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

        public Appointment FindAppointment(int appointmentId)
        {
            Appointment appointment =
                _appointmentRepository.FindAll().Find(a => a.Id == appointmentId);
            
            return appointment;
        }

        public List<GuideRating> FindComments(Appointment appointment)
        {
            List<GuideRating>reviews=new List<GuideRating>();
            foreach (GuideRating ratings in _guideRatingRepository.GetAll())
            {
                reviews.Add(ratings);
            }

            return reviews.FindAll(r => r.AppointmentId == appointment.Id);
        }


        public List<TourRatingDTO> MakeTourRatings(ObservableCollection<GuideRating> guideRatings, AppointmentGuestsDTO appointment )
        {
            List<TourRatingDTO> ratings = new List<TourRatingDTO>();
            foreach (var guideRating in guideRatings)
            {
                TourRatingDTO tourRating = new TourRatingDTO();
                tourRating.Rating = guideRating;
                tourRating.TourName=appointment.Name;
                tourRating.AppointmentId=appointment.AppointmentId;
                //tourRating.CheckPoint = _tourAttendanceRepository.GetAll()
                //  .Find(ta => ta.Guest.Tour.Id == _appointmentRepository.FindAll().Find(a=>a.Id == appointment.AppointmentId).Tour.Id).StartedCheckPoint.Id.ToString();
                tourRating.CheckPoint = _tourAttendanceRepository.GetAll().Find(ta => ta.Guest.Id == tourRating.Rating.GuestId).StartedCheckPoint.Id.ToString();
                ratings.Add(tourRating); 
            }

            return ratings;
        }
    }
}
