using booking.Model;
using booking.Repository;
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
using booking.WPF.ViewModels;
using WPF.ViewModels;

namespace WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for SearchTourView.xaml
    /// </summary>
    public partial class SearchTourView : UserControl
    {
        public SearchTourView()
        {
            InitializeComponent();
        }
        public SearchTourView(User user)
        {
            InitializeComponent();
            this.DataContext = new SearchTourViewModel(user);
        }
    }
}
