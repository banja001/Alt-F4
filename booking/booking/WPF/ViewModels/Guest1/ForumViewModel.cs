using application.UseCases;
using booking.Commands;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class ForumViewModel
    {
        public ObservableCollection<Forum> MyForums { get; set; }
        public ObservableCollection<Forum> AllForums { get; set; }
        public Forum SelectedMyForum { get; set; }

        private int userId;

        private readonly ForumService _forumService;
        public ICommand CreateForumClickCommand => new RelayCommand(CreateForum);
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
    }
}
