using booking.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for TutorialOwner.xaml
    /// </summary>
    public partial class TutorialOwner : Window
    {
        public List<string> lista;
        public int i = 0;
        int userid;
        public TutorialOwner(int id)
        {
            InitializeComponent();
            DataContext = this;
            userid = id;
            SetImageSource("../../../Resources/Icons/IconsOwner/StartImage.png");
            lista = new List<string>()
            {
                 "../../../Resources/Icons/IconsOwner/firstImage.png",
                 "../../../Resources/Icons/IconsOwner/SecondImage.png",
                 "../../../Resources/Icons/IconsOwner/ThirdImage.png",
                 "../../../Resources/Icons/IconsOwner/FourthImage.png",
                 "../../../Resources/Icons/IconsOwner/FifthImage.png",
                 "../../../Resources/Icons/IconsOwner/StartImage - Copy.png"
            };

        }
        public void SetImageSource(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@url, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            imageControl.Source = bitmapImage;

        }
        private void EndTutorialClick(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow(userid);
            win.Show();
            this.Close();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            if (i != 6)
            {
                SetImageSource(lista[i]);
                i++;
            }
            if(i==6)
            {
                nextButton.Content = "End tutorial";
                nextButton.Click += EndTutorialClick;
                endButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
