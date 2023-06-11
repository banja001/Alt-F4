using booking.Domain.Model;
using booking.Model;
using booking.WPF.ViewModels;
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

namespace booking.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for MyRequestsView.xaml
    /// </summary>
    public partial class MyRequestsView : UserControl
    {
        public MyRequestsView()
        {
            InitializeComponent();
        }
        public MyRequestsView(User user)
        {
            InitializeComponent();
            this.DataContext = new MyRequestsViewModel(user);
        }
    }
}
