using application.UseCases;
using booking.application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ForumCommentsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public string ForumLocationComments { get; set; }
        public ObservableCollection<ForumCommentUserDTO> CommentDTOs { get; set; }

        private string newComment;
        public string NewComment 
        {
            get
            {
                return newComment;
            }
            set
            {
                if(newComment != value)
                {
                    newComment = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool TxtbEnabled { get; set; }

        private bool postButtonEnabled;
        public bool PostButtonEnabled 
        {
            get { return postButtonEnabled; }
            set
            {
                if(postButtonEnabled != value)
                {
                    postButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private Forum selectedForum;

        private int userId;

        private readonly ForumCommentService _forumCommentService;
        private readonly UserService _userService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;
        private readonly TourAttendanceService _tourAttendanceService;
        private readonly ReservationTourService _reservationTourService;
        private readonly TourService _tourService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand PostCommentClickCommand => new RelayCommand(PostComment);
        public ICommand TextForCommentChangedCommand => new RelayCommand(TextForCommentChanged);
        public ForumCommentsViewModel(Forum selectedForum, int userId)
        {
            _forumCommentService = new ForumCommentService();
            _userService = new UserService();
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();
            _locationService = new LocationService();
            _tourAttendanceService = new TourAttendanceService();
            _reservationTourService = new ReservationTourService();
            _tourService = new TourService();

            CommentDTOs = new ObservableCollection<ForumCommentUserDTO>();
            this.selectedForum = selectedForum;

            this.userId = userId;

            SetEnable();

            LoadComments();

            ForumLocationComments = selectedForum.Location + " - Comments";
        }

        private void SetEnable()
        {
            if(selectedForum == null)
            {
                MessageBox.Show("You have to choose a forumbefore u can interact with it");
                return;
            }
            TxtbEnabled = selectedForum.Open;
            PostButtonEnabled = false;
        }

        private void LoadComments()
        {
            if (selectedForum == null)
            {
                MessageBox.Show("You have to choose a forumbefore u can interact with it");
                return;
            }

            List<ForumComment> allComments = _forumCommentService.GetByForumId(selectedForum.Id);
            
            foreach(var comment in allComments)
            {
                List<Location> locations = GetVisitedLocations(comment);

                bool visited = locations.Find(l => (l.State + "," + l.City).Equals(selectedForum.Location)) == null ? false : true;

                User user = _userService.GetById(comment.UserId);
                CommentDTOs.Add(new ForumCommentUserDTO(user.Username, user.Role, comment.Comment, selectedForum.Id, visited));
            }
        }

        private List<Location> GetVisitedLocations(ForumComment comment)
        {
            List<Location> locations = new List<Location>();

            GetLocationsOfAccommodations(comment, locations);
            GetLocationsOfTours(comment, locations);

            return locations;
        }

        private void GetLocationsOfTours(ForumComment comment, List<Location> locations)
        {
            List<TourAttendance> tourAttendances = _tourAttendanceService.GetAll();
            List<ReservationTour> reservationTours = _reservationTourService.GetByUserId(comment.UserId);
            List<ReservationTour> resTours = new List<ReservationTour>();

            foreach (var rTour in reservationTours)
            {
                if (tourAttendances.Find(t => t.Guest.Id == rTour.Id) != null)
                    resTours.Add(rTour);
            }

            foreach (var tour in resTours)
            {
                tour.Tour = _tourService.GetById(tour.Tour.Id);
                locations.Add(_locationService.GetById(tour.Tour.Location.Id));
            }
        }

        private void GetLocationsOfAccommodations(ForumComment comment, List<Location> locations)
        {
            List<ReservedDates> reservedDates = _reservedDatesService.GetByGuestId(comment.UserId);
            List<Accommodation> accommodations = new List<Accommodation>();

            foreach (var date in reservedDates)
            {
                accommodations.Add(_accommodationService.GetById(date.AccommodationId));
            }

            foreach (var accommodation in accommodations)
            {
                locations.Add(_locationService.GetById(accommodation.LocationId));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow()
        {
            CloseCurrentWindow();
        }

        private void PostComment()
        {
            User user = _userService.GetById(userId);

            _forumCommentService.Add(new ForumComment(_forumCommentService.MakeId(), NewComment, selectedForum.Id, userId));
            CommentDTOs.Add(new ForumCommentUserDTO(user.Username, user.Role, NewComment, selectedForum.Id, true));

            NewComment = ""; 
        }
        private void TextForCommentChanged()
        {
            PostButtonEnabled = NewComment.Equals("") ? false : true;
        }
    }
}
