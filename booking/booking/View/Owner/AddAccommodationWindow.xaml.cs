using booking.Manager;
using booking.Model;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AddAccommodationWindow.xaml
    /// </summary>
    public partial class AddAccommodationWindow : Window
    {
        private AccommodationManager accommodationManager;
        private AccommodationImageRepository accommodationImageRepository;
        private List<string> accommodationImagesUrl;
        public AddAccommodationWindow(AccommodationManager accMen)
        {
            InitializeComponent();
            DataContext = this;
            accommodationManager = accMen;
            accommodationImageRepository=new AccommodationImageRepository();
            accommodationImagesUrl = new List<string>();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            List<Accommodation> acc = accommodationManager.GetAllAccommodations();
            Accommodation a = new Accommodation(acc.Max(a => a.Id) + 1,
            NameTextBox.Text,LocationTextBox.Text,TypeComboBox.Text,Convert.ToInt32(MaxVisitorsTextBox.Text),
            Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));
            accommodationManager.AddAccommodation(a);

            List<AccommodationImage> acci=accommodationImageRepository.GetAllImages();
            foreach(string url in accommodationImagesUrl)
            {
                AccommodationImage image;
                if (acci.Count() == 0)
                {

                    image = new AccommodationImage(0, url, a.Id);
                }
                else
                {
                    image=new AccommodationImage(acci.Max(a => a.Id) + 1,url,a.Id);
                }
                

                accommodationImageRepository.AddAccommodationImage(image);
            }
            
            
        }







        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            
            AddAccommodationImageWindow win = new AddAccommodationImageWindow(accommodationImageRepository,accommodationImagesUrl);
            win.Show();

        }
    }
}
