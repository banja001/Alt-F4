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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _userName;

        

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
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(UserName);

            if(user != null)
            {
                if(user.Password == txtPassword.Password)
                {

                    if (user.Role == "Owner")
                    {
                        OwnerWindow win=new OwnerWindow(user.Id);
                        win.Show();
                        this.Close();
                    }
                    else if (user.Role == "Guest1")
                    {
                        Guest1View accomodationOverview = new Guest1View(user.Id);
                        accomodationOverview.Show();
                        this.Close();
                    }
                    else if(user.Role == "Guest2")
                    {
                        //Guest2Overview guest2Window = new Guest2Overview(user);
                        //guest2Window.Show();

                        MainGuest2View mainWindow = new MainGuest2View(user);
                        mainWindow.Show();
                        this.Close();
                    } 
                    else if(user.Role == "Guide")
                    {
                            
                        GuideWindow guideWindow = new GuideWindow(user);
                        guideWindow.ShowDialog();
                        //this.Close();
                    }
                }
                else
                {
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
