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
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for ForumSelect.xaml
    /// </summary>
    public partial class ForumSelect : Page
    {
        public ForumSelect(int id)
        {
            InitializeComponent();
            DataContext=new ForumSelectViewModel(id);
        }
    }
}
