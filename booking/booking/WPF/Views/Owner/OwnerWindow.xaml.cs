using booking.Repository;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Owner;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Contracts;
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
using booking.WPF.Views.Owner;
using booking.Domain.Model;
using Repositories;
using application.UseCases;
using WPF.ViewModels.Owner;
using WPF.Views.Owner;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Page
    {
        public OwnerViewModel OwnerModel;
        public OwnerWindow(int id)
        {
            InitializeComponent();
            OwnerModel = new OwnerViewModel(id);
            DataContext = OwnerModel;
            
        }

        
    }
}
