﻿using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using booking.WPF.Views.Guest1;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ReservationsViewModel
    {
        public static ObservableCollection<ReservationAccommodationDTO> ReservationAccommodationDTOs { get; set; }
        public static ObservableCollection<ReservationsRequestsDTO> ReservationRequestsDTOs { get; set; }

        public static ReservationAccommodationDTO SelectedReservation { get; set; }

        private readonly ReservationService _reservationService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;
        private readonly ReservationRequestsService _reservationRequestsService;
        private readonly OwnerNotificationsService _ownerNotificationsService;
        private readonly UserService _userService;

        public ICommand PostponeReservationCommand => new RelayCommand(PostponeReservation);
        public ICommand CancelReservationCommand => new RelayCommand(CancelReservation);

        private int userId;

        public ReservationsViewModel(int userId)
        {
            this.userId = userId;

            _reservationService = new ReservationService();
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();
            _reservationRequestsService = new ReservationRequestsService();
            _ownerNotificationsService = new OwnerNotificationsService();
            _userService = new UserService();

            InitializeDTOs();
        }

        private void InitializeDTOs()
        {
            ReservationRequestsDTOs = _reservationService.CreateReservationsRequestsDTOs();
            ReservationAccommodationDTOs = _reservationService.CreateReservationAccommodationDTOs(userId);
        }

        private void PostponeReservation()
        {
            PostponeReservation postponeReservation = new PostponeReservation(_reservedDatesService.GetById(SelectedReservation.ReservationId));
            postponeReservation.ShowDialog();

            UpdateDataGrids();
        }

        private void UpdateDataGrids()
        {
            ReservationRequestsDTOs = _reservationService.CreateReservationsRequestsDTOs();
            ReservationAccommodationDTOs = _reservationService.CreateReservationAccommodationDTOs(userId);
        }

        private void CancelReservation()
        {
            ReservedDates reservedDate = _reservedDatesService.GetById(SelectedReservation.ReservationId);
            AccommodationLocationDTO accomodation = _accommodationService.CreateAccomodationDTOs().Where(a => a.Id == reservedDate.AccommodationId).ToList()[0];

            bool isMoreThan24H = accomodation.MinDaysToCancel == 0 && (SelectedReservation.StartDate - DateTime.Now).Hours >= 24;
            bool isMoreThanMinDays = accomodation.MinDaysToCancel <= (SelectedReservation.StartDate - DateTime.Now).Days;

            if (isMoreThan24H || isMoreThanMinDays)
            {
                _reservedDatesService.Delete(reservedDate);
                _reservationRequestsService.RemoveAllByReservationId(reservedDate.Id);

                UpdateDataGrids();
                int ownerId = _accommodationService.GetById(reservedDate.AccommodationId).OwnerId;
                _ownerNotificationsService.Add(new OwnerNotification(_ownerNotificationsService.MakeId(), ownerId, accomodation, reservedDate, _userService.GetUserNameById(userId)));

                MessageBox.Show("Your reservation is deleted!");
            }
            else
            {
                MessageBox.Show("You can cancle your reservation only 24h or " + accomodation.MinDaysToCancel + "days before!");
            }
        }
    }
}