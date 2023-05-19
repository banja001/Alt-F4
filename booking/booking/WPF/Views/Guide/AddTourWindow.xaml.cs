using booking.Model;
using booking.Repository;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPF.ViewModels;

namespace booking.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Page
    {
        public AddTourWindow()
        {
            InitializeComponent();
            DataContext = new AddTourViewModel();
        }
        public AddTourWindow(SimpleAndComplexTourRequestsDTO simpleRequest, DateTime startDate,bool isNotRequest)
        {
            InitializeComponent();
            DataContext = new AddTourViewModel(simpleRequest,startDate,isNotRequest);
        }
    }
}
