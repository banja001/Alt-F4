using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
using booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ReservationChange.xaml
    /// </summary>
    public partial class ReservationChangeWindow : Window
    {
       
        public ReservationChangeWindow(OwnerWindow win)
        {
            InitializeComponent();
            ReservationChangeViewModel rVM=new ReservationChangeViewModel(win,this);
            DataContext = rVM;
            

        }

        
    }
}
