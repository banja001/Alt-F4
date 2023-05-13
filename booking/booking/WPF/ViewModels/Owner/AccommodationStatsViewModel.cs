using booking.Commands;
using booking.Model;
using booking.View.Owner;
using booking.WPF.ViewModels;
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
    

    public class AccommodationStatsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public OwnerViewModel ownerViewModel { get; set; }
        public ObservableCollection<Accommodation> AccommodationList { get; set; }
        public Accommodation SelectedItem { get; set; }

        public ICommand GetStatsCommand => new RelayCommand(GetStatsClick);

        public AccommodationStatsViewModel(OwnerViewModel ownerViewModel)
        {
            AccommodationList = new ObservableCollection<Accommodation>(ownerViewModel.accommodationService.GetAllById(ownerViewModel.OwnerId));
            this.ownerViewModel = ownerViewModel;


        }

        public void GetStatsClick()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items", "Error");
            }
            else
            {
                AccommodationStats2 win = new AccommodationStats2(SelectedItem.Id,ownerViewModel);
                MainWindow.w.Main.Content = win;
            }
        }

    }
}
