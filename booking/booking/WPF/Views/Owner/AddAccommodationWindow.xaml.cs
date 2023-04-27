using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPF.ViewModels.Owner;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AddAccommodationWindow.xaml
    /// </summary>
    public partial class AddAccommodationWindow : Page
    {
        public OwnerViewModel ownerWindow;

        public AddAccommodationWindow(OwnerViewModel win)
        {
            InitializeComponent();
            DataContext = new AddAccommodationViewModel(win);
           
            ownerWindow = win;            
        }

        private void StateComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            List<string> CityList = new List<string>();
            CityComboBox.SelectedItem = null;
            string SelectedState = StateComboBox.SelectedItem.ToString();
            CityList = ownerWindow.locationService.FillCityList(CityList, SelectedState, ownerWindow.locations);
            CityComboBox.ItemsSource = CityList;
        }
        
    }
}
