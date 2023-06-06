using application.UseCases;
using booking.Commands;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    public class ForumSelectViewModel : BaseViewModel
    {
        private Forum selectedItem;
        public Forum SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }
        
        public ICommand ForumViewCommand => new RelayCommand(ForumViewClick);
        public ObservableCollection<Forum> ForumList { get; set; }
        public ForumService forumService;
        private int ownerId;
        public ForumSelectViewModel(int id)
        {
            ownerId = id;
            forumService = new ForumService();
            ForumList = new ObservableCollection<Forum>(forumService.GetAll());
        }

        public void ForumViewClick()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items", "Error");
            }
            else
            {
                ForumView win = new ForumView(selectedItem,ownerId);
                MainWindow.w.Main.Content = win;
            }
        }

    }
}
