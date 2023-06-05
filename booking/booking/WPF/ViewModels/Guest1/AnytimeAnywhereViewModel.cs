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

namespace WPF.ViewModels.Guest1
{
    public class AnytimeAnywhereViewModel : BaseViewModel
    {
        private int userId;

        public ReservedDates Date { get; set; }
        public int NumOfGuests { get; set; }
        public int NumOfDays { get; set; }
        public ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }

        private ObservableCollection<AccommodationLocationDTO> AllAccommodationDTOs { get; set; }
        public List<ReservedDates> ReservedDates { get; set; }
        public ObservableCollection<ReservedDates> FreeDates { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly ReservedDatesService _reservedDatesService;

        public ICommand SearchAccommodationsCommand => new RelayCommand(SearchAccommodations);

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
            foreach(var accommodation in AllAccommodationDTOs)
            {
                int maxCapacity = _accommodationService.FindById(accommodation.Id).MaxCapacity;

                ReservedDates = _reservedDatesService.GetAllByAccommodationId(accommodation.Id);

                FilterReservedDatesByMonth();
                CreateDateIntervals(Date.StartDate, Date.EndDate, accommodation.Id);
                RemoveReservedDatesFromIntervals();

                if (maxCapacity >= NumOfGuests && NumOfDays >= accommodation.MinDaysToUse && FreeDates.Count > 0)
                {
                    AccommodationDTOs.Add(accommodation);
                }
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

                DateTime firstDate = GetStartOfReservedDate(date).AddDays(-NumOfDays + 2);
                DateTime lastDate = GetEndOfReservedDate(date);

                while (firstDate < lastDate)
                {
                    if (FreeDates.Where(d => d.StartDate == firstDate).Count() != 0)
                        FreeDates.Remove(FreeDates.Where(d => d.StartDate == firstDate).ToList()[0]);

                    firstDate = firstDate.AddDays(1);
                }
            }
        }

        private DateTime GetStartOfReservedDate(ReservedDates date)
        {
            List<ReservedDates> lastDates = FreeDates.Where(d => d.StartDate == date.StartDate).ToList();

            return lastDates.Count() == 0
                ? new DateTime(FreeDates[0].StartDate.Year, FreeDates[0].StartDate.Month, FreeDates[0].StartDate.Day)
                : lastDates[0].StartDate;
        }
        private DateTime GetEndOfReservedDate(ReservedDates date)
        {
            List<ReservedDates> firstDates = FreeDates.Where(d => d.EndDate == date.EndDate).ToList();

            return firstDates.Count() == 0
                ? new DateTime(FreeDates[FreeDates.Count - 1].EndDate.Year, FreeDates[FreeDates.Count - 1].EndDate.Month, FreeDates[FreeDates.Count - 1].EndDate.Day)
                : firstDates[0].EndDate;
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
    }
}
