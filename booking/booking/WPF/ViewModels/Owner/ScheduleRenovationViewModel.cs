using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

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
        public ScheduleRenovationViewModel(OwnerViewModel ow)
        {
            ownerViewModel = ow;
            AccommodationList = new ObservableCollection<Accommodation>(ow.accommodationService.GetAllById(ow.OwnerId));
            IntervalList = new ObservableCollection<DateIntervalDTO>();
        }

        private void SearchFreeDates()
        {
            IntervalList.Clear();
            DateTime startDate = DateTime.Parse(FromDate);
            DateTime endDate = DateTime.Parse(ToDate);
            DateTime tempDate = startDate.AddDays(Duration);
            bool check=false;
            while(tempDate.Date<=endDate.Date)
            {
                check = true;
                foreach(var reservation in ownerViewModel.reservedDatesService.GetAll())
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

                if (check)
                {
                    DateIntervalDTO interval = new DateIntervalDTO(startDate, tempDate);
                    IntervalList.Add(interval);
                }


                tempDate=tempDate.AddDays(1);
                startDate = startDate.AddDays(1);
            }


        }
    }
}
