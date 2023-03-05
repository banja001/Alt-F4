using booking.Manager;
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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        private AccommodationManager accommodationManager;
        public OwnerWindow()
        {
            InitializeComponent();
            DataContext = this;
            accommodationManager = new AccommodationManager();
        }

        private void AddAccommodation(object sender, RoutedEventArgs e)
        {
            AddAccommodationWindow win=new AddAccommodationWindow(accommodationManager);
            win.Show();
        }
    }
}
