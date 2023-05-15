using booking.application.UseCases.Guest2;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.Model;
using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Security.Policy;

namespace booking.WPF.ViewModels
{
    public class RateGuideViewModel : BaseViewModel
    {
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ICommand AddPhotoCommand => new RelayCommand(AddPhoto);
        public ICommand SubmitCommand => new RelayCommand(Submit);
        public Appointment SelectedTour { get; set; }
        public BitmapSource ImageSource { get; set; }
        public ICommand SwipeLeftCommand => new RelayCommand(OnSwipeLeftButtonClick);
        public ICommand SwipeRightCommand => new RelayCommand(OnSwipeRightButtonClick);

        private readonly GuideRatingImageService _guideRatingImageService;
        private readonly GuideRatingService _guideRatingService;
        private readonly AppointmentService _appointmentService;
        public string ImageUrl { get; set; }
        public string Comment { get; set; }

        private List<GuideRatingImage> _guideRatingImages;
        private int currentImageIndex;

        private StackPanel _tourEnjoymentPanel;
        private StackPanel _languageKnowledgePanel;
        private StackPanel _tourKnowledgePanel;
        public User Guest { get; set; }

        public RateGuideViewModel(Appointment selectedTour, StackPanel tourKnowledgePanel, StackPanel languageKnowledgePanel,
                                    StackPanel tourEnjoymentPanel, User guest)
        {
            SelectedTour = selectedTour;
            _guideRatingImageService = new GuideRatingImageService();
            _guideRatingService = new GuideRatingService();
            _appointmentService = new AppointmentService();
            _guideRatingImages = new List<GuideRatingImage>();
            currentImageIndex = 0;
            _tourEnjoymentPanel = tourEnjoymentPanel as StackPanel;
            _languageKnowledgePanel = languageKnowledgePanel as StackPanel;
            _tourKnowledgePanel = tourKnowledgePanel as StackPanel;
            Guest = guest;
        }
        private void OnSwipeLeftButtonClick()
        {
            if (currentImageIndex == 0)
            {
                currentImageIndex = _guideRatingImages.Count - 1;
                changePresentImage();
            }
            else
            {
                currentImageIndex--;
                changePresentImage();
            }

        }
        private void OnSwipeRightButtonClick()
        {
            if (currentImageIndex == _guideRatingImages.Count - 1)
            {
                currentImageIndex = 0;
                changePresentImage();
            }
            else
            {
                currentImageIndex++;
                changePresentImage();
            }
        }
        private bool CanSwipeLeftButtonClick()
        {
            return _guideRatingImages.Count() > 1;
        }
        private bool CanSwipeRightButtonClick()
        {
            return (_guideRatingImages.Count() > 1);
        }
        private void changePresentImage()
        {
            if (_guideRatingImages.Count != 0)
            {
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                if (_guideRatingImages[currentImageIndex].Url == "" || _guideRatingImages[currentImageIndex].Url == null)
                {
                    string url = "https://img.icons8.com/?size=512&id=N3wRcSUFuct_&format=png";
                    bitmapimage.UriSource = new Uri(@url, UriKind.Absolute);
                    bitmapimage.EndInit();
                    ImageSource = bitmapimage;
                    OnPropertyChanged(nameof(ImageSource));
                    return;
                }
                bitmapimage.UriSource = new Uri(_guideRatingImages[currentImageIndex].Url, UriKind.Absolute);
                bitmapimage.EndInit();
                ImageSource = bitmapimage;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        private void Submit()
        {
            RadioButton tourKnowledgeButton = _tourKnowledgePanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            RadioButton languageKnowledgeButton = _languageKnowledgePanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            RadioButton tourEnjoymentButton = _tourEnjoymentPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);

            bool radioButtonsSelected = (tourKnowledgeButton != null) && (tourEnjoymentButton != null) && (languageKnowledgeButton != null);
            if (!radioButtonsSelected || (Comment == null))
            {
                MessageBox.Show("You have to fill in each category!", "Alert", MessageBoxButton.OK);
            }
            else
            {
                SaveGuideRating(tourKnowledgeButton, languageKnowledgeButton, tourEnjoymentButton);
                MessageBox.Show("Successfully rated a tour!", "Confirm", MessageBoxButton.OK);
                this.CloseCurrentWindow();

            }
        }
        private void AddPhoto()
        {
            try
            {
                _guideRatingImages.Add(new GuideRatingImage(ImageUrl));
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.UriSource = new Uri(@ImageUrl, UriKind.Absolute);
                bitmapimage.EndInit();

                ImageUrl = "";
                ImageSource = bitmapimage;
                OnPropertyChanged(nameof(ImageSource));
                OnPropertyChanged(nameof(ImageUrl));
            }
            catch (Exception ex)
            {
                MessageBox.Show("The photo you tried to add cannot be loaded!", "Error");
                _guideRatingImages.RemoveAt(_guideRatingImages.Count() - 1);
                ImageUrl = "";
                OnPropertyChanged(nameof(ImageUrl));
            }
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private void SaveGuideRating(RadioButton tourKnowledgeButton, RadioButton languageKnowledgeButton, RadioButton tourEnjoymentButton)
        {
            var guideRating = _guideRatingService.AddRating(int.Parse(tourKnowledgeButton.Name.ToString().Substring(9)),
                                                                int.Parse(languageKnowledgeButton.Name.ToString().Substring(8)),
                                                                int.Parse(tourEnjoymentButton.Name.ToString().Substring(9)),
                                                                SelectedTour.Id,
                                                                Comment.ToString(),
                                                                Guest.Id);
            SelectedTour.IsRated = true;
            _appointmentService.Update(SelectedTour);
            if(_guideRatingImages.Count > 0)
                _guideRatingImageService.AddImagesByGuideRatingId(_guideRatingImages, guideRating.Id);
        }
    }
}
