using application.UseCases;
using booking.Commands;
using booking.Model;
using Domain.Model;
using Microsoft.Expression.Interactivity.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class ForumViewModel
    {
        public static ObservableCollection<Forum> MyForums { get; set; }
        public static ObservableCollection<Forum> AllForums { get; set; }
        public Forum SelectedMyForum { get; set; }
        public Forum SelectedFromAllForums { get; set; }
        public bool CloseForumEnabled { get; set; }

        private int userId;

        private readonly ForumService _forumService;
        private readonly ForumCommentService _forumCommentService;
        private readonly UserService _userService;
        public ICommand CreateForumClickCommand => new RelayCommand(CreateForum);
        public ICommand CloseFormCommand => new RelayCommand(CloseForm);
        public ICommand OpenCommentsMyFormCommand => new RelayCommand(OpenCommentsMyForm);
        public ICommand OpenCommentsCommand => new RelayCommand(OpenComments);
        public ForumViewModel(int userId)
        {
            _forumService = new ForumService();
            _forumCommentService = new ForumCommentService();
            _userService = new UserService();

            MyForums = new ObservableCollection<Forum>(_forumService.GetByCreatorId(userId));
            AllForums = new ObservableCollection<Forum>(_forumService.GetAll());

            SetVeryUsefulCheckBoxes();

            this.userId = userId;
        }

        private void SetVeryUsefulCheckBoxes()
        {
            foreach(Forum forum in AllForums)
            {
                if (forum.VeryUseful)
                {
                    int numOfGuestsComments = 0;
                    int numOfOwnersComments = 0;

                    List<ForumComment> comments = _forumCommentService.GetByForumId(forum.Id);

                    foreach (var comment in comments)
                    {
                        User user = _userService.GetById(comment.UserId);

                        if (user.Role.Contains("Guest"))
                            numOfGuestsComments++;
                        else
                            numOfOwnersComments++;
                    }

                    if (numOfGuestsComments >= 20 && numOfOwnersComments >= 10)
                    {
                        forum.VeryUseful = true;
                        _forumService.Update(forum);
                    }
                }
            }
        }

        private void CreateForum()
        {
            var createForumWindow = new CreateForumView(userId);
            createForumWindow.ShowDialog();
        }

        private void CloseForm()
        {
            if(!SelectedMyForum.Open)
            {
                MessageBox.Show("This forum has already been closed");
                return;
            }

            SelectedMyForum.Open = false;

            int myForumIndx = MyForums.IndexOf(MyForums.First(f => f.Location == SelectedMyForum.Location));
            MyForums[myForumIndx].Open = false;
            int allForumsIndx = AllForums.IndexOf(AllForums.First(f => f.Location == SelectedMyForum.Location));
            AllForums[allForumsIndx].Open = false;

            _forumService.Update(SelectedMyForum);
            MessageBox.Show("You have successfully close forum on " + SelectedMyForum.Location);
        }
        private void OpenCommentsMyForm()
        {
            if(SelectedMyForum == null)
            {
                MessageBox.Show("You have to select a forum before you can leave a comment");
                return;
            }
            var forumCommentWindow = new ForumCommentsView(SelectedMyForum, userId);
            forumCommentWindow.ShowDialog();
        }

        private void OpenComments()
        {
            if (SelectedFromAllForums == null)
            {
                MessageBox.Show("You have to select a forum before you can leave a comment");
                return;
            }
            var forumCommentWindow = new ForumCommentsView(SelectedFromAllForums, userId);
            forumCommentWindow.ShowDialog();
        }
    }
}
