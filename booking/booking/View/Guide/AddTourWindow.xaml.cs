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
    public partial class AddTourWindow : Window
    {
        public Tour Tour { get; set; }
        public CheckPoint CheckPoint { get; set; }
        private TourRepository _tourRepository { get; set; }
        private LocationRepository _locationRepository { get; set; }
        private CheckPointRepository _checkPointRepository { get; set; }
        private TourImageRepository _tourImageRepository { get; set; }
        public ObservableCollection<CheckPoint> CheckPointsForListBox{ get; set;}
        public List<TourImage> TourImages { get; set; }
        public string Image { get; set; }
        public AddTourWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _tourRepository =new TourRepository();
            _checkPointRepository = new CheckPointRepository();
            _locationRepository=new LocationRepository();
            _tourImageRepository=new TourImageRepository();
            Tour = new Tour();
            CheckPoint = new CheckPoint();
            TourImages= new List<TourImage>();
            CheckPointsForListBox = new ObservableCollection<CheckPoint>();
            ConfirmB.IsEnabled = true;
            Tour.StartTime.Date= DateTime.Now;
        }
        

        private void ConfirmTour(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Tour.StartTime.IsValid && IsAllOK())
                {
                    Tour.Id = _tourRepository.MakeID();
                    Tour.Location.Id = _locationRepository.MakeID();
                    _checkPointRepository.AddRange(ObservableToList(CheckPointsForListBox));
                    _tourImageRepository.AddRange(TourImages);
                    _locationRepository.AddLocation(Tour.Location);
                    _tourRepository.Add(Tour);
                    MessageBox.Show("Tour is addded!");
                    this.Close();
                }
                else
                    MessageBox.Show("Form is not properly filled!");
            }

        }

        private void AddCheckPointToListBox(object sender, RoutedEventArgs e)
        {
            int idTour = _tourRepository.MakeID();
            int idCheckPoint= _checkPointRepository.MakeID()+CheckPointsForListBox.Count;
            CheckPoint checkPointToListBox=new CheckPoint(idCheckPoint,CheckPoint.Name,false,idTour,CheckPointsLB.Items.Count+1);
            CheckPointsForListBox.Add(checkPointToListBox);
            CheckPointTB.Text = "";
            CheckPointTB.Focus();
        }
       
        private List<CheckPoint> ObservableToList(ObservableCollection<CheckPoint> checkPoints)
        { 
            List<CheckPoint> checkPointsFromObservable=new List<CheckPoint>();
            checkPointsFromObservable.AddRange(checkPoints);
            return checkPointsFromObservable;
        }
        private bool IsEmpty()
        {

            return !string.IsNullOrEmpty(NameTB.Text) && !string.IsNullOrEmpty(CountyTB.Text) && !string.IsNullOrEmpty(CityTB.Text)
                && !string.IsNullOrEmpty(LanguageTB.Text) && !string.IsNullOrEmpty(DurationTB.Text) && !string.IsNullOrEmpty(DescriptionTB.Text)
                && !string.IsNullOrEmpty(TimeTB.Text) && !string.IsNullOrEmpty(MaxNumGuestsTB.Text);
        }

        private bool IsDateAfter()
        {
            return DateDTP.SelectedDate >= DateTime.Today;
        }
        private bool IsAllOK()
        {
            if (CheckPointsLB.Items.Count < 2)
                return false;
            else
                return IsEmpty() && IsDateAfter();
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            TourImage image= new TourImage(_tourImageRepository.MakeID()+TourImages.Count,Image,_tourRepository.MakeID());
            TourImages.Add(image);
            ImagesTB.Text = "";
            ImagesTB.Focus();
        }
    }
}
