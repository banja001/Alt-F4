using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repository;
using booking.WPF.Views.Guide;
using WPF.Views.Guide;

namespace booking.WPF.ViewModels
{
    class ShowReviewsViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public ObservableCollection<GuideRating> AllCommentsForThatTour { get; set; }
        private readonly AppointmentService _appointmentService;
        public TourRatingDTO SelectedComment { get; set; }
        public ObservableCollection<TourRatingDTO> AllComments { get; set; }
        public ICommand ShowCommand => new RelayCommand(Show,CanShow);
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public string TourName { get; set; }
        public User Guide { get; set; }
        public ICommand TooltipReviewsCommand => new RelayCommand(ReviewsToolTip);

        private Thickness _myMargin;
        public Thickness MyMargin
        {
            get { return _myMargin; }
            set
            {
                _myMargin = value;
                OnPropertyChanged(nameof(MyMargin));
            }
        }

        private bool empty;

        public bool Empty
        {
            get { return empty; }
            set
            {
                if (empty != value)
                {
                    empty = value;
                    OnPropertyChanged(nameof(Empty));
                }
            }
        }

        private bool  reviewsTooltip;

        public bool ReviewsTooltip
        {
            get { return reviewsTooltip; }
            set
            {
                if (reviewsTooltip != value)
                {
                    reviewsTooltip = value;
                    OnPropertyChanged(nameof(ReviewsTooltip));
                }
            }
        }

        public bool DemoOn { get; private set; }

        public void ReviewsToolTip()
        {
            ReviewsTooltip = !ReviewsTooltip;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ShowReviewsViewModel(User guide, AppointmentGuestsDTO appointment,bool demoOn)
        {
            _appointmentService = new AppointmentService();
            AllCommentsForThatTour=new ObservableCollection<GuideRating>(_appointmentService.FindComments(_appointmentService.FindAppointment(appointment.AppointmentId)));
            AllComments =
                new ObservableCollection<TourRatingDTO>(
                    _appointmentService.MakeTourRatings(AllCommentsForThatTour, appointment));
            Guide = guide;
            TourName = _appointmentService.GetName(appointment.AppointmentId);
            MyMargin = new Thickness(0, 13, 182 - CalculateWidth() / 2, 0);
            Empty = true;
            if (AllComments.Count>0)
            {
                Empty = false;
            }
           DemoOn=demoOn;
            if (demoOn)
                DemoIsOn(new CancellationToken());
        }

        private int CalculateWidth()
        {
            string text = TourName;
            double fontSize = 30;
            FontWeight fontWeight = FontWeights.Bold;

            FormattedText formattedText = new FormattedText(text,
                                                            System.Globalization.CultureInfo.CurrentCulture,
                                                            FlowDirection.LeftToRight,
                                                            new Typeface("Arial"),
                                                            fontSize,
                                                            Brushes.Black);
            formattedText.SetFontWeight(fontWeight);

            double width = formattedText.WidthIncludingTrailingWhitespace;
            return Convert.ToInt32(width);
        }
        public bool CanShow()
        {
            return SelectedComment != null;
        }

        public void Show()
        {
            try
            {
                if (SelectedComment != null && !string.IsNullOrEmpty(SelectedComment.TourName))
                {
                    //ExitWindow();
                    Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    if (window != null)
                    {
                        window.Effect = new BlurEffect();
                    }
                    SelectedCommentWindow showComment = new SelectedCommentWindow(SelectedComment, Guide,DemoOn);
                    showComment.ShowDialog();
                    if (!SelectedComment.Rating.IsValid)
                    {
                        
                        AllComments.Add(SelectedComment);
                        AllComments.Remove(SelectedComment);
                    }
                    window.Effect = null;

                }
                else
                    MessageBox.Show("Select tour!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private async Task DemoIsOn(CancellationToken ct)
        {

            ct.ThrowIfCancellationRequested();
            SelectedComment = AllComments[0];

            await Task.Delay(2000, ct);
            Show();
            await Task.Delay(2000, ct);
            ExitWindow();
        }
    }
}
