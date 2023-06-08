using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using booking.View;
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
using Syncfusion.UI.Xaml;
using System.Windows.Controls;
using System.Reflection.Metadata;
using System.IO;
using System.IO.Pipes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using System.Windows.Markup;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class ReservationsViewModel
    {
        public static ObservableCollection<ReservationAccommodationDTO> ReservationAccommodationDTOs { get; set; }
        public static ObservableCollection<ReservationsRequestsDTO> ReservationRequestsDTOs { get; set; }

        public static ReservationAccommodationDTO SelectedReservation { get; set; }
        public static ReservationsRequestsDTO SelectedReservationRequestDTO { get; set; }

        private readonly ReservationService _reservationService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;
        private readonly ReservationRequestsService _reservationRequestsService;
        private readonly OwnerNotificationsService _ownerNotificationsService;
        private readonly UserService _userService;

        public ICommand PostponeReservationCommand => new RelayCommand(PostponeReservation);
        public ICommand CancelReservationCommand => new RelayCommand(CancelReservation);
        public ICommand ViewCommentCommand => new RelayCommand(ViewComment);
        public ICommand GenerateReportCommand => new RelayCommand(GenerateReport);

        private User user;
        private int userId;
        private Guest1View guest1ViewWindow;

        public ReservationsViewModel(int userId, Guest1View guest1View)
        {
            this.userId = userId;
            this.guest1ViewWindow = guest1View;

            _reservationService = new ReservationService();
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();
            _reservationRequestsService = new ReservationRequestsService();
            _ownerNotificationsService = new OwnerNotificationsService();
            _userService = new UserService();

            user = _userService.GetById(userId);

            InitializeDTOs();
        }

        private void InitializeDTOs()
        {
            ReservationRequestsDTOs = _reservationService.CreateReservationsRequestsDTOs();
            SetReservationAccommodationDTOs();
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
            SetReservationAccommodationDTOs();

            guest1ViewWindow.reservationsData.ItemsSource = ReservationAccommodationDTOs;
            guest1ViewWindow.reservationRequestsData.ItemsSource = ReservationRequestsDTOs;
        }

        private void SetReservationAccommodationDTOs()
        {
            ObservableCollection<ReservationAccommodationDTO> reservationAccommodations = _reservationService.CreateReservationAccommodationDTOs(userId);
            ReservationAccommodationDTOs = new ObservableCollection<ReservationAccommodationDTO>(reservationAccommodations.Where(d => d.StartDate > DateTime.Now));
        }

        private void CancelReservation()
        {
            ReservedDates reservedDate = _reservedDatesService.GetById(SelectedReservation.ReservationId);
            AccommodationLocationDTO accomodation = _accommodationService.CreateAccomodationDTOs().Where(a => a.Id == reservedDate.AccommodationId).ToList()[0];

            bool isMoreThan24H = accomodation.MinDaysToCancel == 0 && (SelectedReservation.StartDate - DateTime.Now).Hours >= 24;
            bool isMoreThanMinDays = accomodation.MinDaysToCancel <= (SelectedReservation.StartDate - DateTime.Now).Days;

            if (isMoreThan24H || isMoreThanMinDays)
            {
                /*
                
                ReservationRequests request = new ReservationRequests(_reservationRequestsService.MakeId(), reservedDate, "Cancel");
                _reservationRequestsService.Add(request);
                */

                reservedDate.DateOfReserving = DateTime.Now;
                _reservationRequestsService.RemoveAllByReservationId(reservedDate.Id);
                _reservedDatesService.AddCanceled(reservedDate); 
                
                int ownerId = _accommodationService.GetById(reservedDate.AccommodationId).OwnerId;

                _reservedDatesService.Delete(reservedDate);

                _ownerNotificationsService.Add(new OwnerNotification(_ownerNotificationsService.MakeId(), ownerId, accomodation, reservedDate, _userService.GetUserNameById(userId)));

                UpdateDataGrids();
                IncreaseScoreOfSuper();

                MessageBox.Show("Your reservation is deleted!");
            }
            else
            {
                MessageBox.Show("You can cancel your reservation only 24h or " + accomodation.MinDaysToCancel + "days before!");
            }
        }

        private void IncreaseScoreOfSuper()
        {
            if (user.Score < 5 && user.Super)
            {
                ++user.Score;
                _userService.Update(user);
            }
        }

        private void ViewComment()
        {
            ReservationRequests reservationRequest = _reservationRequestsService.GetById(SelectedReservationRequestDTO.RequestId);

            if (reservationRequest.isCanceled == RequestStatus.Postponed)
            {
                MessageBox.Show("Your request has been confirmed");
                return;
            }
            else
                if (reservationRequest.isCanceled == RequestStatus.Pending)
            {
                MessageBox.Show("Your request is still pending");
                return;
            }

            if (reservationRequest.Comment == "")
                MessageBox.Show("Owner didn't leave a comment", "Owner's comment");
            else MessageBox.Show(reservationRequest.Comment, "Owner's comment");
        }

        private void GenerateReport()
        {
            var generateReportWindow = new Guest1ReportView(user);
            generateReportWindow.Show();
        }
    }
}
