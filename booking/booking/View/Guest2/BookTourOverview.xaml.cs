using booking.DTO;
using System;
using System.Collections.Generic;
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

namespace booking.View.Guest2
{
    /// <summary>
    /// Interaction logic for BookTourOverview.xaml
    /// </summary>
    
    public partial class BookTourOverview : Window
    {
        public TourLocationDTO TourForBooking { get; set; }

        public BookTourOverview(Guest2Overview guest2Overview)
        {
            InitializeComponent();
            this.DataContext = this;
            this.TourForBooking = guest2Overview.SelectedTour;
        }
    }
}
