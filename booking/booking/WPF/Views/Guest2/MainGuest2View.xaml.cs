using booking.Model;
using booking.WPF.ViewModels;
using booking.WPF.Views.Guest2;
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

namespace booking.View.Guest2.Windows
{
    /// <summary>
    /// Interaction logic for MainGuest2View.xaml
    /// </summary>
    /// 

    public partial class MainGuest2View : Window
    {
        private MainGuest2ViewModel _mainGuest2ViewModel;
        public User User { get; set; }
        public MainGuest2View(User user)
        {
            InitializeComponent();
            _mainGuest2ViewModel = new MainGuest2ViewModel(user);
            DataContext = _mainGuest2ViewModel;
            User = user;
        }
    }
}
