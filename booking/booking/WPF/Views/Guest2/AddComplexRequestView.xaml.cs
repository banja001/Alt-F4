using booking.Model;
using Domain.Model;
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
    /// Interaction logic for AddComplexRequestView.xaml
    /// </summary>
    public partial class AddComplexRequestView : Window
    {
        public AddComplexRequestView(User user, ComplexRequest complexRequest)
        {
            InitializeComponent();
            this.DataContext = new AddComplexRequestViewModel(user, complexRequest);
        }
    }
}
