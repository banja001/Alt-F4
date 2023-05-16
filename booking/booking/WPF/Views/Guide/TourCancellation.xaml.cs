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
using WPF.ViewModels;

namespace booking.View.Guide
{

    public partial class TourCancellation : Page
    {
        public TourCancellation(User guide)
        {
            InitializeComponent();
            DataContext = new TourCancellationViewModel(guide);
            //LooksOfDataGrid(UpcomingTours);
        }
        public void LooksOfDataGrid(DataGrid d)
        {
            for (int i = 0; i < d.Columns.Count; i++)
                d.Columns[i].Width = (d.Width) / d.Columns.Count;
        }
    }
}
