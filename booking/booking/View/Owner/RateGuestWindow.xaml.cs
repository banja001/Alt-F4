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
        private Guest1RatingsRepository guest1RatingsRepository;
        private UserRepository userRepository;
        private ReservedDatesRepository reservedDatesRepository;

        private Guest1RatingDTO SelectedItem;
        public ObservableCollection<Guest1RatingDTO> ListToRate;
        public RateGuestWindow(Guest1RatingsRepository g,UserRepository u,ReservedDatesRepository r,Guest1RatingDTO select, ObservableCollection<Guest1RatingDTO> list)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedCleanRadiobutton = new bool[] {false,false,false,false,false};
            SelectedRulesRadiobutton = new bool[] { false, false, false, false, false };

            guest1RatingsRepository = g;
            userRepository= u;
            reservedDatesRepository = r;

            SelectedItem= select;
            ListToRate = list;
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

            List<Guest1Rating> guestRatings = guest1RatingsRepository.FindAll();
            List<User> users = userRepository.FindAll();
            
            if (guestRatings.Count == 0)
            {
                id = 0;
            }
            else
            {
                id = guestRatings.Max(m => m.Id)+1;
            }

            guestid = users.Find(m => m.Username == SelectedItem.GuestName).Id;
            Guest1Rating guestrating = new Guest1Rating(id,guestid,cleanliness,rules,comment);
            guest1RatingsRepository.AddRating(guestrating);

            reservedDatesRepository.UpdateRating(SelectedItem.DateId);
            ListToRate.Remove(SelectedItem);

            this.Close();
        }

        

    }
}
