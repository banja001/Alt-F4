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

        public List<Forum> AllForums { get; set; }

        private int userId;

        private readonly LocationService _locationService;
        private readonly ForumService _forumService;
        private readonly ForumCommentService _forumCommentService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand CreateForumCommand => new RelayCommand(CreateForum);
        public ICommand StateSelectionChangedCommand => new RelayCommand(StateSelectionChanged);
        public CreateForumViewModel(int userId)
        {
            _locationService = new LocationService();
            _forumService = new ForumService();
            _forumCommentService = new ForumCommentService();

            States = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            AllForums = _forumService.GetAll();

            FillStateComboBox();
            this.userId = userId;
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
            Forum existingForum = AllForums.Find(f => f.Location == SelectedState + "," + SelectedCity);

            if (existingForum == null)
            {
                int forumId = _forumService.MakeId();

                _forumService.Add(new Forum(forumId, SelectedState + "," + SelectedCity, userId, true));
                _forumCommentService.Add(new ForumComment(_forumCommentService.MakeId(), Comment, forumId, userId));

                MessageBox.Show("You have successfully opened a new forum and left the first comment!");
                CloseWindow();
            }
            else
            {
                if(existingForum.Open)
                    MessageBox.Show("There's already an open forum for that location");
                else
                {
                    existingForum.Open = true;
                    existingForum.CreatorId = userId;

                    _forumService.Update(existingForum);
                    _forumCommentService.Add(new ForumComment(_forumCommentService.MakeId(), Comment, existingForum.Id, userId));
                    MessageBox.Show("Forum u want to create alredy exists, but it's close. You have just opened it again");
                    CloseWindow();
                }
            }
        }
    }
}
