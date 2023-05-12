using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPF.ViewModels.Owner
{
    public class AccommodationStats2ViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public OwnerViewModel ownerViewModel;
        private int accommodationId;
        public List<int> YearList { get; set; }
        private int selectedYear;
        public int SelectedYear {
            get
            {
                return selectedYear;
            }
            set
            {
                if (value != selectedYear)
                {
                    selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                    YearSelectionChanged();
                }
            }
        }
        public List<string> Months =new List<string>{"January","February","March","April","May","June","July","August","September","October","November","December"};
        private List<string> monthList;
        public List<string> MonthList {
            get
            {
                return monthList;
            }
            set
            {
                if (value != monthList)
                {
                    monthList = value;
                    OnPropertyChanged("MonthList");
                }
            }
        }
        private string selectedMonth;
        public string SelectedMonth {
            get
            {
                return selectedMonth;
            }
            set
            {
                if (value != selectedMonth)
                {
                    selectedMonth = value;
                    OnPropertyChanged("SelectedMonth");
                }
            }
        }
        public AccommodationStats2ViewModel(int accId,OwnerViewModel ownerViewModel)
        {
            accommodationId = accId;
            this.ownerViewModel=ownerViewModel;
            YearList = new List<int>();
            MonthList = new List<string>();
            for(int i=2018;i<=DateTime.Now.Year; i++)
            {
                YearList.Add(i);
            }
            
        }

        public void YearSelectionChanged()
        {
            SelectedMonth = null;
            MonthList = Months.ToList();
            if (SelectedYear == DateTime.Now.Year)
            {
                MonthList=MonthList.Take(DateTime.Now.Month).ToList();
            } 
        }
    }
}
