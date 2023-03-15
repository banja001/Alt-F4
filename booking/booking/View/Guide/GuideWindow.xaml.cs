using booking.Model;
using booking.Repository;
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
        private TourRepository _tourRepository { get; set; }
        private LocationRepository _locationRepository { get; set; }
        private CheckPointRepository _checkPointRepository { get; set; }
        public ObservableCollection<CheckPoint> CheckPointsForListBox{ get; set;}
        public GuideWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _tourRepository =new TourRepository();
            _checkPointRepository = new CheckPointRepository();
            _locationRepository=new LocationRepository();
            Tour = new Tour();
            CheckPoint = new CheckPoint();
            CheckPointsForListBox = new ObservableCollection<CheckPoint>();
            ConfirmB.IsEnabled = true;
            Tour.StartTime.Date= DateTime.Now;
        }
        

        private void ConfirmTour(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Tour.Id = _tourRepository.MakeID();
                Tour.Location.Id=_locationRepository.MakeID();
                _locationRepository.AddLocation(Tour.Location);
                _tourRepository.Add(Tour);
            }

        }

        private void AddCheckPointToListBox(object sender, RoutedEventArgs e)
        {
            int idTour = _tourRepository.MakeID();
            int idCheckPoint= _checkPointRepository.MakeID();
            CheckPoint checkPointToListBox=new CheckPoint(idCheckPoint,CheckPoint.Name,false,idTour,CheckPointsLB.Items.Count+1);
            _checkPointRepository.Add(checkPointToListBox);
            CheckPointsForListBox.Add(checkPointToListBox);
            CheckPointTB.Text = "";
            CheckPointTB.Focus();
        }
        private void RemoveTimeFromDate(string[] time)
        {
            
        }
    }
}
