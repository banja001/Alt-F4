using booking.Domain.DTO;
using booking.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.ViewModels.Guest1;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for Guest1ReportView.xaml
    /// </summary>
    public partial class Guest1ReportView : Window
    {
        public Guest1ReportView(User user)
        {
            InitializeComponent();

            DataContext = new Guest1ReportViewModel(user);
        }
    }
}
