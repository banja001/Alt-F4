using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class RenovationViewViewModel : BaseViewModel
    {
        public ICommand CancelRenovationTooltipCommand => new RelayCommand(CancelRenovationTooltip);

        private bool cancelTooltip = false;
        public bool CancelTooltip
        {
            get
            {
                return cancelTooltip;
            }
            set
            {
                if (value != cancelTooltip)
                {
                    cancelTooltip = value;
                    OnPropertyChanged("CancelTooltip");
                }
            }
        }

        private void CancelRenovationTooltip()
        {
            if (GlobalVariables.tt == true)
            {
                if (cancelTooltip)
                {
                    CancelTooltip = false;

                }
                else
                {
                    CancelTooltip = true;

                }
            }
        }
        public RenovationAccommodationDTO SelectedRenovation { get; set; }
        public ObservableCollection<RenovationAccommodationDTO> FutureRenovations{get;set;}
        public ObservableCollection<RenovationAccommodationDTO> PastRenovations { get; set; }

        private OwnerViewModel ownerViewModel;
        public ICommand CancelRenovationCommand => new RelayCommand(CancelRenovation);
        public RenovationViewViewModel(OwnerViewModel ow)
        {
            ownerViewModel = ow;
            FutureRenovations = new ObservableCollection<RenovationAccommodationDTO>();
            PastRenovations = new ObservableCollection<RenovationAccommodationDTO>();
            List<Accommodation> accList = ownerViewModel.accommodationService.GetAll();

            foreach(var renovation in ownerViewModel.renovationDatesService.GetAll())
            {
                Accommodation accommodation = accList.Find(a => a.Id == renovation.AccommodationId);
                if (accommodation.OwnerId == ownerViewModel.OwnerId)
                {

                    RenovationAccommodationDTO temp = new RenovationAccommodationDTO(accommodation.Id,accommodation.Name, renovation.StartDate, renovation.EndDate);
                    if (renovation.StartDate >= DateTime.Now.AddDays(5))
                    {
                        
                        FutureRenovations.Add(temp);
                    }
                    else
                    {
                        PastRenovations.Add(temp);
                    }
                
                }
            }

        }

        private void CancelRenovation()
        {
            RenovationDates ren=ownerViewModel.renovationDatesService.GetAll().Find(a => a.AccommodationId == SelectedRenovation.AccommodationId && a.StartDate==SelectedRenovation.StartDate && a.EndDate==SelectedRenovation.EndDate);
            ownerViewModel.renovationDatesService.Remove(ren);
            FutureRenovations.Remove(SelectedRenovation);
            SelectedRenovation = null;
            MessageBox.Show("renovation canceled successfully!");
        }


    }
}
