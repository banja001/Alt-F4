using System;
using System.Collections.Generic;
using System.Text;
using booking.Injector;
using booking.Model;
using booking.Repository;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;

namespace application.UseCases
{
    public class ReservationTourService
    {
        private readonly IReservationTourRepository _reservationTourRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        public ReservationTourService()
        {
            _reservationTourRepository = Injector.CreateInstance<IReservationTourRepository>();
            _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _appointmentRepository = Injector.CreateInstance<AppointmentRepository>();
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
        public List<ReservationTour> GetAllByGuest2(User guest2)
        {
            return _reservationTourRepository.GetAll().FindAll(r => r.User.Id == guest2.Id); 
        }
    }
}
