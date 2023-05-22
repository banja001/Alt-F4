using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    public class RateGuestViewModel:BaseViewModel
    {
        private string nameLabel;
        public string NameLabel
        {
            get
            {
                return nameLabel;
            }
            set
            {
                if (value != nameLabel)
                {
                    nameLabel = value;
                    OnPropertyChanged("NameLabel");
                }
            }

        }

        private string accommodationLabel;
        public string AccommodationLabel
        {
            get
            {
                return accommodationLabel;
            }
            set
            {
                if (value != accommodationLabel)
                {
                    accommodationLabel = value;
                    OnPropertyChanged("AccommodationLabel");
                }
            }

        }

        private string dateLabel;
        public string DateLabel
        {
            get
            {
                return dateLabel;
            }
            set
            {
                if (value != dateLabel)
                {
                    dateLabel = value;
                    OnPropertyChanged("DateLabel");
                }
            }

        }

        public Guest1RatingDTO SelectedItem { get; set; }
        public bool[] SelectedCleanRadiobutton { get; set; }
        public bool[] SelectedRulesRadiobutton { get; set; }

        public string Comment { get; set; }

        public OwnerViewModel ownerWindow;
        public ICommand AddRatingCommand => new RelayCommand(AddRating_Click);

        public RateGuestViewModel(OwnerViewModel ownerWindow,Guest1RatingDTO s)
        {
            this.SelectedCleanRadiobutton = new bool[] { false, false, false, false, false };
            this.SelectedRulesRadiobutton = new bool[] { false, false, false, false, false };
            this.ownerWindow = ownerWindow;
            SelectedItem = s;

            NameLabel ="Name:" + ownerWindow.SelectedItem.GuestName;
            AccommodationLabel ="Accommodation:"+ ownerWindow.SelectedItem.AccommodationName;
            DateLabel ="Date:"+ ownerWindow.SelectedItem.StartDate + "-" + ownerWindow.SelectedItem.EndDate;
        }


        
        private void AddRating_Click()
        {


            if (SelectedItem == null)
            {
                MessageBox.Show("Guest for this reservation is already rated", "Error");
                return;
            }
            string comment = Comment;
            int cleanliness = GetCleanliness();
            int rules = GetRulesRating();
            int id = ownerWindow.guest1Ratings.Count == 0 ? 0 : ownerWindow.guest1Ratings.Max(m => m.Id) + 1;
            int guestid = ownerWindow.users.Find(m => m.Username == ownerWindow.SelectedItem.GuestName).Id;
            if (cleanliness == 0 || rules == 0)
            {
                MessageBox.Show("Please rate all of the stats", "Error");
                return;
            }
            ModifyForGuestRating(comment, cleanliness, rules, id, guestid);
            MainWindow.w.Main.Navigate(MainWindow.w.OwnerWindow);
            SelectedItem = null;    
                
 




        }

        

        private int GetRulesRating()
        {
            int rules = 0;
            for (int i = 0; i < SelectedRulesRadiobutton.Length; i++)
            {
                if (SelectedRulesRadiobutton[i] == true)
                {
                    rules = i + 1;
                    break;
                }
            }

            return rules;
        }

        private int GetCleanliness()
        {
            int c = 0;
            for (int i = 0; i < SelectedCleanRadiobutton.Length; i++)
            {
                if (SelectedCleanRadiobutton[i] == true)
                {
                    c = i + 1;
                    break;
                }
            }

            return c;
        }
        private void ModifyForGuestRating(string comment, int cleanliness, int rules, int id, int guestid)
        {
            Guest1Rating guestrating = new Guest1Rating(id, guestid, cleanliness, rules, comment,SelectedItem.DateId);
            ownerWindow.guest1RatingsService.Add(guestrating);
            ownerWindow.reservedDatesService.UpdateRating(ownerWindow.SelectedItem.DateId);
            ownerWindow.ListToRate.Remove(ownerWindow.SelectedItem);
        }
    }
}
