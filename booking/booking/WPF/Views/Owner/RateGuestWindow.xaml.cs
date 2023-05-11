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
using WPF.ViewModels.Owner;
using WPF.Views.Owner;

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestWindow.xaml
    /// </summary>
    public partial class RateGuestWindow : Page
    {
        
        public RateGuestWindow(OwnerViewModel win, Guest1RatingDTO s)
        {
            InitializeComponent();
            this.DataContext = new RateGuestViewModel(win,s);
        }



       

    }
}
