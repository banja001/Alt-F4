﻿using booking.Manager;
using booking.Model;
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
    /// Interaction logic for AddAccommodationWindow.xaml
    /// </summary>
    public partial class AddAccommodationWindow : Window
    {
        private AccommodationManager accommodationManager;
        

        public AddAccommodationWindow(AccommodationManager accMen)
        {
            InitializeComponent();
            DataContext = this;
            accommodationManager = accMen;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Accommodation a = new Accommodation(1,NameTextBox.Text,LocationTextBox.Text,TypeComboBox.Text,Convert.ToInt32(MaxVisitorsTextBox.Text), Convert.ToInt32(MinDaysToUseTextBox.Text), Convert.ToInt32(DaysToCancelTextBox.Text));
            accommodationManager.AddAccommodation(a);
            
        }







        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
