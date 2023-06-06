using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class ForumViewViewModel : BaseViewModel
    {

        

        private bool open;
        public bool Open
        {
            get { return open; }
            set
            {
                if (value != open)
                {
                    open = value;
                    OnPropertyChanged("Open");
                }

            }
        }


        private string popupText;
        public string PopupText
        {
            get { return popupText; }
            set
            {
                if (value != popupText)
                {
                    popupText = value;
                    OnPropertyChanged("PopupText");
                }

            }
        }

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
        private List<string> fullComments;

        private string selectList;
        public string SelectList
        {
            get { return selectList; }
            set
            {
                if (value != selectList)
                {
                    selectList = value;
                    OnPropertyChanged("SelectList");
                    DisplayPopup();
                }

            }
        }
        
        public Forum forum { get; set; }
        private ForumCommentService commentService;
        private UserService userService;
        private AccommodationService accommodationService;
        private LocationService locationService;
        private ForumCommentService forumCommentService;
        public ObservableCollection<string> comments { get; set; }
        private int i;

        public ICommand PostCommentCommand => new RelayCommand(PostComment);
        
        public void ClosePopup()
        {
            Open = false;
        }
        private int ownerId;
        public ForumViewViewModel(Forum select,int id)
        {
            ownerId = id;
            forum = select;
            commentService=new ForumCommentService();
            userService=new UserService();
            accommodationService=new AccommodationService();
            locationService=new LocationService();
            forumCommentService = new ForumCommentService();

            List<User> users = userService.GetAll();

            fullComments = new List<string>();

            comments = new ObservableCollection<string>();
            i = 1;
            foreach(var comm in commentService.GetAll())
            {
                if (comm.ForumId == forum.Id)
                {
                    User user = users.Find(s => s.Id == comm.UserId);
                    string name=user.Username;
                    string builder;
                    if (comm.Comment.Length >= 10) 
                          builder= i.ToString() + ". ("+user.Role+")" + name + ": " + comm.Comment.Substring(0, 10) + "...";
                    else
                          builder= i.ToString() + ". (" + user.Role + ")" + name + ": " + comm.Comment ;
                    comments.Add(builder);
                    fullComments.Add(comm.Comment);
                    i++;
                }
            }
        }

        public void DisplayPopup()
        {
            Open = false;
            int idlist = Convert.ToInt32(selectList[0]) - 48 - 1;
            PopupText = fullComments[idlist];
            Open = true;
        }

        public void PostComment()
        {
            bool canLeaveComment = false;
            string[] loc=forum.Location.Split(",");
            List<Location> locations = locationService.GetAll();
            
            
           

            int locId = locations.Find(s => s.State == loc[0] && s.City == loc[1]).Id;
            Accommodation acc = accommodationService.GetAll().Find(s => s.LocationId == locId && s.OwnerId == ownerId);

            if (acc == null)
            {
                MessageBox.Show("Only owners with accommodation on this locations can leave comments");
                return;
            }
            ForumComment comm = new ForumComment(forumCommentService.MakeId(),Comment,forum.Id,ownerId);
            forumCommentService.Add(comm);

            User user = userService.GetAll().Find(s => s.Id == comm.UserId);
            string name = user.Username;
            string builder;
            if (comm.Comment.Length >= 10)
                builder = i.ToString() + ". (" + user.Role + ")" + name + ": " + comm.Comment.Substring(0, 10) + "...";
            else
                builder = i.ToString() + ". (" + user.Role + ")" + name + ": " + comm.Comment;
            comments.Add(builder);
            fullComments.Add(comm.Comment);
            i++;
            MessageBox.Show("Comment posted!");
        }
    }
}
