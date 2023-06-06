using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
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

        private Forum selectedForum;

        private int userId;

        private readonly ForumCommentService _forumCommentService;
        private readonly UserService _userService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand PostCommentClickCommand => new RelayCommand(PostComment);
        public ForumCommentsViewModel(Forum selectedForum, int userId)
        {
            _forumCommentService = new ForumCommentService();
            _userService = new UserService();

            CommentDTOs = new ObservableCollection<ForumCommentUserDTO>();
            this.selectedForum = selectedForum;

            this.userId = userId;

            LoadComments();

            ForumLocationComments = selectedForum.Location + " - Comments";
        }

        private void LoadComments()
        {
            List<ForumComment> allComments = _forumCommentService.GetByForumId(selectedForum.Id); //treb dodati nesto kao oznaku ko je posetio
            
            foreach(var comment in allComments)
            {
                User user = _userService.GetById(comment.UserId);
                CommentDTOs.Add(new ForumCommentUserDTO(user.Username, user.Role, comment.Comment, selectedForum.Id));
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
            CommentDTOs.Add(new ForumCommentUserDTO(user.Username, user.Role, NewComment, selectedForum.Id));

            NewComment = ""; 
        }
    }
}
