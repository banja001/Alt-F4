using booking.Model;
using booking.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPF.ViewModels.Owner;

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationImageWindow.xaml
    /// </summary>
    public partial class AddAccommodationImageWindow : Window
    {

        public AddAccommodationImageWindow(List<string> images)
        {
            InitializeComponent();
            DataContext = new AddAccommodationImageViewModel(images);
             

        }

        
    }
}
