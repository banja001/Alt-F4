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
using WPF.ViewModels;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for ParametarOfStatisticsForTourCreationWindow.xaml
    /// </summary>
    public partial class ParametarOfStatisticsForTourCreationWindow : Window
    {
        public ParametarOfStatisticsForTourCreationWindow(bool DemoOn)
        {
            InitializeComponent();
            DataContext = new ParametarOfStatisticsForTourCreationVIewModel(DemoOn);
        }
    }
}
