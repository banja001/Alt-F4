using booking.DTO;
using booking.Model;
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
    /// Interaction logic for BookTourView.xaml
    /// </summary>
    public partial class BookTourView : Window
    {
        public BookTourView(TourLocationDTO selectedTour, User user, SearchTourViewModel searchtourviewmodel)
        {
            InitializeComponent();
            this.DataContext = new BookTourViewModel(selectedTour, user, searchtourviewmodel);
        }
    }
}
