using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using application.UseCases;
using WPF.ViewModels;

namespace booking.View.Guide
{
    /// <summary>
    /// Interaction logic for LiveTrackingWindow.xaml
    /// </summary>
    public partial class LiveTrackingWindow : Page
    {
        public LiveTrackingWindow(User Guide)
        {
            InitializeComponent();
            DataContext = new LiveTrackingTourViewModel(Guide);
            //LooksOfDataGrid(ToursDG);
        }
        public void LooksOfDataGrid(DataGrid d)
        {
            foreach (var t in d.Columns)
                t.Width = (d.Width) / d.Columns.Count;
        }
    }
    
}
