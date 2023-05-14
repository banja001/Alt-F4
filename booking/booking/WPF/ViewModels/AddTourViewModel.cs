using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using booking.WPF.ViewModels;
using booking.Commands;
using System.Windows.Input;

namespace WPF.ViewModels
{
    internal class AddTourViewModel: BaseViewModel
    {
        public Tour Tour { get; set; }
        public CheckPoint CheckPoint { get; set; }
        private TourRepository _tourRepository { get; set; }
        private LocationRepository _locationRepository { get; set; }
        private CheckPointRepository _checkPointRepository { get; set; }
        private TourImageRepository _tourImageRepository { get; set; }
        public ObservableCollection<CheckPoint> CheckPointsForListBox { get; set; }
        public ObservableCollection<TourImage> ImagesForListBox { get; set; }
        public List<TourImage> TourImages { get; set; }
        public string Image { get; set; }
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ICommand AddCheckPointCommand => new RelayCommand(AddCheckPointToListBox);
        public ICommand AddImageCommand => new RelayCommand(AddImageToList);
        public ICommand ConfirmTourCommand=> new RelayCommand(ConfirmTour, CanAddTour);
        public AddTourViewModel()
        {
            _tourRepository = new TourRepository();
            _checkPointRepository = new CheckPointRepository();
            _locationRepository = new LocationRepository();
            _tourImageRepository = new TourImageRepository();
            Tour = new Tour();
            CheckPoint = new CheckPoint();
            TourImages = new List<TourImage>();
            CheckPointsForListBox = new ObservableCollection<CheckPoint>();
            ImagesForListBox=new ObservableCollection<TourImage>();
            Tour.StartTime.Date = DateTime.Now;
        }


        private void ConfirmTour()
        {
            if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Tour.StartTime.IsValid)
                {
                    Tour.Id = _tourRepository.MakeID();
                    Tour.Location.Id = _locationRepository.MakeID();
                    _checkPointRepository.AddRange(ObservableToList(CheckPointsForListBox));
                    _tourImageRepository.AddRange(TourImages);
                    _locationRepository.Add(Tour.Location);
                    _tourRepository.Add(Tour);
                    MessageBox.Show("Tour is addded!");
                    this.CloseCurrentWindow();
                }
                else
                    MessageBox.Show("Form is not properly filled!");
            }

        }

        public bool CanAddTour()
        {
            return Tour.StartTime.IsValid && !String.IsNullOrEmpty(Tour.Description) &&
                   !string.IsNullOrEmpty(Tour.Language) && !string.IsNullOrEmpty(Tour.Name) && Tour.Duration > 0 &&
                   Tour.MaxGuests > 0 && CheckPointsForListBox.Count > 1 && !String.IsNullOrEmpty(Tour.Location.City) && !String.IsNullOrEmpty(Tour.Location.State);
        }

        private void AddCheckPointToListBox()
        {
            int idTour = _tourRepository.MakeID();
            int idCheckPoint = _checkPointRepository.MakeID() + CheckPointsForListBox.Count;
            CheckPoint checkPointToListBox = new CheckPoint(idCheckPoint, CheckPoint.Name, false, idTour, CheckPointsForListBox.Count + 1);
            CheckPointsForListBox.Add(checkPointToListBox);

        }

        private List<CheckPoint> ObservableToList(ObservableCollection<CheckPoint> checkPoints)
        {
            List<CheckPoint> checkPointsFromObservable = new List<CheckPoint>();
            checkPointsFromObservable.AddRange(checkPoints);
            return checkPointsFromObservable;
        }
        /*private bool IsEmpty()
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
        */
        private void AddImageToList()
        {
            TourImage image = new TourImage(_tourImageRepository.MakeID() + TourImages.Count, Image, _tourRepository.MakeID());
            TourImages.Add(image);
            ImagesForListBox.Add(image);
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}
