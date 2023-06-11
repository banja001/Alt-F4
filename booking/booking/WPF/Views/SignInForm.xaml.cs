using booking.Model;
using booking.Repository;
using booking.View.Guest2;
using booking.View.Guest2.Windows;
using booking.View.Guide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.Views.Guide;
using WPF.Views.Owner;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        private readonly UserRepository _repository;
        private readonly OwnerRatingRepository _ownerRatingsRepository;
        private string _userName;

        public bool tutorial { get; set; }
        public string UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
            _ownerRatingsRepository = new OwnerRatingRepository();
            RefreshUsers();

        }

        public void RefreshUsers()
        {
            
            List<OwnerRating> ownerRatings = _ownerRatingsRepository.GetAll();
            List<User> users = _repository.GetAll().ToList();
            double sum, i;
            double AverageRating = 0;
            foreach (User user in users)
            {
                sum = 0;
                i = 0;
                if (user.Role != "Owner") continue;
                foreach (var rating in ownerRatings)
                {
                    if (rating.OwnerId != user.Id) continue;
                    sum += rating.CleanRating + rating.KindRating;
                    i += 1;
                }
                AverageRating= i==0 ? 0 : sum / (i * 2);
                _repository.UpdateById(user.Id, AverageRating >= 4.5 && i >= 3);
            }
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(UserName);

            if(user != null)
            {
                if(user.Password == txtPassword.Password&&!user.IsQuit)
                {

                    if (user.Role == "Owner")
                    {
                        if (user.Tutorial) {
                            MainWindow win = new MainWindow(user.Id);
                            win.Show();
                        }
                        else
                        {
                            TutorialOwner win=new TutorialOwner(user.Id);
                            win.Show();
                        }
                        user.Tutorial = true;
                        _repository.Update(user);
                        this.Close();


                    }
                    else if (user.Role == "Guest1")
                    {
                        Guest1View accomodationOverview = new Guest1View(user.Id,this);
                        accomodationOverview.Show();
                        this.Close();
                    }
                    else if(user.Role == "Guest2")
                    {
                        MainGuest2View mainWindow = new MainGuest2View(user);
                        mainWindow.Show();
                        this.Close();
                    } 
                    else if(user.Role == "Guide")
                    {
                        
                        GuideMainWindow guideWindow = new GuideMainWindow(user);
                        guideWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    if(user.IsQuit)
                        MessageBox.Show("User quitted job!");
                    if(!user.IsQuit)
                        MessageBox.Show("Wrong passwrod!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }
    }
}
