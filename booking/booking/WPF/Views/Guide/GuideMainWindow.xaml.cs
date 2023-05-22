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
using booking.Model;
using booking.View.Guide;
using booking.WPF.Views.Guide;
using WPF.ViewModels;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        public User Guide { get; set; }

        public GuideMainWindow(User guide)
        {
            InitializeComponent();
            Guide = guide;
            DataContext=new GuideMainViewModel(Guide,Content);
            Content.NavigationService.Navigate(new ProfilePage(Guide));
        }
    }
}
