using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.Model
{
    public class Guest1Notifications : ISerializable
    {
        public int Id { get; set; }
        public int Guest1Id { get; set; }
        public int RequestId { get; set; }

        public Guest1Notifications() { }
        public Guest1Notifications(int id, int guest1Id, int requestId)
        {
            Id = id;
            Guest1Id = guest1Id;
            RequestId = requestId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest1Id.ToString(), RequestId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest1Id = Convert.ToInt32(values[1]);
            RequestId = Convert.ToInt32(values[2]);
        }
    }
}
