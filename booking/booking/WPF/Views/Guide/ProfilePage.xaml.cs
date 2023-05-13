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
using booking.Model;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public User Guide { get; set; }
        public string Super { get; set; }

        public ProfilePage(User guide)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guide;
            Super = Guide.IsSuper();
        }
    }
}
