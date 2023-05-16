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
using WPF.ViewModels.Guest1;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ReviewView.xaml
    /// </summary>
    public partial class ReviewView : Window
    {
        public ReviewView(int userId)
        {
            InitializeComponent();

            this.DataContext = new ReviewViewModel(userId);
        }
    }
}
