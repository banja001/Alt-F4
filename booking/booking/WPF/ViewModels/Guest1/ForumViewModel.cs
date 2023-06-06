using application.UseCases;
using booking.Commands;
using Domain.Model;
using Microsoft.Expression.Interactivity.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private int userId;

        private readonly ForumService _forumService;
        public ICommand CreateForumClickCommand => new RelayCommand(CreateForum);
        public ICommand CloseFormCommand => new RelayCommand(CloseForm);
        public ICommand OpenCommentsMyFormCommand => new RelayCommand(OpenCommentsMyForm);
        public ICommand OpenCommentsCommand => new RelayCommand(OpenComments);
        public ForumViewModel(int userId)
        {
            _forumService = new ForumService();

            MyForums = new ObservableCollection<Forum>(_forumService.GetByCreatorId(userId));
            AllForums = new ObservableCollection<Forum>(_forumService.GetAll());

            this.userId = userId;
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
            var forumCommentWindow = new ForumCommentsView(SelectedMyForum, userId);
            forumCommentWindow.ShowDialog();
        }

        private void OpenComments()
        {
            var forumCommentWindow = new ForumCommentsView(SelectedFromAllForums, userId);
            forumCommentWindow.ShowDialog();
        }
    }
}
