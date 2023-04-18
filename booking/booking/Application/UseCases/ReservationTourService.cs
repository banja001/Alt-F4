using System;
using System.Collections.Generic;
using System.Text;
using booking.Model;
using booking.Repository;
using Domain.Model;
using Repositories;

namespace application.UseCases
{
    public class ReservationTourService
    {
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly TourRepository _tourRepository;
        private readonly VoucherRepository _voucherRepository;
        public ReservationTourService()
        {
            _reservationTourRepository= new ReservationTourRepository();
            _tourRepository= new TourRepository();
            _voucherRepository= new VoucherRepository();
        }

        public void GiveVouchers(Tour tour,User guide)
        {
            List<ReservationTour> reservationToursToDelete= new List<ReservationTour>();
            foreach (var rt in _reservationTourRepository.GetAll())
            {
                if (rt.Tour.Id == tour.Id)
                {
                    DateAndTime now= new DateAndTime(DateTime.Now, "00:00");
                    DateAndTime expire = new DateAndTime(DateTime.Now.AddDays(365), "00:00");
                    Voucher voucher=new Voucher(_voucherRepository.MakeID(),now,guide.Id,rt.User.Id,expire,false);
                    reservationToursToDelete.Add(rt);
                    _voucherRepository.Add(voucher);
                }
            }

            for (int i = 0; i < reservationToursToDelete.Count; i++)
            {
                _reservationTourRepository.Delete(reservationToursToDelete[i]);
            }
        }
    }
}
