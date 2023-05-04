using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.DTO
{
    public enum Taken{ Yes, No}
    public class ReservationChangeDTO
    {
        public int RequestId { get; set; }
        public int ReservationId { get; set; }
        public string AccommodationName{get;set;}
        public DateTime OldStartDate { get;set;}

        public DateTime OldEndDate { get;set;}
        public DateTime NewStartDate { get;set;}

        public DateTime NewEndDate { get; set; }
        public Taken IsTaken { get;set;}

        public ReservationChangeDTO(int id, string accommodationName, DateTime oldStartDate, DateTime oldNewDate, DateTime newStartDate, DateTime newEndDate, Taken isTaken, int requestId)
        {
            ReservationId = id;
            AccommodationName = accommodationName;
            OldStartDate = oldStartDate;
            OldEndDate = oldNewDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            IsTaken = isTaken;
            RequestId = requestId;
        }

        public ReservationChangeDTO()
        {
        }
    }
}
