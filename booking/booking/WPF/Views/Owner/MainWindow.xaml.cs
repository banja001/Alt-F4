using booking.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public int OwnerId;
        public OwnerWindow OwnerWindow;
        public OwnerViewModel OwnerModel;

        public MainWindow(int id)
        {
            InitializeComponent();
            OwnerId = id;
            OwnerWindow = new OwnerWindow(id);
            OwnerModel=OwnerWindow.OwnerModel;
            Main.Content = OwnerWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content=OwnerWindow;   
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Main.Content=new AddAccommodationWindow(OwnerModel);
        }
    }
}
