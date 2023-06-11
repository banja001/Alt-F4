using booking.application.UseCases;
using booking.Injector;
using booking.Model;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace application.UseCases
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly AppointmentService _appointmentService;
        private readonly VoucherCalculator _voucherCalculator;
        public VoucherService()
        {
            _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _appointmentService = new AppointmentService();
            _voucherCalculator = new VoucherCalculator();
        }
        public List<Voucher> GetUsableVouchersByGuest2(User guest2)
        {
            return ((List<Voucher>)_voucherRepository.GetAll()).FindAll(v => (v.Guest2Id == guest2.Id) && v.IsUsable() && v.IsUsed == false);
        }
        public void GenerateNewVouchersByGuest2(User guest2)
        {
            var completedAppointments = _appointmentService.GetCompletedAppointmentByGuest2(guest2);
            List<Voucher> vouchers = _voucherCalculator.CalculateVouchersForGuest2(completedAppointments, guest2);
            
            List<Voucher> v = vouchers.ToList();
            foreach(var voucher in v)
            {
                if (_voucherRepository.Contains(voucher))
                {
                    vouchers.Remove(voucher);
                }
            }

            _voucherRepository.AddRange(vouchers);
        }
        public void Remove(Voucher voucher)
        {
            _voucherRepository.Delete(voucher.Id);
        }
        public void Update(Voucher voucher)
        {
            _voucherRepository.Update(voucher);
        }
        public int MakeID() 
        {
            return _voucherRepository.MakeID();
        }
        public void Add(Voucher v)
        {
            _voucherRepository.Add(v);
        }
    }
}
