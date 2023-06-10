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
        public bool VeryUseful { get; set; }

        public Forum() { }

        public Forum(int id, string location, int creatorId, bool open, bool veryUseful)
        {
            Id = id;
            Location = location;
            CreatorId = creatorId;
            Open = open;
            VeryUseful = veryUseful;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Location, CreatorId.ToString(), Open.ToString(), VeryUseful.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location = values[1];
            CreatorId = Convert.ToInt32(values[2]);
            Open = values[3] == "True" ? true : false;
            VeryUseful = values[4] == "True" ? true : false;
        }
    }
}
