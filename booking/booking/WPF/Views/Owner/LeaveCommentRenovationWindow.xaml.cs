using booking.application.UseCases;
using Domain.DTO;
using Repositories;
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
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for LeaveCommentRenovationWindow.xaml
    /// </summary>
    public partial class LeaveCommentRenovationWindow : Window
    {
        public LeaveCommentRenovationWindow(DateIntervalDTO s, int accid,RenovationDatesService ren)
        {
            InitializeComponent();
            DataContext = new LeaveCommentRenovationViewModel(s,accid,ren);
        }
    }
}
