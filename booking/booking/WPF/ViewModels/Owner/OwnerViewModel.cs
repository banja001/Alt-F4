using application.UseCases;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View;
using booking.View.Owner;
using booking.WPF.ViewModels;
using booking.WPF.Views.Owner;
using Domain.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Repositories;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.XPath;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
   
    public class OwnerViewModel:BaseViewModel
    {
        private string worstAcc= "No accommodation";
        public string WorstAcc
        {
            get
            {
                return worstAcc;
            }
            set
            {
                if (value != worstAcc)
                {
                    worstAcc = value;
                    OnPropertyChanged("WorstAcc");
                }
            }
        }


        private string bestAcc="No accommodation";
        public string BestAcc
        {
            get
            {
                return bestAcc;
            }
            set
            {
                if (value != bestAcc)
                {
                    bestAcc = value;
                    OnPropertyChanged("BestAcc");
                }
            }
        }



        private Visibility visible=Visibility.Collapsed;
        public Visibility Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (value != visible)
                {
                    visible = value;
                    OnPropertyChanged("Visible");
                    
                }
            }
        }


        private bool tooltips;
        public bool Tooltips
        {
            get
            {
                return tooltips;
            }
            set
            {
                if (value != tooltips)
                {
                    tooltips = value;
                    OnPropertyChanged("Tooltips");
                    GlobalVariables.tt = GlobalVariables.tt == false ? true : false;
                    ChangeVisibility();
                }
            }
        }

        private string averageLabel;
        public string AverageLabel {
            get
            {
                return averageLabel;
            }
            set
            {
                if (value != averageLabel)
                {
                    averageLabel = value;
                    OnPropertyChanged("AverageLabel");
                }
            }
        }
        

        private string superOwnerLabel;
        public string SuperOwnerLabel
        {
            get
            {
                return superOwnerLabel;
            }
            set
            {
                if (value != superOwnerLabel)
                {
                    superOwnerLabel = value;
                    OnPropertyChanged("AverageLabel");
                }
            }
        }
        public int OwnerId { get; set; }
        public double AverageRating { get; set; }
        public string UserName { get; set; }

        public AccommodationService accommodationService;
        public List<Accommodation> accommodations;
        public AccommodationImageService accommodationImageService;
        public List<AccommodationImage> accommodationImages;
        public LocationService locationService;
        public List<Location> locations;
        public ReservedDatesService reservedDatesService;
        public List<ReservedDates> reservedDates;
        public Guest1RatingsService guest1RatingsService;
        public List<Guest1Rating> guest1Ratings;
        public UserService userService;
        public List<User> users;
        public OwnerRatingImageService OwnerRatingImageService;
        public List<OwnerRatingImage> OwnerRatingImages;
        public OwnerRatingService OwnerRatingService;
        public List<OwnerRating> OwnerRatings;
        public RenovationDatesService renovationDatesService;
        public ForumNotificationService forumNotificationService;
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }

        private readonly OwnerNotificationsService _ownerNotificationService;
        public static List<OwnerNotification> Notifications { get; set; }
        
        public ICommand NotifyUserCommand => new RelayCommand(NotifyUser);
        public ICommand GeneratePDFCommand => new RelayCommand(GeneratePDF);

        
        public ICommand TypeOfOwnerCommand => new RelayCommand(ClickTypeOfOwner);

        
        private bool ownerTypeTooltip=false;
        public bool OwnerTypeTooltip
        {
            get
            {
                return ownerTypeTooltip;
            }
            set
            {
                if (value != ownerTypeTooltip)
                {
                    ownerTypeTooltip = value;
                    OnPropertyChanged("OwnerTypeTooltip");
                }
            }
        }
        private void ClickTypeOfOwner()
        {
            if (GlobalVariables.tt == true)
            {
                if (ownerTypeTooltip)
                {
                    OwnerTypeTooltip=false;
                    
                }
                else
                {
                    OwnerTypeTooltip = true;
                    RateGuestsTooltip = false;
                    DownloadPDFTooltip = false;
                }
            }
        }
        public ICommand DownloadPDFTooltipCommand => new RelayCommand(ClickDownloadPDF);
        

        private bool downloadPDFTooltip = false;
        public bool DownloadPDFTooltip
        {
            get
            {
                return downloadPDFTooltip;
            }
            set
            {
                if (value != downloadPDFTooltip)
                {
                    downloadPDFTooltip = value;
                    OnPropertyChanged("DownloadPDFTooltip");
                }
            }
        }

        private void ClickDownloadPDF()
        {
            if (GlobalVariables.tt == true)
            {
                if (downloadPDFTooltip)
                {
                    DownloadPDFTooltip = false;
                    
                }
                else
                {
                    DownloadPDFTooltip = true;
                    OwnerTypeTooltip = false;
                    RateGuestsTooltip = false;
                }
            }
        }

        public ICommand RateGuestsTooltipCommand => new RelayCommand(ClickRateGuests);

        private bool rateGuestsTooltip = false;

        public bool RateGuestsTooltip
        {
            get
            {
                return rateGuestsTooltip;
            }
            set
            {
                if (value != rateGuestsTooltip)
                {
                    rateGuestsTooltip = value;
                    OnPropertyChanged("RateGuestsTooltip");
                }
            }
        }
        private void ClickRateGuests()
        {
            if (GlobalVariables.tt == true)
            {
                if (rateGuestsTooltip)
                {
                    
                    RateGuestsTooltip = false;
                }
                else
                {
                    RateGuestsTooltip = true;
                    OwnerTypeTooltip = false;
                    DownloadPDFTooltip = false;
                }
            }
        }

        public bool load;
        public ICommand RateGuestsCommand => new RelayCommand(RateGuests_Click);
        public OwnerViewModel(int id)
        {
            

            OwnerId = id;
       
            CreateInstances();
            UserName = userService.GetUserNameById(id);
            List<ReservedDates> ratingDates = PickDatesForRating();

            List<Guest1RatingDTO> tempList = GetGuestsToRate(ratingDates);
            ListToRate = new ObservableCollection<Guest1RatingDTO>(tempList);

            CalculateAverageRating();
            load = false;
            if (tempList.Count() != 0)
            {
                load = true;
            }

            _ownerNotificationService = new OwnerNotificationsService();
            Notifications = _ownerNotificationService.GetAll();

            if (Notifications.Count != 0)
            {
                NotifyOwner();
            }

            foreach(var notif in forumNotificationService.GetAll())
            {
                if (notif.OwnerId == OwnerId)
                {
                    MessageBox.Show("A forum at location "+notif.Location+ " is now open!");
                }

            }
            forumNotificationService.DeleteAllByOwnerId(OwnerId);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Accommodation maxAcc=new Accommodation();
            Accommodation minAcc=new Accommodation();
            List<Accommodation> accommodations = accommodationService.GetAll();
            
            int busy=0;
            
            int max = -1;
            int min = 100000000;
            DateTime yearStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime yearEnd = new DateTime(DateTime.Now.Year, 12, 31);
            foreach (var accommodation in accommodations)
            {
                busy = 0;
                if (accommodation.OwnerId == OwnerId)
                {
                    foreach(var reservation in reservedDatesService.GetAll())
                    {
                        if (reservation.AccommodationId == accommodation.Id && reservation.StartDate<yearEnd &&  reservation.EndDate>yearStart)
                        {
                            DateTime endDate=reservation.EndDate.Year > DateTime.Now.Year ? new DateTime(DateTime.Now.Year, 12, 31) :reservation.EndDate;
                            DateTime startDate = reservation.StartDate.Year < DateTime.Now.Year ? new DateTime(DateTime.Now.Year, 1, 1) : reservation.StartDate;

                            TimeSpan timeDifference = endDate - startDate;
                            busy += (int)timeDifference.TotalDays;

                        }

                    }
                    if (busy > max)
                    {
                        max = busy;
                        maxAcc = accommodation;
                        
                    }
                    if (busy < min)
                    {
                        min= busy;
                        minAcc = accommodation;
                    }

                }
                
            }
            if (max != -1)
            {


                BestAcc = maxAcc.Name;
                WorstAcc = minAcc.Name;
            }
            
            
        }
        
        public void ChangeVisibility()
        {
            if (GlobalVariables.tt == true) Visible= Visibility.Visible;
            else Visible= Visibility.Collapsed;
        }
        public void NotifyOwner()
        {
            foreach (var notification in Notifications)
            {
                if (notification.OwnerId == OwnerId)
                    MessageBox.Show(notification.ToString());
            }
            _ownerNotificationService.DeleteAllByOwnerId(OwnerId);
        }

        private void CalculateAverageRating()
        {
            double sum = 0;
            int i = 0;
            foreach (var rating in OwnerRatings)
            {
                if (rating.OwnerId == OwnerId)
                {

                    sum += rating.KindRating + rating.CleanRating;
                    i += 1;
                }
            }
            if (i == 0) AverageRating = 0;
            else AverageRating =sum / (i * 2);
            AverageRating = Math.Round(AverageRating,2);////////
            AverageLabel = AverageRating.ToString();
            if (AverageRating >= 4.5 && i >= 3)
            {
                SuperOwnerLabel = "Super Owner**";
                userService.UpdateById(OwnerId, true);
            }
        }

        private List<Guest1RatingDTO> GetGuestsToRate(List<ReservedDates> ratingDates)
        {
            List<Guest1RatingDTO> tempList = new List<Guest1RatingDTO>();
            foreach (ReservedDates date in ratingDates)
            {
                Guest1RatingDTO guestsToRate = new Guest1RatingDTO();
                guestsToRate.DateId = date.Id;
                guestsToRate.StartDate = date.StartDate.ToShortDateString();
                guestsToRate.EndDate = date.EndDate.ToShortDateString();
                guestsToRate.GuestName = users.Find(u => u.Id == date.UserId).Username;
                guestsToRate.AccommodationName = accommodations.Find(u => u.Id == date.AccommodationId).Name;
                //guestsToRate.ReservationId=date.
                tempList.Add(guestsToRate);
            }
            return tempList;
        }
        private void CreateInstances()
        {
            userService = new UserService();
            users = userService.GetAll();

            accommodationService = new AccommodationService();
            accommodations = accommodationService.GetAll();
            accommodationImageService = new AccommodationImageService();
            accommodationImages = accommodationImageService.GetAll();
            locationService = new LocationService();
            locations = locationService.GetAll();

            reservedDatesService = new ReservedDatesService();
            reservedDates = reservedDatesService.GetAll();
            guest1RatingsService = new Guest1RatingsService();
            guest1Ratings = guest1RatingsService.GetAll();
            OwnerRatingImageService = new OwnerRatingImageService();
            OwnerRatingImages = OwnerRatingImageService.GetAll();
            OwnerRatingService = new OwnerRatingService();
            OwnerRatings = OwnerRatingService.GetAll();

            renovationDatesService = new RenovationDatesService();

            forumNotificationService = new ForumNotificationService();
        }

        private void NotifyUser()
        {
            if (GlobalVariables.a == true && load==true)
            {
                MessageBox.Show("You have unrated guests", "Message");
                GlobalVariables.a = false;
            }
        }
        public List<ReservedDates> PickDatesForRating()
        {
            List<ReservedDates> ratingDates = new List<ReservedDates>();
            foreach (ReservedDates reservedDate in reservedDates)
            {
                accommodations.Find(m => m.Id == reservedDate.AccommodationId);
                if (DateTime.Today >= reservedDate.EndDate && DateTime.Today < reservedDate.EndDate.AddDays(5) && reservedDate.RatedByOwner == false
                    && accommodations.Find(m => m.Id == reservedDate.AccommodationId).OwnerId == OwnerId)
                {
                    ratingDates.Add(reservedDate);
                }

            }
            return ratingDates;
        }

        private void RateGuests_Click()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items", "Error");
            }
            else
            {
                RateGuestWindow win = new RateGuestWindow(this,SelectedItem);
                MainWindow.w.Main.Content = win;
            }

        }

        private void GeneratePDF()
        {
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            
            
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 12);
            XFont font2 = new XFont("Arial", 14,XFontStyle.Bold);

            XBrush brush = XBrushes.Black;

            XImage image = XImage.FromFile("../../../Resources/Icons/logo.png"); 
            gfx.DrawImage(image, 100, 10, 100, 100);


            User u = userService.GetById(OwnerId);
            int xcord = 194;
            int ycord = 87;
            gfx.DrawString("Accommodation stats", new XFont("Arial", 22,XFontStyle.Bold), XBrushes.Green, new XPoint(190, 70));

            gfx.DrawString("Name: "+u.Username, new XFont("Arial", 10), XBrushes.Gray, new XPoint(xcord, ycord));
            gfx.DrawString("Role: " + u.Role, new XFont("Arial", 10), XBrushes.Gray, new XPoint(xcord, ycord+10));
            if(u.Super)
                gfx.DrawString("SuperOwner: " + "Yes", new XFont("Arial", 10), XBrushes.Gray, new XPoint(xcord, ycord+20));
            else
                gfx.DrawString("SuperOwner: " + "No", new XFont("Arial", 10), XBrushes.Gray, new XPoint(xcord, ycord+20));
            
            gfx.DrawString("DateTime: " + DateTime.Now.ToString(), new XFont("Arial", 10), XBrushes.Gray, new XPoint(xcord, ycord+30));

            gfx.DrawLine(new XPen(XColor.FromArgb(50,30,200)),new XPoint(150,130),new XPoint(450,130));
            gfx.DrawString("Accommodation name", font2, XBrushes.Black, new XPoint(160, 150));
            gfx.DrawString("Average rating", font2, XBrushes.Black, new XPoint(330, 150));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(150, 157), new XPoint(450, 157));
            int xpos1 = 180;
            int xpos2 = 340;
            int ypos = 170;
            int i = 80;
            double grade;
            int j;
            List<ReservedDates> reservations = reservedDatesService.GetAll();
            foreach(var acc in accommodationService.GetAll())
            {
                grade = 0;
                j = 0;
                if (acc.OwnerId == OwnerId)
                {
                    foreach(var rating in OwnerRatingService.GetAll())
                    {
                        
                            if (reservations.Find(s => s.Id == rating.ReservationId).AccommodationId == acc.Id)
                            {
                                grade += rating.KindRating + rating.CleanRating;
                                j++;
                            }
                        

                    }
                    
                    grade = j==0 ? 0 : grade / (j*2);
                    gfx.DrawString(acc.Name, font, brush, new XPoint(xpos1, ypos));
                    gfx.DrawString(Math.Round(grade,3).ToString(), font, brush, new XPoint(xpos2, ypos));
                    ypos += 7;
                    gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(150, ypos), new XPoint(450, ypos));
                    ypos += 13;
                }
                
            }
            ypos -= 13;
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(150, 130), new XPoint(150, ypos));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(450, 130), new XPoint(450, ypos));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(320, 130), new XPoint(320, ypos));



            string name = $"ownerRatings_{Guid.NewGuid()}.pdf";
            document.Save(name);
            System.Diagnostics.Process.Start("explorer", name);

        }
    }
}
