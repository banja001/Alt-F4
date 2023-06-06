using application.UseCases;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WPF.ViewModels.Owner
{
    public class ForumViewViewModel : BaseViewModel
    {
        private string comment;
        

        public string Comment
        {
            get { return comment; }
            set {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }

            }
        }
        public Forum forum { get; set; }
        private ForumCommentService commentService;
        private UserService userService;
        public ObservableCollection<string> comments { get; set; }
        public ForumViewViewModel(Forum select)
        {
            forum = select;
            commentService=new ForumCommentService();
            userService=new UserService();
            List<User> users = userService.GetAll();
            comments = new ObservableCollection<string>();

            foreach(var comm in commentService.GetAll())
            {
                if (comm.ForumId == forum.Id)
                {
                    string name=users.Find(s=>s.Id==comm.UserId).Username;
                    comments.Add(name+": "+comm.Comment.Substring(0,10)+"...");
                }
            }
        }
    }
}
