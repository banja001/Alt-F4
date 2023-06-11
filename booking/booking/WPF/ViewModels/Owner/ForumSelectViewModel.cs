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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    public class ForumSelectViewModel : BaseViewModel
    {
        private Forum selectedItem;
        public Forum SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }
        
        public ICommand ForumViewCommand => new RelayCommand(ForumViewClick);
        public ObservableCollection<Forum> ForumList { get; set; }
        public ForumService forumService;
        private ForumCommentService forumCommentService;
        private UserService userService;
        private int ownerId;
        public ForumSelectViewModel(int id)
        {
            ownerId = id;
            forumService = new ForumService();
            forumCommentService = new ForumCommentService();
            userService = new UserService();
            int owner = 0;
            int guest=0;
            List<Forum> forums = forumService.GetAll();
            List<User> lista = userService.GetAll();
            int i = 0;
            foreach(var forum in forumService.GetAll().ToList())
            {
                owner = 0;
                guest = 0;
                foreach(var comment in forumCommentService.GetAll())
                {
                    if (comment.ForumId == forum.Id)
                    {
                        User u = lista.Find(s => s.Id == comment.UserId);
                        if (u.Role == "Owner") owner++;
                        else if (u.Role == "Guest1") guest++;
                    }
                }
                if(owner>=3 && guest >= 3)
                {
                    forums[i].Location += "*";
                }
                i++;
            }


            ForumList = new ObservableCollection<Forum>(forums);
            


        }

        public void ForumViewClick()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items", "Error");
            }
            else
            {
                ForumView win = new ForumView(selectedItem,ownerId);
                MainWindow.w.Main.Content = win;
            }
        }

    }
}
