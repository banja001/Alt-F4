using application.UseCases;
using booking.application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class CreateForumViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }
        public string Comment { get; set; }

        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private bool citiesComboBoxEnabled;
        public bool CitiesComboBoxEnabled
        {
            get { return citiesComboBoxEnabled; }
            set
            {
                if (citiesComboBoxEnabled != value)
                {
                    citiesComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly LocationService _locationService;
        private readonly ForumService _forumService;
        private readonly ForumCommentService _forumCommentService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand CreateForumCommand => new RelayCommand(CreateForum);
        public ICommand StateSelectionChangedCommand => new RelayCommand(StateSelectionChanged);
        public CreateForumViewModel()
        {
            _locationService = new LocationService();
            _forumService = new ForumService();
            _forumCommentService = new ForumCommentService();

            States = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();

            FillStateComboBox();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FillStateComboBox()
        {
            List<Location> locations = _locationService.GetAll();
            foreach (Location location in locations)
            {
                String state = location.State;
                if (!States.Contains(state))
                    States.Add(state);
            }
        }

        private void StateSelectionChanged()
        {
            List<Location> locations = _locationService.GetAll();

            while (Cities.Count > 0)
            {
                Cities.RemoveAt(0);
            }

            foreach (Location location in locations)
            {
                String city = location.City;
                bool isValid = !Cities.Contains(city) && SelectedState.Equals(location.State);
                if (isValid)
                    Cities.Add(city);
            }

            CitiesComboBoxEnabled = true;
        }

        private void CloseWindow()
        {
            this.CloseCurrentWindow();
        }

        private void CreateForum()
        {
            MessageBox.Show(SelectedState + ", " + SelectedCity + "\n" + Comment);
        }
    }
}
