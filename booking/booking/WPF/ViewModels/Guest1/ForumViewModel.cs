using booking.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class ForumViewModel
    {
        private int userId;
        public ICommand CreateForumClickCommand => new RelayCommand(CreateForum);
        public ForumViewModel(int userId)
        {
            this.userId = userId;
        }

        private void CreateForum()
        {
            var createForumWindow = new CreateForumView(userId);
            createForumWindow.ShowDialog();
        }
    }
}
