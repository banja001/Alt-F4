using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Model;

namespace booking.WPF.ViewModels
{
    public class FinishedToursViewModel : BaseViewModel
    {
        public ObservableCollection<int> Years { get; set; }
        public int SelectedYear { get; set; }
        public List<AppointmentGuestsDTO> FinishedTours { get; set; }
        public ObservableCollection<AppointmentGuestsDTO> MostVisitedTour { get; set; }
        public AppointmentGuestsDTO SelectedTour { get; set; }
        private readonly AppointmentService _appointmentService;
        private User Guide { get; set; }

        public FinishedToursViewModel(User guide)
        {
            _appointmentService = new AppointmentService();
            Years = _appointmentService.FindAllYears(guide.Id);
            FinishedTours = _appointmentService.CreateListOfFinishedTours(guide.Id);
            MostVisitedTour =new ObservableCollection<AppointmentGuestsDTO>();
            SelectedTour =new AppointmentGuestsDTO();
            SelectedYear = Years[0];
            Guide = guide;
        }

        public ICommand ShowCommand => new RelayCommand(Show);
        public ICommand FindCommand => new RelayCommand(FindMostVisitedTour);
        public void Show()
        {
            
        }

        public void FindMostVisitedTour()
        {
            try
            {
                MostVisitedTour.Clear();
                MostVisitedTour.Add(_appointmentService.FindMostVisitedTour(Guide.Id, SelectedYear.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}
