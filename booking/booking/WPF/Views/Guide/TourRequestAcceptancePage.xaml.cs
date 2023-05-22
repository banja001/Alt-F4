using booking.Model;
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
using WPF.ViewModels;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for TourRequestAcceptancePage.xaml
    /// </summary>
    public partial class TourRequestAcceptancePage : Page
    {
        public TourRequestAcceptancePage(User guide,NavigationService ns)
        {
            InitializeComponent();
            DataContext=new TourRequestAcceptanceViewModel(guide,ns);
        }
    }
}
