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
        
        public ICommand Grid_MouseLeftButtonDownCommand => new RelayCommand(ClosePopup);
        
        public ICommand ReportCommentTooltipCommand => new RelayCommand(ReportCommentTooltip);

        private bool reportTooltip = false;
        public bool ReportTooltip
        {
            get
            {
                return reportTooltip;
            }
            set
            {
                if (value != reportTooltip)
                {
                    reportTooltip = value;
                    OnPropertyChanged("ReportTooltip");
                }
            }
        }

        private void ReportCommentTooltip()
        {
            if (GlobalVariables.tt == true)
            {
                if (reportTooltip)
                {
                    ReportTooltip = false;

                }
                else
                {
                    ReportTooltip = true;

                }
            }
        }

        private string infoLabel;
        public string InfoLabel
        {
            get { return infoLabel; }
            set
            {
                if (value != infoLabel)
                {
                    infoLabel = value;
                    OnPropertyChanged("InfoLabel");
                }

            }
        }

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
        public List<ForumComment> forumComments;
        
        public Forum forum { get; set; }
        private ForumCommentService commentService;
        private UserService userService;
        private AccommodationService accommodationService;
        private LocationService locationService;
        private ForumCommentService forumCommentService;
        private ReportedComentsService reportedComentsService;
        public ObservableCollection<string> comments { get; set; }
        private int i;
        public ICommand ReportCommentCommand => new RelayCommand(ReportComment);
        public ICommand PostCommentCommand => new RelayCommand(PostComment);
        
        public void ClosePopup()
        {
            Open = false;
        }
        private int ownerId;
        public ForumViewViewModel(Forum select,int id)
        {
            if(select.Open==true)
                InfoLabel = "Name: " + select.Location + "- Status: " + "Open";
            else
                InfoLabel = "Name: " + select.Location + "- Status: " + "Closed";
            ownerId = id;
            forum = select;
            commentService=new ForumCommentService();
            userService=new UserService();
            accommodationService=new AccommodationService();
            locationService=new LocationService();
            forumCommentService = new ForumCommentService();
            reportedComentsService=new ReportedComentsService();

            List<User> users = userService.GetAll();

            fullComments = new List<string>();
            forumComments=new List<ForumComment>();

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
                    forumComments.Add(comm);
                    i++;
                }
            }
        }

        public void DisplayPopup()
        {
            Open = false;
            string br = selectList.Split(".")[0];
            int idlist = Convert.ToInt32(br)  - 1;
            PopupText = "Reports:" + forumComments[idlist].Reports +" "+fullComments[idlist];
            Open = true;
        }
        public void ReportComment()
        {
            Open = false;
            if (SelectList == null)
            {
                MessageBox.Show("Please select a comment you want to report");
                return;
            }

            int num=Convert.ToInt32(SelectList.Split(".")[0])-1;
            int idcom=forumComments[num].Id;
            if(reportedComentsService.GetAll().Find(s=> s.UserId==ownerId && s.ForumId==forum.Id && s.CommentId == idcom) == null)
            {
                commentService.Update(idcom);

                reportedComentsService.Add(new ReportedComents(reportedComentsService.MakeId(), forum.Id,idcom,ownerId));
                forumComments[num].Reports+=1;
                MessageBox.Show("Comment was reported");
                return;
            }
            MessageBox.Show("Comment already reported by you");
        }
        public void PostComment()
        {
            Open=false;
            if (forum.Open == true)
            {
                bool canLeaveComment = false;
                string[] loc = forum.Location.Split(",");
                List<Location> locations = locationService.GetAll();




                int locId = locations.Find(s => s.State == loc[0] && s.City == loc[1]).Id;
                Accommodation acc = accommodationService.GetAll().Find(s => s.LocationId == locId && s.OwnerId == ownerId);

                if (acc == null)
                {
                    MessageBox.Show("Only owners with accommodation on this locations can leave comments");
                    return;
                }
                ForumComment comm = new ForumComment(forumCommentService.MakeId(), Comment, forum.Id, ownerId);
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
                forumComments.Add(comm);
                i++;
                MessageBox.Show("Comment posted!");
            }
            else
            {
                MessageBox.Show("Forum closed!");
            }
        }
    }
}
