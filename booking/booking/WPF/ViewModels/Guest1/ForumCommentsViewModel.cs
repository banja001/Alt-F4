using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ForumCommentsViewModel : BaseViewModel
    {
        public string ForumLocationComments { get; set; }
        public ObservableCollection<ForumCommentUserDTO> CommentDTOs { get; set; }

        private Forum selectedForum;

        private readonly ForumCommentService _forumCommentService;
        private readonly UserService _userService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ForumCommentsViewModel(Forum selectedForum)
        {
            _forumCommentService = new ForumCommentService();
            _userService = new UserService();

            CommentDTOs = new ObservableCollection<ForumCommentUserDTO>();
            this.selectedForum = selectedForum;

            LoadComments();

            ForumLocationComments = selectedForum.Location + " - Comments";
        }

        private void LoadComments()
        {
            List<ForumComment> allComments = _forumCommentService.GetByForumId(selectedForum.Id);//samo komentare za taj forum
            

            foreach(var comment in allComments)
            {
                User user = _userService.GetById(comment.UserId);
                CommentDTOs.Add(new ForumCommentUserDTO(user.Username, user.Role, comment.Comment, selectedForum.Id));
            }
        }

        private void CloseWindow()
        {
            CloseCurrentWindow();
        }
    }
}
