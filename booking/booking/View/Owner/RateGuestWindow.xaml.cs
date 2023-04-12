 using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestWindow.xaml
    /// </summary>
    public partial class RateGuestWindow : Window
    {
        public bool[] SelectedCleanRadiobutton { get;set; }
        public bool[] SelectedRulesRadiobutton { get; set; }
        OwnerWindow ownerWindow;
        public RateGuestWindow(OwnerWindow win)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedCleanRadiobutton = new bool[] {false,false,false,false,false};
            SelectedRulesRadiobutton = new bool[] { false, false, false, false, false };
            ownerWindow = win;
            
        }

        

        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            int cleanliness=0,rules=0,id,guestid;
            string comment=CommentTextBox.Text;
            for(int i = 0; i < SelectedCleanRadiobutton.Length; i++)
            {
                if (SelectedCleanRadiobutton[i] == true)
                {
                    cleanliness = i+1;
                    break;
                }
            }
            for (int i = 0; i < SelectedRulesRadiobutton.Length; i++)
            {
                if (SelectedRulesRadiobutton[i] == true)
                {
                    rules = i + 1;
                    break;
                }
            }

            
            
            if (ownerWindow.guest1Ratings.Count == 0)
            {
                id = 0;
            }
            else
            {
                id = ownerWindow.guest1Ratings.Max(m => m.Id)+1;
            }

            guestid = ownerWindow.users.Find(m => m.Username == ownerWindow.SelectedItem.GuestName).Id;
            if (cleanliness == 0 || rules == 0)
            {
                MessageBox.Show("Please rate all of the stats", "Error");
                return;

            }
            Guest1Rating guestrating = new Guest1Rating(id,guestid,cleanliness,rules,comment);
            ownerWindow.guest1RatingsRepository.AddRating(guestrating);

            ownerWindow.reservedDatesRepository.UpdateRating(ownerWindow.SelectedItem.DateId);
            ownerWindow.ListToRate.Remove(ownerWindow.SelectedItem);

            this.Close();
        }

        

    }
}
