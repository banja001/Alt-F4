using booking.DTO;
using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace booking.Domain.Model
{
    public class OwnerNotification : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserName { get; set; }

        public OwnerNotification() { }

        public OwnerNotification(int id, int ownerId, AccommodationLocationDTO accommodation, ReservedDates reservedDate, string userName)
        {
            Id = id;
            OwnerId = ownerId;
            AccommodationName = accommodation.Name;
            Location = accommodation.Location;
            StartDate = reservedDate.StartDate;
            EndDate = reservedDate.EndDate;
            UserName = userName;
        }

        public override string ToString()
        {
            return AccommodationName + " " + Location + " "
                + StartDate.ToString("dd/MM/yyyy") + "-" + EndDate.ToString("dd/MM/yyyy") + " is now free, because "
                + UserName + " has canceled his/her reservation";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), AccommodationName, Location, StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"), UserName};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            AccommodationName = values[2];
            Location = values[3];
            StartDate = DateTime.ParseExact(values[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            UserName = values[6];
        }
    }
}
