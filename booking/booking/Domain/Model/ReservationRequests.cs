using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace booking.Domain.Model
{
    public enum RequestType { Postpone, Cancel }//ne treba
    public enum RequestStatus { Pending, Postponed, Canceled }

    public class ReservationRequests : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public RequestType RequestType { get; set; }

        public RequestStatus isCanceled { get; set; }//prihvaceno, odbijeno, /

        public string Comment { get; set; }


        public ReservationRequests() { }

        public ReservationRequests(int id, ReservedDates reservedDate, string requestType,string comment = "", RequestStatus isCanceled = RequestStatus.Pending)
        {
            Id = id;
            ReservationId = reservedDate.Id;
            NewStartDate = reservedDate.StartDate;
            NewEndDate = reservedDate.EndDate;
            RequestType = requestType == "Postpone" ? RequestType.Postpone : RequestType.Cancel;
            Comment = comment;
            this.isCanceled = isCanceled;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), NewStartDate.ToString("dd/MM/yyyy"), NewEndDate.ToString("dd/MM/yyyy"), RequestType.ToString(),isCanceled.ToString(),Comment};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            NewStartDate = DateTime.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            NewEndDate = DateTime.ParseExact(values[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            RequestType = (values[4] == "Postpone") ? RequestType.Postpone : RequestType.Cancel;
            if (values[5] == "Pending") 
                isCanceled = RequestStatus.Pending;
            else if (values[5] == "Postponed") 
                isCanceled = RequestStatus.Postponed;
            else isCanceled = RequestStatus.Canceled;
            Comment = values[6];
        }
    }
}
