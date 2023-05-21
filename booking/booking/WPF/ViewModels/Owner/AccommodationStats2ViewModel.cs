using application.UseCases;
using booking.application.usecases;
using booking.Domain.Model;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPF.ViewModels.Owner
{
    public class AccommodationStats2ViewModel : BaseViewModel, INotifyPropertyChanged
    {
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

        public AccommodationStats2ViewModel(int accId,OwnerViewModel ownerViewModel)
        {
            accommodationId = accId;
            this.ownerViewModel=ownerViewModel;
            YearList = new List<int>();
            res = new ReservationRequestsService();
            for(int i=2018;i<=DateTime.Now.Year; i++)
            {
                YearList.Add(i);
            }
            
            reservedDates = ownerViewModel.reservedDatesService.GetAll();
            DatagridYearList=new ObservableCollection<AccommodationYearlyStatsDTO>();
            DatagridMonthList = new ObservableCollection<AccommodationMonthlyStatsDTO>();
            int max = -1;
            MaxYear = -1;
            int maxTemp;
            foreach (int year in YearList)
            {
                maxTemp = 0;
                NumberOfReservations = 0;
                CanceledReservations = 0;
                PostponedReservations = 0;
                var lastDayOfTheYear = new DateTime(year, 12, 31);
                CalculateMaxYear(ref max, ref maxTemp, year, lastDayOfTheYear);
                FindPostponedReservations(year);

                foreach(ReservedDates res in ownerViewModel.reservedDatesService.GetAllCanceled())
                {
                    if (res.AccommodationId == accommodationId && res.StartDate.Year == year)
                    {
                        CanceledReservations++;
                    }
                }

                AccommodationYearlyStatsDTO statsPerYear = new AccommodationYearlyStatsDTO(year, NumberOfReservations, CanceledReservations, PostponedReservations);
                DatagridYearList.Add(statsPerYear);
            }
        }

        private void FindPostponedReservations(int year)
        {
            foreach (var request in res.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == request.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Year == year)
                {
                    //if (request.isCanceled == RequestStatus.Canceled) CanceledReservations++;
                    if (request.isCanceled == RequestStatus.Postponed) PostponedReservations++;
                }//Preporuk za renoviranje fali
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
                foreach (ReservedDates res in ownerViewModel.reservedDatesService.GetAllCanceled())
                { 
                    if (res.StartDate.Month == i && res.StartDate.Year == SelectedItem.year && accommodationId == res.AccommodationId) CanceledReservations++;
                }

                GetNumberOfReservations(i);
                GetPostponedrenovations(i);
                AccommodationMonthlyStatsDTO statsPerMonth = new AccommodationMonthlyStatsDTO(Months[i - 1], NumberOfReservations, CanceledReservations, PostponedReservations);
                DatagridMonthList.Add(statsPerMonth);

            }

        }

        private void GetPostponedrenovations(int i)
        {
            foreach (var request in res.GetAll())
            {
                ReservedDates date = reservedDates.Find(a => a.Id == request.ReservationId);
                if (date.AccommodationId == accommodationId && date.StartDate.Month == i && date.StartDate.Year == SelectedItem.year)
                {
                    //if (request.isCanceled == RequestStatus.Canceled) CanceledReservations++;
                    if (request.isCanceled == RequestStatus.Postponed) PostponedReservations++;
                }//Preporuk za renoviranje fali
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
