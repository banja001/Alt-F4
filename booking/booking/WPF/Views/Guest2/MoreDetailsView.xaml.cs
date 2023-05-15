using booking.DTO;
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
using System.Windows.Shapes;
using WPF.ViewModels;

namespace WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for MoreDetailsView.xaml
    /// </summary>
    public partial class MoreDetailsView : Window
    {
        public MoreDetailsView(TourLocationDTO selectedTour)
        {
            InitializeComponent();
            this.DataContext = new MoreDetailsViewModel(selectedTour);
        }
    }
}
