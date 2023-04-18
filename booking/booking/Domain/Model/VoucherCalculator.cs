using booking.Injector;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Model
{
    public class VoucherCalculator
    {
        public VoucherCalculator() 
        {
        }
        public List<Voucher> CalculateVouchersForGuest2(List<Appointment> completedAppointments, User guest2)
        {
            List<Voucher> vouchers = new List<Voucher>();

            if (completedAppointments.Count() == 0)
                return vouchers;
            else
            {
                int idx = 0;
                foreach (var appointment in completedAppointments)
                {
                    int currentAppointmentIdx = completedAppointments.IndexOf(appointment);
                    if (((currentAppointmentIdx + 1) % 5 != 0))
                        continue;

                    List<Appointment> appointmentRange = completedAppointments.GetRange(idx, 5);
                    Appointment youngestAppointment = appointmentRange.MaxBy(a => a.End.Date);
                    DateAndTime expirationDate = new DateAndTime(youngestAppointment.End.Date.AddDays(180), "00:00");

                    Voucher possibleVoucher = new Voucher(-1,
                                                          youngestAppointment.End,
                                                          -1,
                                                          guest2.Id,
                                                          expirationDate,
                                                          false);
                    idx += 5;
                    vouchers.Add(possibleVoucher);
                }
            }
            return vouchers;
        }
    }
}
