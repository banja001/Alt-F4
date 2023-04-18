﻿using booking.Model;
using booking.View.Owner;
using booking.View;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using booking.Commands;
using booking.WPF.ViewModels;

namespace WPF.ViewModels.Owner
{
    public class AddAccommodationViewModel : BaseViewModel
    {
        
        private List<string> accommodationImagesUrl;
        public OwnerViewModel ownerViewModel;
        //public List<string> StateList;

        public string State { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string MaxVisitors { get; set; }
        public string MinDaysToUse { get; set; }
        public string DaysToCancel { get; set; }

        public string SelectedItemCity { get; set; }
        public string SelectedItemState { get; set; }
        
        public List<string> StateList { get; set; }
        public ICommand ConfirmCommand => new RelayCommand(Confirm);
        public ICommand AddImageCommand => new RelayCommand(AddImageClick);
        //public ICommand ComboboxSelectionChangedCommand => new RelayCommand(StateComboBox_SelectionChanged);
        public ICommand RemoveImageCommand => new RelayCommand(RemoveImageClick);

        
        public Regex intRegex = new Regex("^[0-9]{1,4}$");

        public AddAccommodationViewModel(OwnerViewModel ownerWindow)
        {
            this.accommodationImagesUrl = new List<string>();
            this.ownerViewModel = ownerWindow;
            StateList = ownerWindow.locationService.InitializeStateList(new List<string>(), ownerWindow.locations);

        }
        
        private bool isValid()
        {
            if (string.IsNullOrEmpty(State) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Type) ||
                string.IsNullOrEmpty(MaxVisitors) || string.IsNullOrEmpty(MinDaysToUse) || string.IsNullOrEmpty(DaysToCancel))
            {
                MessageBox.Show("Please fill in all of the textboxes", "Error");
                return false;
            }
            Match match = intRegex.Match(MaxVisitors);
            if (!match.Success)
            {
                MessageBox.Show("Max visitors should be a valid number", "Error");
                return false; ;
            }
            match = intRegex.Match(MinDaysToUse);
            if (!match.Success)
            {
                MessageBox.Show("Min reservation should be a valid number", "Error");
                return false;
            }
            match = intRegex.Match(DaysToCancel);
            if (!match.Success)
            {
                MessageBox.Show("Days to cancel should be a valid number", "Error");
                return false;
            }
            if (accommodationImagesUrl.Count() == 0)
            {
                MessageBox.Show("Please enter atleast one image", "Error");
                return false;
            }
            if (Name.Last().Equals('*'))
            {
                MessageBox.Show("Accommodation name cant end with *", "Error");
                return false;
            }


            return true;
        }

        private void Confirm()
        {
            
            if (!isValid())
            {
                return;
            }

            Accommodation a = AddAccommodation();
            ownerViewModel.accommodationService.Add(a);
            ownerViewModel.accommodationImageService.AddImages(a, accommodationImagesUrl, ownerViewModel.accommodationImages);
            this.CloseCurrentWindow();
        }

        private Accommodation AddAccommodation()
        {
            int locid = ownerViewModel.locationService.GetLocationId(State, City, ownerViewModel.locations);
            int accid = ownerViewModel.accommodations.Count() == 0 ? 0 : ownerViewModel.accommodations.Max(a => a.Id) + 1;
            Accommodation a = new Accommodation(accid, ownerViewModel.OwnerId, Name, locid, Type, Convert.ToInt32(MaxVisitors), Convert.ToInt32(MinDaysToUse), Convert.ToInt32(DaysToCancel));
            return a;
        }

        private void AddImageClick()
        {

            AddAccommodationImageWindow win = new AddAccommodationImageWindow(accommodationImagesUrl);
            win.ShowDialog();
            

        }
        /*
        private void StateComboBox_SelectionChanged()
        {
            List<string> CityList = new List<string>();
            SelectedItemCity = null;
            string SelectedState = SelectedItemState.ToString();
            CityList = ownerViewModel.locationService.FillCityList(CityList, SelectedState, ownerViewModel.locations);
            //CityComboBox.ItemsSource = CityList;
        }*/
        private void RemoveImageClick()
        {
            if (accommodationImagesUrl.Count() > 0)
            {
                accommodationImagesUrl.RemoveAt(accommodationImagesUrl.Count() - 1);
                MessageBox.Show("Image removed", "Message");
            }

        }
    }
}