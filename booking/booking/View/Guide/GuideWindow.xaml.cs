using booking.Model;
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

namespace booking.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        public Tour Tour { get; set; }
        public CheckPoint CheckPoint { get; set; }
        public ObservableCollection<CheckPoint> CheckPointsForListBox{ get; set;}
        public GuideWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Tour = new Tour();
            Tour.Name = "Probaa";
            CheckPoint = new CheckPoint();
            CheckPointsForListBox = new ObservableCollection<CheckPoint>();
        }
        

        private void ConfirmTour(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddCheckPointToListBox(object sender, RoutedEventArgs e)
        {
            CheckPoint CheckPointToListBox=new CheckPoint(3,CheckPoint.Name,true,2,2);
            CheckPointsForListBox.Add(CheckPointToListBox);
        }
    }
}
