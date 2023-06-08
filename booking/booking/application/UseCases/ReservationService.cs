using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repository;
using booking.WPF.Views.Guest1;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace booking.application.usecases
{
    public class ReservationService
    {
        private readonly IReservedDatesRepository _reservedDatesRepository;
        private readonly IReservationRequestsRepository _reservationRequestsRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;

        public ReservationService()
        {
            _reservedDatesRepository = Injector.Injector.CreateInstance<IReservedDatesRepository>();
            _reservationRequestsRepository = Injector.Injector.CreateInstance<IReservationRequestsRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
        }

        public ObservableCollection<ReservationsRequestsDTO> CreateReservationsRequestsDTOs()
        {
            List<ReservationRequests> reservationRequests = _reservationRequestsRepository.GetAll();
            ObservableCollection<ReservationsRequestsDTO> reservationRequestsDTOs = new ObservableCollection<ReservationsRequestsDTO>();

            foreach (var request in reservationRequests)
            {
                ReservedDates reservedDate = _reservedDatesRepository.GetById(request.ReservationId);
                Accommodation accommodation = _accommodationRepository.GetById(reservedDate.AccommodationId);
                Location location = _locationRepository.GetById(accommodation.LocationId);

                reservationRequestsDTOs.Add(new ReservationsRequestsDTO(accommodation, location, "Postpone", request.isCanceled.ToString(), request.Id));
            }

            return reservationRequestsDTOs;
        }

        public ObservableCollection<ReservationAccommodationDTO> CreateReservationAccommodationDTOs(int userId)
        {
            List<ReservedDates> reservedDates = _reservedDatesRepository.GetAll().Where(d => d.UserId == userId).ToList();
            ObservableCollection<ReservationAccommodationDTO> reservationAccommodationDTOs = new ObservableCollection<ReservationAccommodationDTO>();

            foreach (var date in reservedDates)
            {
                Accommodation accommodation = _accommodationRepository.GetById(date.AccommodationId);
                Location location = _locationRepository.GetById(accommodation.LocationId);

                reservationAccommodationDTOs.Add(new ReservationAccommodationDTO(accommodation, location, date, date.DateOfReserving));
            }

            return reservationAccommodationDTOs;
        }
    }
}
