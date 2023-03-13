using System;
using System.Collections.Generic;
using System.Linq;
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
using booking.Model;

namespace booking.View.Guest2
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        public Guest2Overview(User user)
        {
            InitializeComponent();
            welcome.Header = "Welcome " + user.Username.ToString();
        }
        private void SetContentToDefault(TextBox selectedTextbox, string defaultText)
        {
            if (selectedTextbox.Text.Equals(""))
            {
                selectedTextbox.Text = defaultText;
                selectedTextbox.Foreground = Brushes.LightGray;
            }
        }
        private void RemoveContent(TextBox selectedTextBox, string defaultText)
        {
            if (selectedTextBox.Text.Equals(defaultText))
            {
                selectedTextBox.Text = "";
                selectedTextBox.Foreground = Brushes.Black;
            }
        }
        private void PeopleCount_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(peopleCount, "People count");
        }

        private void PeopleCount_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(peopleCount, "People count");
        }

        private void Language_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(language, "Language");
        }

        private void Language_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(language, "Language");
        }

        private void Location_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(location, "Location");
        }
        private void Location_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(location, "Location");
        }
        private void TimeSpan_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(timeSpan, "Time span");
        }
        private void TimeSpan_LostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(timeSpan, "Time span");
        }

    }
}
