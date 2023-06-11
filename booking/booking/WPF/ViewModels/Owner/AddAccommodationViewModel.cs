using booking.Model;
using booking.View.Owner;
using booking.View;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using booking.Commands;
using booking.WPF.ViewModels;
using WPF.Views.Owner;
using System.Collections.ObjectModel;

namespace WPF.ViewModels.Owner
{
    public class AddAccommodationViewModel : BaseViewModel,INotifyPropertyChanged
    {
        
        private List<string> accommodationImagesUrl;
        public OwnerViewModel ownerViewModel;
        //public List<string> StateList;

        private bool nextEnabled;
        public bool NextEnabled
        {
            get
            {
                return nextEnabled;
            }
            set
            {
                if (value != nextEnabled)
                {
                    nextEnabled = value;
                    OnPropertyChanged("NextEnabled");
                }
            }
        }

        private bool prevEnabled;
        public bool PrevEnabled
        {
            get
            {
                return prevEnabled;
            }
            set
            {
                if (value != prevEnabled)
                {
                    prevEnabled = value;
                    OnPropertyChanged("PrevEnabled");
                }
            }
        }
        
        public string State { get; set; }
        
        public string City { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string MaxVisitors { get; set; }
        public string MinDaysToUse { get; set; }
        public string DaysToCancel { get; set; }

        private string image;
        public string ImageUrl {
            get
            {
                return image;
            }
            set
            {
                if (value != image)
                {
                    image = value;
                    OnPropertyChanged("ImageUrl");
                }
            }
            }
        private string selectedItemCity;
        public string SelectedItemCity {
            get
            {
                return selectedItemCity;
            }
            set
            {
                if (value != selectedItemCity)
                {
                    selectedItemCity = value;
                    OnPropertyChanged("SelectedItemCity");
                    CityLabel = "";

                }

            }
        }

        private string selectedItemState;
        public string SelectedItemState {
            get
            {
                return selectedItemState;
            }
            set
            {
                if (value != selectedItemState)
                {
                    selectedItemState = value;
                    OnPropertyChanged("SelectedItemState");
                    StateComboBox_SelectionChanged();
                    CityLabel = "City";
                }

            }

        }

        private BitmapImage slika;
        public BitmapImage Slika { 
            get
            {
                return slika;
            }
            set
            {
                if (value != slika)
                {
                    slika = value;
                    OnPropertyChanged("Slika");
                }
                
            }
            }
        public int ActiveImageIndx { get; set; }
        public List<string> StateList { get; set; }

        public ObservableCollection<string> CityList { get; set; }
        public ICommand NextPictureCommand => new RelayCommand(NextPictureClick);
        public ICommand PrevPictureCommand => new RelayCommand(PrevImageButtonClick);
        public ICommand ConfirmCommand => new RelayCommand(Confirm);

        public ICommand CancelCommand => new RelayCommand(Cancel);
        public ICommand AddImageCommand => new RelayCommand(AddImageClick);
        //public ICommand ComboboxSelectionChangedCommand => new RelayCommand(StateComboBox_SelectionChanged);
        public ICommand RemoveImageCommand => new RelayCommand(RemoveImageClick);

        
        public Regex intRegex = new Regex("^[0-9]{1,4}$");

        private string stateLabel;
        public string StateLabel {
            get
            {
                return stateLabel;
            }
            set
            {
                if (value != stateLabel)
                {
                    stateLabel = value;
                    OnPropertyChanged("StateLabel");
                }

            }
        }
        private string cityLabel;
        public string CityLabel
        {
            get
            {
                return cityLabel;
            }
            set
            {
                if (value != cityLabel)
                {
                    cityLabel = value;
                    OnPropertyChanged("CityLabel");
                }

            }
        }
        private bool imaSlika;
        public bool ImaSlika {
            get
            {
                return imaSlika;
            }
            set
            {
                if (value != imaSlika)
                {
                    imaSlika = value;
                    OnPropertyChanged("ImaSlika");
                }

            }
        }
        public AddAccommodationViewModel(OwnerViewModel ownerViewModel)
        {
            this.accommodationImagesUrl = new List<string>();
            this.ownerViewModel = ownerViewModel;
            StateList = ownerViewModel.locationService.InitializeStateList(new List<string>(), ownerViewModel.locations);
            this.CityList = new ObservableCollection<string>();
            StateLabel = "State";
            CityLabel = "City";
            ImaSlika = false;
        }

        public void StateComboBox_SelectionChanged()
        {
            StateLabel = "";
            
            List<string> listTemp = new List<string>();
            SelectedItemCity = null;
            listTemp = ownerViewModel.locationService.FillCityList(listTemp, SelectedItemState.ToString(), ownerViewModel.locations);
            CityList.Clear();
            foreach(string item in listTemp)
            {
                CityList.Add(item);
            }
        }

        private void Cancel()
        {
            MainWindow.w.Main.Navigate(MainWindow.w.OwnerWindow);
        }

        private void Confirm()
        {

            Accommodation a = AddAccommodation();
            ownerViewModel.accommodationService.Add(a);
            ownerViewModel.accommodationImageService.AddImages(a, accommodationImagesUrl, ownerViewModel.accommodationImages);
            MessageBox.Show("Accommodation added successfully!");
            MainWindow.w.Main.Navigate(MainWindow.w.OwnerWindow);
            
        }

        private Accommodation AddAccommodation()
        {
            int locid = ownerViewModel.locationService.GetLocationId(State, City, ownerViewModel.locations);
            int accid = ownerViewModel.accommodations.Count() == 0 ? 0 : ownerViewModel.accommodations.Max(a => a.Id) + 1;
            Accommodation a = new Accommodation(accid, ownerViewModel.OwnerId, Name, locid, Type, Convert.ToInt32(MaxVisitors), Convert.ToInt32(MinDaysToUse), Convert.ToInt32(DaysToCancel));
            return a;
        }

        private void AddImageClick()
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                MessageBox.Show("Please enter atleast one image", "Error");
                
            }
            else
            {
                accommodationImagesUrl.Insert(0, ImageUrl);
                ImageUrl = "";
            }
            ShowImage();

            ImaSlika = true;
        }
        
        private void RemoveImageClick()
        {
            if (accommodationImagesUrl.Count() > 0)
            {
                accommodationImagesUrl.RemoveAt(ActiveImageIndx);
                MessageBox.Show("Image removed", "Message");
            }
            ShowImage();
            if (accommodationImagesUrl.Count == 0)
            {
                ImaSlika = false;
            }
        }

        public void SetImageSource(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@url, UriKind.Absolute);
            bitmapImage.EndInit();
            Slika = bitmapImage;
        }
        public void ShowImage()
        {
            CheckIndexScope();
            if (accommodationImagesUrl.Count == 0)
            {
                Slika = null;
               
                return;
            }

            ActiveImageIndx = 0;
            SetImageSource(accommodationImagesUrl[ActiveImageIndx]);
            CheckIndexScope();
        }

        public void NextPictureClick()
        {
            SetImageSource(accommodationImagesUrl[++ActiveImageIndx]);
            CheckIndexScope();
        }

        private void PrevImageButtonClick()
        {
            SetImageSource(accommodationImagesUrl[--ActiveImageIndx]);
            CheckIndexScope();
        }
        
        public void CheckIndexScope()
        {
            if (ActiveImageIndx + 1 >= accommodationImagesUrl.Count)
            {
                NextEnabled = false;
            }
            else
            {
                NextEnabled = true;
            }

            if (ActiveImageIndx <= 0)
            {
                PrevEnabled = false;
            }
            else
            {
                PrevEnabled = true;
            }
        }
    }
}
