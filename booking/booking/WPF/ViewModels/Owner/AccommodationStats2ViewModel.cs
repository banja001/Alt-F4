using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.Domain.Model;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class AccommodationStats2ViewModel : BaseViewModel
    {
        
        public ICommand MostBookedTooltipCommand => new RelayCommand(MostBookedLabelClick);

        private bool mostBookedTooltip = false;
        public bool MostBookedTooltip
        {
            get
            {
                return mostBookedTooltip;
            }
            set
            {
                if (value != mostBookedTooltip)
                {
                    mostBookedTooltip = value;
                    OnPropertyChanged("MostBookedTooltip");
                }
            }
        }

        private void MostBookedLabelClick()
        {
            if (GlobalVariables.tt == true)
            {
                if (mostBookedTooltip)
                {
                    MostBookedTooltip = false;

                }
                else
                {
                    MostBookedTooltip = true;

                }
            }
        }


        public OwnerViewModel ownerViewModel;
        private int accommodationId;
        public List<int> YearList { get; set; }
        
        public List<string> Months =new List<string>{"January","February","March","April","May","June","July","August","September","October","November","December"};

        private AccommodationYearlyStatsDTO selectedItem;
        public AccommodationYearlyStatsDTO SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    DatagridSelectionChange();
                }
            }
        }
        private int NumberOfReservations;
        private int CanceledReservations;
        private int PostponedReservations;
        private int RenovationRequests;
        ReservationRequestsService res;
        public List<ReservedDates> reservedDates;
        public ObservableCollection<AccommodationYearlyStatsDTO> DatagridYearList { get; set; }
        public ObservableCollection<AccommodationMonthlyStatsDTO> DatagridMonthList { get; set; }

        private int maxYear;
        public int MaxYear {
            get
            {
                return maxYear;
            }
            set
            {
                if (value != maxYear)
                {
                    maxYear = value;
                    OnPropertyChanged("MaxYear");
                    
                }
            }
        }
        OwnerRatingService _service;

        public AccommodationStats2ViewModel(int accId,OwnerViewModel ownerViewModel)
        {
            accommodationId = accId;
            this.ownerViewModel = ownerViewModel;
            YearList = new List<int>();
            res = new ReservationRequestsService();
            for (int i = 2018; i <= DateTime.Now.Year; i++)
            {
                YearList.Add(i);
            }

            reservedDates = ownerViewModel.reservedDatesService.GetAll();
            DatagridYearList = new ObservableCollection<AccommodationYearlyStatsDTO>();
            DatagridMonthList = new ObservableCollection<AccommodationMonthlyStatsDTO>();
            int max = -1;
            MaxYear = -1;
            int maxTemp = 0;
            _service = new OwnerRatingService();
            FindMaxYear(ownerViewModel, ref max, ref maxTemp);
        }

        private void FindMaxYear(OwnerViewModel ownerViewModel, ref int max, ref int maxTemp)
        {
            foreach (int year in YearList)
            {
                maxTemp = 0;
                NumberOfReservations = 0;
                CanceledReservations = 0;
                PostponedReservations = 0;
                RenovationRequests = 0;
                var lastDayOfTheYear = new DateTime(year, 12, 31);
                CalculateMaxYear(ref max, ref maxTemp, year, lastDayOfTheYear);
                FindPostponedReservations(year);
                FindCanceledReservations(ownerViewModel, year);
                FindRenovationRequests(year);
                AccommodationYearlyStatsDTO statsPerYear = new AccommodationYearlyStatsDTO(year, NumberOfReservations, CanceledReservations, PostponedReservations, RenovationRequests);
                DatagridYearList.Add(statsPerYear);
            }
        }

        private void FindCanceledReservations(OwnerViewModel ownerViewModel, int year)
        {
            foreach (ReservedDates res in ownerViewModel.reservedDatesService.GetAllCanceled())
            {
                if (res.AccommodationId == accommodationId && res.StartDate.Year == year)
                {
                    CanceledReservations++;
                }
            }
        }

        private void FindRenovationRequests(int year)
        {
            foreach (OwnerRating rating in _service.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == rating.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Year == year && rating.Urgency != Urgency.Blank) RenovationRequests++;

            }
        }

        private void FindPostponedReservations(int year)
        {
            foreach (var request in res.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == request.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Year == year)
                {
                    
                    if (request.isCanceled == RequestStatus.Postponed) PostponedReservations++;
                }
            }
        }

        private void CalculateMaxYear(ref int max, ref int maxTemp, int year, DateTime lastDayOfTheYear)
        {
            foreach (var reservation in reservedDates)
            {

                if (reservation.StartDate.Year == year && accommodationId == reservation.AccommodationId)
                {
                    NumberOfReservations++;
                    if (reservation.EndDate >= lastDayOfTheYear) maxTemp += (int)(lastDayOfTheYear - reservation.StartDate).TotalDays;
                    else maxTemp += (int)(reservation.EndDate - reservation.StartDate).TotalDays;
                }
            }
            if (maxTemp >= max)
            {
                max = maxTemp;
                MaxYear = year;
            }
        }

        public void DatagridSelectionChange()
        {
            DatagridMonthList.Clear();
            for(int i=1;i <= 12; i++)
            {
                NumberOfReservations = 0;
                CanceledReservations = 0;
                PostponedReservations = 0;
                RenovationRequests = 0;
                GetcanceledReservations(i);
                GetrenovationRequests(i);
                GetNumberOfReservations(i);
                GetPostponedrenovations(i);
                AccommodationMonthlyStatsDTO statsPerMonth = new AccommodationMonthlyStatsDTO(Months[i - 1], NumberOfReservations, CanceledReservations, PostponedReservations, RenovationRequests);
                DatagridMonthList.Add(statsPerMonth);

            }

        }

        private void GetcanceledReservations(int i)
        {
            foreach (ReservedDates res in ownerViewModel.reservedDatesService.GetAllCanceled())
            {
                if (res.StartDate.Month == i && res.StartDate.Year == SelectedItem.year && accommodationId == res.AccommodationId) CanceledReservations++;
            }
        }

        private void GetrenovationRequests(int i)
        {
            foreach (OwnerRating rating in _service.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == rating.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Month == i && date.StartDate.Year == SelectedItem.year && rating.Urgency != Urgency.Blank) RenovationRequests++;

            }
        }

        private void GetPostponedrenovations(int i)
        {
            foreach (var request in res.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == request.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Month == i && date.StartDate.Year == SelectedItem.year)
                {
                    
                    if (request.isCanceled == RequestStatus.Postponed) PostponedReservations++;
                }
            }
        }

        private void GetNumberOfReservations(int i)
        {
            foreach (var reservation in reservedDates)
            {
                if (reservation.StartDate.Month == i && reservation.StartDate.Year == SelectedItem.year && accommodationId == reservation.AccommodationId) NumberOfReservations++;
            }
        }



    }
}
