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
        public string Name { get; set; }
        public int LocationId { get; set; }
        public int CreatorId { get; set; }
        public bool Open { get; set; }

        public Forum() { }

        public Forum(int id, string name, int locationId, int creatorId, bool open)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            CreatorId = creatorId;
            Open = open;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LocationId.ToString(), CreatorId.ToString(), Open.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            CreatorId = Convert.ToInt32(values[3]);
            Open = values[4] == "true" ? true : false;
        }
    }
}
