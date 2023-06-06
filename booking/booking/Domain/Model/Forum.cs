using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int CreatorId { get; set; }
        public bool Open { get; set; }

        public Forum() { }

        public Forum(int id, string location, int creatorId, bool open)
        {
            Id = id;
            Location = location;
            CreatorId = creatorId;
            Open = open;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Location, CreatorId.ToString(), Open.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location = values[1];
            CreatorId = Convert.ToInt32(values[2]);
            Open = values[3] == "true" ? true : false;
        }
    }
}
