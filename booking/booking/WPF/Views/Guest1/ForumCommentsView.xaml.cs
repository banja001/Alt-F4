using Domain.Model;
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
using WPF.ViewModels.Guest1;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ForumCommentsView.xaml
    /// </summary>
    public partial class ForumCommentsView : Window
    {
        public ForumCommentsView(Forum selecteForum, int userId)
        {
            InitializeComponent();

            DataContext = new ForumCommentsViewModel(selecteForum, userId);
        }
    }
}
