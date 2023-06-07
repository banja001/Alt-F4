using Domain.Model;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for ForumView.xaml
    /// </summary>
    public partial class ForumView : Page
    {
        public ForumViewViewModel win;
        public ForumView(Forum select,int id)
        {
            InitializeComponent();
            win= new ForumViewViewModel(select,id);
            DataContext = win;
        }
        

       
    }
}
