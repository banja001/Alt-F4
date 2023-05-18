using booking.Domain.Model;
using booking.Model;
using booking.WPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace booking.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for MyRequestsView.xaml
    /// </summary>
    public partial class MyRequestsView : UserControl
    {
        public MyRequestsView()
        {
            InitializeComponent();
        }
        public MyRequestsView(User user)
        {
            InitializeComponent();
            this.DataContext = new MyRequestsViewModel(user);
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
        private void DescriptionLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(DescriptionTextBox, "Description");
        }

        private void DescriptionGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(DescriptionTextBox, "Description");
        }

        private void LanguageGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(LanguageTextBox, "Language");
        }
        private void LanguageLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(LanguageTextBox, "Language");
        }
        private void NumberOfGuestsGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(NumberOfGuestsTextBox, "NumberOfGuests");
        }
        private void NumberOfGuestsLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(NumberOfGuestsTextBox, "NumberOfGuests");
        }
        private void CityGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(CityTextBox, "City");
        }
        private void CityLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(CityTextBox, "City");
        }
        private void StateGotFocus(object sender, RoutedEventArgs e)
        {
            RemoveContent(StateTextBox, "State");
        }
        private void StateLostFocus(object sender, RoutedEventArgs e)
        {
            SetContentToDefault(StateTextBox, "State");
        }
    }
}
