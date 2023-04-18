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

namespace booking.WPF.ViewModels
{
    public class RateGuideViewModel : BaseViewModel
    {
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ICommand AddPhotoCommand => new RelayCommand(AddPhoto);
        public ICommand SubmitCommand => new RelayCommand(Submit);
        public Appointment SelectedTour { get; set; }

        private readonly GuideRatingImageService _guideRatingImageService;
        private readonly GuideRatingService _guideRatingService;
        private readonly AppointmentService _appointmentService;
        public string ImageUrl { get; set; }
        public string Comment { get; set; }

        private List<GuideRatingImage> _guideRatingImages;

        private StackPanel _tourEnjoymentPanel;
        private StackPanel _languageKnowledgePanel;
        private StackPanel _tourKnowledgePanel;
        public RateGuideViewModel(Appointment selectedTour, StackPanel tourKnowledgePanel, StackPanel languageKnowledgePanel,
                                    StackPanel tourEnjoymentPanel)
        {
            SelectedTour = selectedTour;
            _guideRatingImageService = new GuideRatingImageService();
            _guideRatingService = new GuideRatingService();
            _appointmentService = new AppointmentService();
            _guideRatingImages = new List<GuideRatingImage>();

            _tourEnjoymentPanel = tourEnjoymentPanel as StackPanel;
            _languageKnowledgePanel = languageKnowledgePanel as StackPanel;
            _tourKnowledgePanel = tourKnowledgePanel as StackPanel;

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
            _guideRatingImages.Add(new GuideRatingImage(ImageUrl));
            ImageUrl = "";
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
                                                                Comment.ToString());
            SelectedTour.IsRated = true;
            _appointmentService.Update(SelectedTour);
            if(_guideRatingImages.Count > 0)
                _guideRatingImageService.AddImagesByGuideRatingId(_guideRatingImages, guideRating.Id);
        }
    }
}
