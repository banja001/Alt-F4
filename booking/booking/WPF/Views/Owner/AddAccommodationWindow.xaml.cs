using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using WPF.ViewModels.Owner;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace booking.View
{
    public partial class AddAccommodationWindow : Page
    {
        public OwnerViewModel ownerWindow;

        public AddAccommodationWindow(OwnerViewModel win)
        {
            InitializeComponent();
            DataContext = new AddAccommodationViewModel(win);
            ownerWindow = win;

            Loaded += Window_Loaded;
        }


        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MinDaysToUseTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            NameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            MaxVisitorsTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            DaysToCancelTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            TypeComboBox.GetBindingExpression(ComboBox.TextProperty).UpdateSource();
            StateComboBox.GetBindingExpression(ComboBox.TextProperty).UpdateSource();
            CityComboBox.GetBindingExpression(ComboBox.TextProperty).UpdateSource();    
        }

        
    }
}
