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
using booking.Domain.DTO;
using booking.Model;
using booking.WPF.ViewModels;

namespace booking.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for ShowReviewsWindow.xaml
    /// </summary>
    public partial class ShowReviewsWindow : Window
    {
        public ShowReviewsWindow(User guide,AppointmentGuestsDTO appointment)
        {
            InitializeComponent();
            DataContext = new ShowReviewsViewModel(guide,appointment );
            LooksOfDataGrid(AllCommentsDataGrid);
        }
        public void LooksOfDataGrid(DataGrid d)
        {
            for (int i = 0; i < d.Columns.Count; i++)
                d.Columns[i].Width = (d.Width) / d.Columns.Count;
        }
    }
}
