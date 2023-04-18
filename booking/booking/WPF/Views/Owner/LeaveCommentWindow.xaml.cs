using booking.Domain.Model;
using booking.View;
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
using WPF.ViewModels.Owner;

namespace booking.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for LeaveComment.xaml
    /// </summary>
    public partial class LeaveCommentWindow : Window
    {
        

        public LeaveCommentWindow(ReservationChangeViewModel res,ReservationRequests r)
        {
            InitializeComponent();
            DataContext = new LeaveCommentViewModel(res,r);
            

        }

        
    }
}
