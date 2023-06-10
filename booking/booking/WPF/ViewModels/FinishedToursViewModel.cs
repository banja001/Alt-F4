﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Model;
using booking.WPF.Views.Guide;
using WPF.Views.Guide;

namespace booking.WPF.ViewModels
{
    public class FinishedToursViewModel : BaseViewModel,INotifyPropertyChanged
    {
        public ObservableCollection<int> Years { get; set; }
        public int SelectedYear { get; set; }
        public List<AppointmentGuestsDTO> FinishedTours { get; set; }
        public ObservableCollection<AppointmentGuestsDTO> MostVisitedTour { get; set; }
        public AppointmentGuestsDTO SelectedTour { get; set; }
        private readonly AppointmentService _appointmentService;
        private User Guide { get; set; }
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public FinishedToursViewModel(User guide)
        {
            _appointmentService = new AppointmentService();
            Years = _appointmentService.FindAllYears(guide.Id);
            FinishedTours = _appointmentService.CreateListOfFinishedTours(guide.Id);
            MostVisitedTour =new ObservableCollection<AppointmentGuestsDTO>();
            SelectedYear = Years[0];
            Guide = guide;
        }

        public ICommand ShowReviewsCommand => new RelayCommand(ShowReviews, CanShow);
        public ICommand ShowStatisticsCommand => new RelayCommand(ShowStatistics, CanShow);
        public ICommand FindCommand => new RelayCommand(FindMostVisitedTour);
        public ICommand TooltipFindCommand => new RelayCommand(FindtoolTip);
        public ICommand TooltipAllToursCommand => new RelayCommand(AllTourstoolTip);

        private bool allToursToolTip;

        public bool AllToursTooltip
        {
            get { return allToursToolTip; }
            set
            {
                if (allToursToolTip != value)
                {
                    allToursToolTip = value;
                    OnPropertyChanged(nameof(AllToursTooltip));
                }
            }
        }

        private bool findTooltip;

        public bool FindTooltip
        {
            get { return findTooltip; }
            set
            {
                if (findTooltip != value)
                {
                    findTooltip = value;
                    OnPropertyChanged(nameof(FindTooltip));
                }
            }
        }
        public void FindtoolTip()
        {
            FindTooltip = !FindTooltip;
        }

        public void AllTourstoolTip()
        {
            AllToursTooltip = !AllToursTooltip;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ShowReviews()
        {
            try
            {
                if (SelectedTour != null && !string.IsNullOrEmpty( SelectedTour.Name))
                {
                    Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    if (window != null)
                    {
                        window.Effect = new BlurEffect();
                    }
                    ShowReviewsWindow showReviews = new ShowReviewsWindow(Guide, SelectedTour);
                    showReviews.ShowDialog();
                    window.Effect = null;
                }
                else
                    MessageBox.Show("Select tour!","Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public bool CanShow()
        {
            return SelectedTour!=null;
        }

        public void ShowStatistics()
        {
            try
            {
                if (SelectedTour != null && !string.IsNullOrEmpty(SelectedTour.Name))
                {
                    Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    if (window != null)
                    {
                        window.Effect = new BlurEffect();
                    }
                    ShowStatisticsWindow showStatistics = new ShowStatisticsWindow(SelectedTour);
                    showStatistics.ShowDialog();
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

        public void FindMostVisitedTour()
        {
            try
            {
                MostVisitedTour.Clear();
                MostVisitedTour.Add(_appointmentService.FindMostVisitedTour(Guide.Id, SelectedYear.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
