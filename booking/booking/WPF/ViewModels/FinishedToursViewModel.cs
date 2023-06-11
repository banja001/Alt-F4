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
using System.Windows.Media.Effects;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Model;
using booking.WPF.Views.Guide;
using Domain.DTO;
using WPF.Views.Guide;

namespace booking.WPF.ViewModels
{
    public class FinishedToursViewModel : BaseViewModel,INotifyPropertyChanged
    {
        public ObservableCollection<int> Years { get; set; }
        public int SelectedYear { get; set; }
        public ObservableCollection<AppointmentGuestsDTO> FinishedTours { get; set; }
        public ObservableCollection<AppointmentGuestsDTO> MostVisitedTour { get; set; }
        private AppointmentGuestsDTO selectedTour;
        public AppointmentGuestsDTO SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }
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
            FinishedTours=new ObservableCollection<AppointmentGuestsDTO>();
            foreach (var a in _appointmentService.CreateListOfFinishedTours(guide.Id))
            {
                FinishedTours.Add(a);
            }  
            MostVisitedTour =new ObservableCollection<AppointmentGuestsDTO>();
            if(Years.Count>0)
                SelectedYear = Years[0];
            Guide = guide;
            DemoName = "Demo";
        }
        private string demoName;
        public string DemoName
        {
            get { return demoName; }
            set
            {
                if (demoName != value)
                {
                    demoName = value;
                    OnPropertyChanged(nameof(DemoName));
                }
            }
        }
        private CancellationTokenSource cts = new CancellationTokenSource();

        public ICommand ShowReviewsCommand => new RelayCommand(ShowReviews, CanShow);
        public ICommand ShowStatisticsCommand => new RelayCommand(ShowStatistics, CanShow);
        public ICommand FindCommand => new RelayCommand(FindMostVisitedTour);
        public ICommand TooltipFindCommand => new RelayCommand(FindtoolTip);
        public ICommand TooltipAllToursCommand => new RelayCommand(AllTourstoolTip);
        public ICommand DemoCommand => new RelayCommand(StartStopDemo);

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
                    ShowReviewsWindow showReviews = new ShowReviewsWindow(Guide, SelectedTour,DemoOn);
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
                    ShowStatisticsWindow showStatistics = new ShowStatisticsWindow(SelectedTour,DemoOn);
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
        private bool demoOn;
        public bool DemoOn
        {
            get { return demoOn; }
            set
            {
                demoOn = value;
                OnPropertyChanged(nameof(DemoOn));
            }
        }
        private void StartStopDemo()
        {
            if (DemoOn)
            {
               
                cts.Cancel();
                DemoOn = !DemoOn;
                DemoName = "Demo";
                MessageBox.Show("Demo has been stopped!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
               
                cts = new CancellationTokenSource();
                DemoOn = !DemoOn;
                DemoIsOn(cts.Token);
                DemoName = "Stop";
            }
        }
        private async Task DemoIsOn(CancellationToken ct)
        {

            if (DemoOn)
            {

                ct.ThrowIfCancellationRequested();
                StartDemoWindow sdw = new StartDemoWindow();
                sdw.Show();

                await Task.Delay(2000, ct);
                sdw.Close();
                SelectedYear = 2023;

                await Task.Delay(2000, ct);
                FindMostVisitedTour();
                await Task.Delay(1000, ct);
                SelectedTour = FinishedTours[0];
                await Task.Delay(2000, ct);
                ShowStatistics();
                await Task.Delay(2000, ct);
                ShowReviews();
                
                await Task.Delay(1000, ct);
                FinishedDemoWindow fdw = new FinishedDemoWindow();
                fdw.Show();

                await Task.Delay(2000, ct);
                fdw.Close();
                DemoOn = !DemoOn;
                DemoName = "Demo";

            }
        }
    }
}
