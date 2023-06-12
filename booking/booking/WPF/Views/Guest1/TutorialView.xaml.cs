using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for TutorialView.xaml
    /// </summary>
    public partial class TutorialView : Window
    {
        public TutorialView(string tip)
        {
            InitializeComponent();
            DataContext = this;

            string putanja = "../../../Resources/Videos/" + tip + ".mp4";

            videoElementTutorial.Source = new Uri(putanja, UriKind.Relative);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            videoElementTutorial.Play();
        }
    }
}
