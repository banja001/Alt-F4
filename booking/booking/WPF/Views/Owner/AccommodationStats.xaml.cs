using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationStats.xaml
    /// </summary>
    public partial class AccommodationStats : Page
    {
        public AccommodationStats(OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            DataContext = new AccommodationStatsViewModel(ownerViewModel);
        }
    }
}
