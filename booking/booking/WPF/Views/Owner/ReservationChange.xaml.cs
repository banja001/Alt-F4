using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
using booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace booking.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for ReservationChange.xaml
    /// </summary>
    public partial class ReservationChange : Window
    {
        public List<ReservationRequests> requests;
        public ReservationRequestsRepository repository;
        public OwnerWindow ownerWindow;

        public ObservableCollection<ReservationChangeDTO> requestsObservable { get; set; }
        public ReservationChangeDTO SelectedItem { get; set; }
        public ReservationChange(OwnerWindow win)
        {
            InitializeComponent();
            DataContext = this;
            ownerWindow = win;
            repository = new ReservationRequestsRepository();
            requestsObservable = new ObservableCollection<ReservationChangeDTO>();
            requests = repository.GetPostpone();
            foreach(ReservationRequests res in requests)
            {
                ReservationChangeDTO resTemp = new ReservationChangeDTO();
                ReservedDates r=win.reservedDates.Find(s => res.ReservationId == s.Id);
                Accommodation a=win.accommodations.Find(s => r.AccommodationId == s.Id);

                if (a.OwnerId != win.OwnerId || res.isCanceled==true) continue;
                resTemp.RequestId = res.Id;
                resTemp.ReservationChangeId = res.ReservationId;
                resTemp.AccommodationName = a.Name;
                resTemp.OldStartDate = r.StartDate;
                resTemp.OldEndDate=r.EndDate;
                resTemp.NewStartDate = res.NewStartDate;
                resTemp.NewEndDate = res.NewEndDate;

                ReservedDates rr= win.reservedDates.Find(s =>!(s.EndDate<res.NewStartDate) && !(s.StartDate> res.NewEndDate) && (s.AccommodationId==a.Id) && (resTemp.OldStartDate!=s.StartDate) && (resTemp.OldEndDate != s.StartDate));
                //ovo testirat
                resTemp.IsTaken =rr==null ? Taken.No : Taken.Yes;

                requestsObservable.Add(resTemp);

            }




        }

        private void AllowClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            ReservedDates r=ownerWindow.reservedDates.Find(s => s.Id==SelectedItem.ReservationChangeId);
            Accommodation a = ownerWindow.accommodations.Find(s => s.Id==r.AccommodationId);
            List<ReservedDates> rr = ownerWindow.reservedDates.FindAll(s => !(s.EndDate < SelectedItem.NewStartDate) && !(s.StartDate > SelectedItem.NewEndDate) && (s.AccommodationId == a.Id)
            && (SelectedItem.OldStartDate != s.StartDate) && (SelectedItem.OldEndDate != s.StartDate));
            r.StartDate = SelectedItem.NewStartDate;
            r.EndDate = SelectedItem.NewEndDate;
            ownerWindow.reservedDatesRepository.Update(r);
            foreach(ReservedDates res in rr)
            {
                ownerWindow.reservedDatesRepository.Remove(res);
                repository.Remove(requests.Find(s=>res.Id==s.ReservationId));
            }

           
            repository.UpdateAllow( requests.Find(s=>SelectedItem.RequestId==s.Id));

            requestsObservable.Remove(SelectedItem);
        }

        private void DeclineClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            LeaveComment win = new LeaveComment(this);
            win.ShowDialog();

        }
    }
}
