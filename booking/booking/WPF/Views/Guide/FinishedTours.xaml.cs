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
using booking.Model;
using booking.WPF.ViewModels;

namespace booking.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for ShowReviews.xaml
    /// </summary>
    public partial class ShowReviews : Page
    {
        public ShowReviews(User guide)
        {
            InitializeComponent();
            DataContext = new FinishedToursViewModel(guide);
            //LooksOfDataGrid(AllTours);
            //LooksOfDataGrid(MostVisitedTourInThatYear);
        }
    }
}
