using booking.Commands;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class RateGuestViewModel:BaseViewModel
    {
        public bool[] SelectedCleanRadiobutton { get; set; }
        public bool[] SelectedRulesRadiobutton { get; set; }

        public string Comment { get; set; }

        OwnerViewModel ownerWindow;

        public ICommand AddRatingCommand => new RelayCommand(AddRating_Click);

        public RateGuestViewModel(OwnerViewModel ownerWindow)
        {
            this.SelectedCleanRadiobutton = new bool[] { false, false, false, false, false };
            this.SelectedRulesRadiobutton = new bool[] { false, false, false, false, false };
            this.ownerWindow = ownerWindow;
        }


        
        private void AddRating_Click()
        {
           
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
            this.CloseCurrentWindow();
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
            Guest1Rating guestrating = new Guest1Rating(id, guestid, cleanliness, rules, comment);
            ownerWindow.guest1RatingsRepository.Add(guestrating);
            ownerWindow.reservedDatesService.UpdateRating(ownerWindow.SelectedItem.DateId);
            ownerWindow.ListToRate.Remove(ownerWindow.SelectedItem);
        }
    }
}
