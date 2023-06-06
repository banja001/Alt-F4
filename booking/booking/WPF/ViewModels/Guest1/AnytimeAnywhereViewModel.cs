using application.UseCases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class AnytimeAnywhereViewModel : BaseViewModel
    {
        public ReservedDates Date { get; set; }
        public int NumOfGuests { get; set; }
        public int NumOfDays { get; set; }
        public ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }

        private ObservableCollection<AccommodationLocationDTO> AllAccommodationDTOs { get; set; }
        public List<ReservedDates> ReservedDates { get; set; }
        public ObservableCollection<ReservedDates> FreeDates { get; set; }

        public AccommodationLocationDTO SelectedAccommodation { get; set; }

        private int userId;

        private readonly AccommodationService _accommodationService;
        private readonly ReservedDatesService _reservedDatesService;

        public ICommand SearchAccommodationsCommand => new RelayCommand(SearchAccommodations);
        public ICommand ReserveAccommodationClickCommand => new RelayCommand(ReserveAccommodationClick);

        public AnytimeAnywhereViewModel(int userId)
        {
            _accommodationService = new AccommodationService();
            _reservedDatesService = new ReservedDatesService();

            AllAccommodationDTOs = _accommodationService.CreateAccomodationDTOs();
            FreeDates = new ObservableCollection<ReservedDates>();
            AccommodationDTOs = new ObservableCollection<AccommodationLocationDTO>();
            Date = new ReservedDates();

            this.userId = userId;
        }

        private void SearchAccommodations()
        {
            ClearAccommodationDTOs();

            SetDateIfNotSpecified();

            foreach (var accommodation in AllAccommodationDTOs)
            {
                AddToAccommodationDTOs(accommodation);
            }
        }

        private void SetDateIfNotSpecified()
        {
            if (Date.StartDate == new DateTime(0001, 01, 01) && Date.EndDate == new DateTime(0001, 01, 01))
            {
                Date.StartDate = DateTime.Now;

                int year = Date.StartDate.Year;
                int month = Date.StartDate.Month + 1;

                if (month > 12)
                {
                    month = 1;
                    year++;
                }

                int days = DateTime.DaysInMonth(year, month);

                Date.EndDate = new DateTime(year, month, days);
            }
        }

        private void AddToAccommodationDTOs(AccommodationLocationDTO accommodation)
        {
            int maxCapacity = _accommodationService.FindById(accommodation.Id).MaxCapacity;

            ReservedDates = _reservedDatesService.GetAllByAccommodationId(accommodation.Id);

            FreeDates.Clear();
            FilterReservedDatesByMonth();
            CreateDateIntervals(Date.StartDate, Date.EndDate, accommodation.Id);
            RemoveReservedDatesFromIntervals();

            if (maxCapacity >= NumOfGuests && NumOfDays >= accommodation.MinDaysToUse && FreeDates.Count > 0)
            {
                AccommodationDTOs.Add(accommodation);
            }
        }

        private void ClearAccommodationDTOs()
        {
            while (AccommodationDTOs.Count > 0)
            {
                AccommodationDTOs.RemoveAt(0);
            }
        }

        private void RemoveReservedDatesFromIntervals()
        {
            foreach (var date in ReservedDates)
            {
                if (FreeDates.Count == 0)
                    return;

                if (date.EndDate < FreeDates[0].StartDate || date.StartDate > FreeDates[FreeDates.Count - 1].EndDate)
                    continue;

                DateTime firstDate = date.StartDate;
                DateTime lastDate = date.EndDate;

                while (firstDate < lastDate)
                {
                    if (FreeDates.Where(d => d.StartDate.Date == firstDate.Date).Count() != 0)
                        FreeDates.Remove(FreeDates.Where(d => d.StartDate.Date == firstDate.Date).ToList()[0]);

                    firstDate = firstDate.AddDays(1);
                }
            }
        }

        private void CreateDateIntervals(DateTime startDate, DateTime endDate, int accommodationId)
        {
            while ((endDate - startDate).Days >= NumOfDays - 1)
            {
                FreeDates.Add(new ReservedDates(startDate, startDate.AddDays(NumOfDays - 1), accommodationId));

                startDate = startDate.AddDays(1);
            }
        }

        private void FilterReservedDatesByMonth()
        {
            ReservedDates = ReservedDates.OrderBy(d => d.StartDate).ToList();
            ReservedDates = ReservedDates.Where(d => d.StartDate.Month == Date.StartDate.Month || d.StartDate.Month == Date.EndDate.Month
                                            || d.EndDate.Month == Date.EndDate.Month || d.EndDate.Month == Date.StartDate.Month).ToList();
        }

        public void ReserveAccommodationClick()
        {
            if(SelectedAccommodation == null)
            {
                MessageBox.Show("You have to select an accommodation before you can procceed");
                return;
            }

            int maxCapacity = _accommodationService.FindById(SelectedAccommodation.Id).MaxCapacity;

            ReservedDates = _reservedDatesService.GetAllByAccommodationId(SelectedAccommodation.Id);

            FreeDates.Clear();
            FilterReservedDatesByMonth();
            CreateDateIntervals(Date.StartDate, Date.EndDate, SelectedAccommodation.Id);
            RemoveReservedDatesFromIntervals();

            var reserveAccommodation = new ReserveAccommodationAAView(FreeDates, SelectedAccommodation.Id, userId, NumOfGuests);
            reserveAccommodation.ShowDialog();
        }
    }
}
