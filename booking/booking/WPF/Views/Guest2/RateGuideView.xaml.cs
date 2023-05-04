using booking.Model;
using booking.WPF.ViewModels;
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

namespace booking.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for RateGuideView.xaml
    /// </summary>
    public partial class RateGuideView : Window
    {
        private readonly RateGuideViewModel _rateGuideViewModel;
        public RateGuideView(Appointment selectedTour, User guest)
        {
            InitializeComponent();
            _rateGuideViewModel = new RateGuideViewModel(selectedTour, TourKnowledgePanel, LanguageKnowledgePanel, TourEnjoymentPanel, guest);
            this.DataContext = _rateGuideViewModel;

        }
    }
}
