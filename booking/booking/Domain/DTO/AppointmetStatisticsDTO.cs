using System;
using System.Collections.Generic;
using System.Text;
using booking;
using booking.Model;
using booking.Repository;

namespace Domain.DTO
{
    public class AppointmentStatisticsDTO
    {
        public string TourName { get; set; }
        public int TotalGuests { get; set; }
        public int GuestsUnder18 { get; set;}
        public int GuestsBetween18And50 { get; set;}
        public int GuestsAbove50 { get; set;}
        public double GuestsWithVoucher { get; set; }
        public double GuestsWithoutVoucher { get; set; }
        public AppointmentStatisticsDTO(string tourName, int totalGuests, int guestsUnder18, int guestsBetween18And50, int guestsAbove50, double guestsWithVoucher, double guestsWithoutVoucher)
        {
            TourName = tourName;
            TotalGuests = totalGuests;
            GuestsUnder18 = guestsUnder18;
            GuestsBetween18And50 = guestsBetween18And50;
            GuestsAbove50 = guestsAbove50;
            GuestsWithVoucher = guestsWithVoucher;
            GuestsWithoutVoucher = guestsWithoutVoucher;
        }

        public AppointmentStatisticsDTO()
        {
        }

        public void CalculateGuestsUnder18(List<TourAttendance> tourAttendances, List<ReservationTour> reservation, int appId)
        {
            int numberOfGuests=0;
            foreach (TourAttendance tourAttendant in tourAttendances)
            {
                tourAttendant.Guest = reservation.Find(res=>res.Id==tourAttendant.Guest.Id);
                if(tourAttendant.Guest != null && tourAttendant.Guest.AverageGuestAge<18 && appId==tourAttendant.StartedCheckPoint.AppointmentId)
                    numberOfGuests+=tourAttendant.Guest.NumberOfGuests;
            }

            this.GuestsUnder18 = numberOfGuests;
        }
        public void CalculateGuestsBetween18And50(List<TourAttendance> tourAttendances, List<ReservationTour> reservation, int appId)
        {
            int numberOfGuests = 0;
            foreach (TourAttendance tourAttendant in tourAttendances)
            {
                tourAttendant.Guest = reservation.Find(res => res.Id == tourAttendant.Guest.Id);
                if (tourAttendant.Guest != null && tourAttendant.Guest.AverageGuestAge >= 18 && tourAttendant.Guest.AverageGuestAge<= 50 && appId == tourAttendant.StartedCheckPoint.AppointmentId)
                    numberOfGuests += tourAttendant.Guest.NumberOfGuests;
            }

            this.GuestsBetween18And50 = numberOfGuests;
        }
        public void CalculateGuestsAbove50(List<TourAttendance> tourAttendances, List<ReservationTour> reservation, int appId)
        {
            int numberOfGuests = 0;
            foreach (TourAttendance tourAttendant in tourAttendances)
            {
                tourAttendant.Guest = reservation.Find(res => res.Id == tourAttendant.Guest.Id && appId == tourAttendant.StartedCheckPoint.AppointmentId);
                if(tourAttendant.Guest != null && tourAttendant.Guest.AverageGuestAge > 50)
                    numberOfGuests += tourAttendant.Guest.NumberOfGuests;
            }

            this.GuestsAbove50 = numberOfGuests;
        }

        public void CalculateNumberOfGuestsWithVoucher(List<TourAttendance> tourAttendances,
            List<ReservationTour> reservation, int appId)
        {
            int numberOfGuests = 0;
            foreach (TourAttendance tourAttendant in tourAttendances)
            {
                if (tourAttendant.Guest != null)
                {
                    tourAttendant.Guest = reservation.Find(res =>
                        res.Id == tourAttendant.Guest.Id && appId == tourAttendant.StartedCheckPoint.AppointmentId);
                    if (tourAttendant.Guest != null && tourAttendant.Guest.VoucherId > 0)
                        numberOfGuests += tourAttendant.Guest.NumberOfGuests;
                }
            }

            this.GuestsWithVoucher = Math.Round(100 * (double)numberOfGuests / (double)TotalGuests);
            this.GuestsWithoutVoucher = 100 - GuestsWithVoucher;
        }
    }
}
