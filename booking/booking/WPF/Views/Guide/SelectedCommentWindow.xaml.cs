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
using booking.Domain.DTO;
using booking.Model;
using WPF.ViewModels;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for SelectedCommentWindow.xaml
    /// </summary>
    public partial class SelectedCommentWindow : Window
    {
        public SelectedCommentWindow(TourRatingDTO rating, User guide, bool demoOn)
        {
            InitializeComponent();
            DataContext = new SelectedCommentViewModel(guide,rating,demoOn);
            
        }
    }
}
