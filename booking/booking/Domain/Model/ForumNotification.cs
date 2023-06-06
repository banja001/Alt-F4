using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ForumNotification : ISerializable
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }

        public int OwnerId { get; set; }

        public ForumNotification()
        {
        }

        public ForumNotification(int id, string location, string userName, int ownerId)
        {
            this.Id = id;
            Location = location;
            UserName = userName;
            OwnerId = ownerId;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Location,UserName,OwnerId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location=values[1];
            UserName = values[2];
            OwnerId = Convert.ToInt32(values[3]);
        }


    }
}
