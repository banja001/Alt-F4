using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View.Guest1;
using booking.WPF.ViewModels;
using Domain.Model;
using Egor92.MvvmNavigation.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Formats.Asn1.AsnWriter;

namespace WPF.ViewModels.Guest1
{
    public class Guest1ViewViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private int selectedIndexTabControl;
        public int SelectedIndexTabControl
        {
            get { return selectedIndexTabControl; }
            set
            {
                if (selectedIndexTabControl != value)
                {
                    selectedIndexTabControl = value;
                    OnPropertyChanged();
                }
            }
        }

        private int userId;
        public string UserName { get; set; }
        public string Score { get; set; }

        private readonly UserService _userService;

        public ICommand OpenFirstTabCommand => new RelayCommand(OpenFirstTab);
        public ICommand OpenSecondTabCommand => new RelayCommand(OpenSecondTab);
        public ICommand OpenThirdTabCommand => new RelayCommand(OpenThirdTab);
        public ICommand OpenFourthTabCommand => new RelayCommand(OpenFourthTab);

        public Guest1ViewViewModel(int userId)
        {
            _userService = new UserService();

            this.userId = userId;

            InitializeStatus();

            SelectedIndexTabControl = 0;
        }
        private void InitializeStatus()
        {
            UserName = _userService.GetUserNameById(userId);
            Score = _userService.GetScoreById(userId).ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenFirstTab()
        {
            SelectedIndexTabControl = 0;
        }

        private void OpenSecondTab()
        {
            SelectedIndexTabControl = 1;
        }

        private void OpenThirdTab()
        {
            SelectedIndexTabControl = 2;
        }

        private void OpenFourthTab()
        {
            SelectedIndexTabControl = 3;
        }
    }
}
