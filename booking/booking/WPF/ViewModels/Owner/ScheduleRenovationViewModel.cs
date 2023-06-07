using booking.application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using booking.WPF.Views.Owner;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    public class ScheduleRenovationViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<Accommodation> AccommodationList{get;set;}
        public OwnerViewModel ownerViewModel;

        private DateIntervalDTO selectedInterval;
        public DateIntervalDTO SelectedInterval{
            get
            {
                return selectedInterval;
            }
            set
            {
                if (value != selectedInterval)
                {
                    selectedInterval = value;
                    OnPropertyChanged("SelectedInterval");
                }
            }
        }
        public ObservableCollection<DateIntervalDTO> IntervalList { get;set;}

        private Accommodation selectedAccommodation;
        public Accommodation SelectedAccommodation {
            get
            {
                return selectedAccommodation;
            }
            set
            {
                if (value != selectedAccommodation)
                {
                    selectedAccommodation = value;
                    OnPropertyChanged("SelectedAccommodation");
                }
            }
        }

        private int duration;
        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        private string fromDate;

        public string FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                if (value != fromDate)
                {
                    fromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private string toDate;

        public string ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                if (value != toDate)
                {
                    toDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        public ICommand SearchFreeDatesCommand => new RelayCommand(SearchFreeDates);
        public ICommand ScheduleRenovationCommand => new RelayCommand(ScheduleRenovation);
        public ScheduleRenovationViewModel(OwnerViewModel ow)
        {
            ownerViewModel = ow;
            AccommodationList = new ObservableCollection<Accommodation>(ow.accommodationService.GetAllById(ow.OwnerId));
            IntervalList = new ObservableCollection<DateIntervalDTO>();
        }

        private void SearchFreeDates()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Please select accommodation!");
                return;
            }
            else if(FromDate==null || ToDate == null)
            {
                MessageBox.Show("Please fill all of info!");
                return;
            }
            DateTime startDate;
            DateTime endDate;
            DateTime tempDate;
            bool check;
            try
            {
            IntervalList.Clear();
                startDate = DateTime.Parse(FromDate);
                endDate = DateTime.Parse(ToDate);
                tempDate = startDate.AddDays(Duration);
                check = false;
            

            }
            catch
            {
                MessageBox.Show("Please fill all of info!");
                return;
            }
            if (endDate <= startDate || startDate<=DateTime.Now)
            {
                MessageBox.Show("Please select valid dates!");
                return;
            }
            else if (duration<=0)
            {
                MessageBox.Show("Please select valid duration!");
                return;
            }
            
            IterateDateIntervals(ref startDate, endDate, ref tempDate, ref check);
        }

        private void IterateDateIntervals(ref DateTime startDate, DateTime endDate, ref DateTime tempDate, ref bool check)
        {
            while (tempDate.Date <= endDate.Date)
            {
                check = true;
                check = IsOverlapingReservations(startDate, tempDate, check);
                if (check)
                {
                    check = IsOverlapingRenovations(startDate, tempDate, check);
                }
                if (check)
                {
                    DateIntervalDTO interval = new DateIntervalDTO(startDate, tempDate);
                    IntervalList.Add(interval);
                }
                tempDate = tempDate.AddDays(1);
                startDate = startDate.AddDays(1);
            }
        }

        private bool IsOverlapingRenovations(DateTime startDate, DateTime tempDate, bool check)
        {
            foreach (var renovation in ownerViewModel.renovationDatesService.GetAll())
            {
                if (renovation.AccommodationId == SelectedAccommodation.Id)
                {
                    if (!(startDate.Date >= renovation.EndDate.Date || tempDate.Date <= renovation.StartDate))
                    {
                        check = false;
                        break;
                    }
                }
            }

            return check;
        }

        private bool IsOverlapingReservations(DateTime startDate, DateTime tempDate, bool check)
        {
            foreach (var reservation in ownerViewModel.reservedDatesService.GetAll())
            {
                if (reservation.AccommodationId == SelectedAccommodation.Id)
                {
                    if (!(startDate.Date >= reservation.EndDate.Date || tempDate.Date <= reservation.StartDate))
                    {
                        check = false;
                        break;
                    }
                }
            }

            return check;
        }

        private void ScheduleRenovation()
        {
            LeaveCommentRenovationWindow win = new LeaveCommentRenovationWindow(SelectedInterval,SelectedAccommodation.Id,ownerViewModel.renovationDatesService);
            win.ShowDialog();
            IntervalList.Clear();
            
        }
    }
}
