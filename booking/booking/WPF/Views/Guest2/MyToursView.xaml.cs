using booking.Model;
using booking.WPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace booking.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for MyToursView.xaml
    /// </summary>
    public partial class MyToursView : UserControl
    {
        private readonly MyToursViewModel _myTourViewModel;
        private User User { get; set; } 
        public MyToursView()
        {
            InitializeComponent();
        }
        public MyToursView(User user)
        {
            User = user;
            InitializeComponent();
            _myTourViewModel = new MyToursViewModel(User);
            this.DataContext = _myTourViewModel;
        }
    }
}
