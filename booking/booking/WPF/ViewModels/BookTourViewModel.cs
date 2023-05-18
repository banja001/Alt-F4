using application.UseCases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Views.Guest2;

namespace WPF.ViewModels
{
    public class BookTourViewModel : BaseViewModel
    {
        private readonly ReservationTourRepository _reservationTourRepository;
        public User User { get; set; }

        private int numberOfGuests;
        public int NumberOfGuests
        {
            get
            {
                return numberOfGuests;
            }
            set
            {
                numberOfGuests = value;
            }
        }
        public int AvailableSpace { get; set; }
        public int AverageGuestAge { get; set; }
        public List<string> VouchersComboBoxSource { get; set; }
        public TourLocationDTO TourForBooking { get; set; }
        public User CurrentUser { get; set; }
        public string SelectedVoucher { get; set; }
        private bool ConfirmButtonFlag { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }
        private readonly VoucherService _voucherService;
        public SearchTourViewModel SearchTourViewModel { get; set; }    
        public ICommand CloseWindowCommand => new RelayCommand(OnCancelBookingButtonClick);
        public ICommand ConfirmBookingCommand { get; set; }
        public BookTourViewModel(TourLocationDTO selectedTour, User user, SearchTourViewModel searchTourViewModel)
        {
            _voucherService = new VoucherService();
            ConfirmBookingCommand = new RelayCommand(OnConfirmBookingButtonClick);
            _voucherService.GenerateNewVouchersByGuest2(user);
            VouchersComboBoxSource = new List<string>();
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetUsableVouchersByGuest2(user));
            User = user;
            InitializeVoucherComboBox();
            this.TourForBooking = selectedTour;
            _reservationTourRepository = new ReservationTourRepository();
            CurrentUser = user;
            int takenSpace = _reservationTourRepository.GetNumberOfGuestsForTourId(TourForBooking.Id);
            AvailableSpace = TourForBooking.MaxGuests - takenSpace;
            ConfirmButtonFlag = false;
            SearchTourViewModel = searchTourViewModel;
        }
        private void OnCancelBookingButtonClick()
        {
            this.CloseCurrentWindow();
        }
        private void OnConfirmBookingButtonClick()
        {
            if (CheckAvailability())
            {
                int usedVoucherId = -1;
                if (SelectedVoucher != null)
                {
                    Voucher usedVoucher = Vouchers.ToList().Find(v => v.Id.ToString() == SelectedVoucher);
                    usedVoucher.IsUsed = true;
                    _voucherService.Update(usedVoucher);
                    usedVoucherId = usedVoucher.Id;
                }
                if (!CanConfirmBookingButtonClick())
                {
                    MessageBox.Show("Number of guests and Average guest age needs to be a number!", "Alert");
                    return;
                }
                    
                ReservationTour reservation = new ReservationTour(_reservationTourRepository.GetNextIndex(),
                                                                  TourForBooking.Id,
                                                                  CurrentUser.Id,
                                                                  NumberOfGuests,
                                                                  AverageGuestAge,
                                                                  usedVoucherId);
                _reservationTourRepository.Add(reservation);
                MessageBox.Show("Tour reserved successfully!", "Success");

                ConfirmButtonFlag = true;
                this.CloseCurrentWindow();
            }
            else
            {
                var result = MessageBox.Show("There's currently not enough space to reserve the selected tour," +
                                             "do you want to see other options on same location?", "Error message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SearchTourViewModel.FilterByLocation(TourForBooking.Location);
                    SearchTourViewModel.FilterByPeopleCount(NumberOfGuests);
                    ConfirmButtonFlag = true;
                    CloseCurrentWindow();
                }
            }
        }
        private bool CanConfirmBookingButtonClick()
        {
            Regex numberOfGuestsRegex = new Regex("^[1-9][0-9]*$");
            bool isInvalidNumberOfGuests = string.IsNullOrEmpty(NumberOfGuests.ToString()) || NumberOfGuests.ToString().Equals("0") || !numberOfGuestsRegex.IsMatch(NumberOfGuests.ToString());

            Regex averageGuestAgeRegex = new Regex("^[1-9][0-9]*$");
            bool isInvalidAverageGuestAge = string.IsNullOrEmpty(AverageGuestAge.ToString()) || AverageGuestAge.ToString().Equals("0") || !averageGuestAgeRegex.IsMatch(AverageGuestAge.ToString());
            return !isInvalidAverageGuestAge && !isInvalidNumberOfGuests;
        }
        private void InitializeVoucherComboBox()
        {
            foreach (var voucher in Vouchers)
            {
                VouchersComboBoxSource.Add("Voucher " + voucher.Id.ToString());
            }
            OnPropertyChanged(nameof(VouchersComboBoxSource));
        }
        private bool CheckAvailability()
        {
            int currentGuestNumber = _reservationTourRepository.GetNumberOfGuestsForTourId(TourForBooking.Id);
            bool isAvailable = currentGuestNumber + NumberOfGuests > TourForBooking.MaxGuests ? false : true;
            return isAvailable;
        }
    }
}
